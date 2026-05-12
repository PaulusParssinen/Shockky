using System.Collections.Immutable;

using Microsoft.CodeAnalysis;

using Shockky.SourceGeneration.Diagnostics;
using Shockky.SourceGeneration.Extensions;
using Shockky.SourceGeneration.Helpers;
using Shockky.SourceGeneration.Models;

namespace Shockky.SourceGeneration;

public sealed partial class ShockwaveItemGenerator
{
    private static Result<ShockwaveItemInfo?> ExtractShockwaveItemInfo(
        GeneratorAttributeSyntaxContext context,
        CancellationToken token)
    {
        using ImmutableArrayBuilder<DiagnosticInfo> diagnostics = ImmutableArrayBuilder<DiagnosticInfo>.Rent();

        var typeSymbol = (INamedTypeSymbol)context.TargetSymbol;

        if (!typeSymbol.TryGetAttributeWithFullyQualifiedMetadataName(ShockwaveItemAttributeName, out AttributeData? attributeData))
        {
            return new Result<ShockwaveItemInfo?>(null, diagnostics.ToImmutable());
        }

        bool bigEndian = attributeData.GetNamedArgument("BigEndian", true);
        bool ignoreContainerEndianness = attributeData.GetNamedArgument("IgnoreContainerEndianness", false);
        bool generateSerialization = attributeData.GetNamedArgument("GenerateSerialization", false);
        bool sizePrefixed = attributeData.GetNamedArgument("SizePrefixed", false);
        int[]? expectedSizes = attributeData.GetNamedArrayArgument<int>("ExpectedSizes");

        using ImmutableArrayBuilder<PropertySerializationInfo> properties = ImmutableArrayBuilder<PropertySerializationInfo>.Rent();
        int? maxEntryIndex = null;
        HashSet<int> entryIndexes = [];

        foreach (IPropertySymbol property in typeSymbol.GetMembers().OfType<IPropertySymbol>())
        {
            token.ThrowIfCancellationRequested();
            if (property.IsStatic || property.SetMethod is null) continue;

            var propInfo = GetPropertyInfo(property);
            properties.Add(propInfo);

            if (propInfo.Kind is PropertyKind.Entry)
            {
                if (propInfo.EntryIndex < 0)
                {
                    diagnostics.Add(DiagnosticDescriptors.DuplicateEntryIndex, property, typeSymbol.Name, property.Name, propInfo.EntryIndex);
                }
                else if (!entryIndexes.Add(propInfo.EntryIndex))
                {
                    diagnostics.Add(DiagnosticDescriptors.DuplicateEntryIndex, property, typeSymbol.Name, property.Name, propInfo.EntryIndex);
                }

                maxEntryIndex = maxEntryIndex.HasValue ? Math.Max(maxEntryIndex.Value, propInfo.EntryIndex) : propInfo.EntryIndex;
            }

            AddPropertyDiagnostics(diagnostics, typeSymbol, property, propInfo, generateSerialization);
        }

        PropertySerializationInfo[] propertyArray = properties.ToArray();

        var info = new ShockwaveItemInfo(
            HierarchyInfo.From(typeSymbol),
            bigEndian,
            ignoreContainerEndianness,
            generateSerialization,
            sizePrefixed,
            expectedSizes.ToImmutableArray(),
            properties.ToImmutable(),
            maxEntryIndex,
            HasGetBodySizeMethod(typeSymbol),
            HasWriteToMethod(typeSymbol));

        return new Result<ShockwaveItemInfo?>(info, diagnostics.ToImmutable());
    }

    private static PropertySerializationInfo GetPropertyInfo(IPropertySymbol property)
    {
        int padBefore = 0;
        int padAfter = 0;
        string? condition = null;
        ParseKind parseKind = ParseKind.Default;
        PropertyKind kind = PropertyKind.Sequential;
        int entryIndex = -1;

        foreach (var attrData in property.GetAttributes())
        {
            string? attrName = attrData.AttributeClass?.GetFullyQualifiedMetadataName();
            if (attrName is null) continue;

            switch (attrName)
            {
                case PadBeforeAttributeName when attrData.ConstructorArguments is [{ Value: int value }]:
                    padBefore = value;
                    break;
                case PadAfterAttributeName when attrData.ConstructorArguments is [{ Value: int value }]:
                    padAfter = value;
                    break;
                case ConditionAttributeName when attrData.ConstructorArguments is [{ Value: string value }]:
                    condition = value;
                    break;
                case ParseStringAsAttributeName when attrData.ConstructorArguments is [{ Value: int value }]:
                    parseKind = (ParseKind)value;
                    break;
                case EntryAttributeName when attrData.ConstructorArguments is [{ Value: int value }]:
                    kind = PropertyKind.Entry;
                    entryIndex = value;
                    break;
            }
        }

        // Check if property type is a SizePrefixed ShockwaveItem
        bool isSizePrefixed = false;
        if (kind is PropertyKind.Sequential &&
            property.Type is INamedTypeSymbol typeSymbol &&
            typeSymbol.TryGetAttributeWithFullyQualifiedMetadataName(ShockwaveItemAttributeName, out var itemAttr))
        {
            isSizePrefixed = itemAttr.GetNamedArgument("SizePrefixed", false);
        }

        bool isNullable = property.Type is { NullableAnnotation: NullableAnnotation.Annotated } or
            INamedTypeSymbol { IsGenericType: true, ConstructedFrom.SpecialType: SpecialType.System_Nullable_T };

        ITypeSymbol serializationType = GetSerializationType(property.Type);
        TypeSerializationKind serializationKind = GetSerializationKind(serializationType);
        string? enumUnderlyingTypeFullName = serializationType is INamedTypeSymbol { TypeKind: TypeKind.Enum } enumType ?
            enumType.EnumUnderlyingType?.GetFullyQualifiedNameWithNullabilityAnnotations() : null;

        return new PropertySerializationInfo(
            property.Name,
            property.Type.GetFullyQualifiedNameWithNullabilityAnnotations(),
            padBefore,
            padAfter,
            condition,
            parseKind,
            isNullable,
            kind,
            entryIndex,
            serializationKind,
            enumUnderlyingTypeFullName,
            isSizePrefixed);
    }

    private static ITypeSymbol GetSerializationType(ITypeSymbol type)
    {
        if (type is INamedTypeSymbol { IsGenericType: true, ConstructedFrom.SpecialType: SpecialType.System_Nullable_T } namedType)
        {
            return namedType.TypeArguments[0];
        }

        return type;
    }

    private static TypeSerializationKind GetSerializationKind(ITypeSymbol type)
    {
        if (type.TypeKind is TypeKind.Enum) return TypeSerializationKind.Enum;

        if (type.HasInterfaceWithFullyQualifiedMetadataName(FullyQualifiedIShockwaveItemName)) return TypeSerializationKind.ShockwaveItem;

        if (type.TryGetAttributeWithFullyQualifiedMetadataName(ShockwaveItemAttributeName, out _)) return TypeSerializationKind.ShockwaveItem;

        return type.SpecialType switch
        {
            SpecialType.System_Byte => TypeSerializationKind.Byte,
            SpecialType.System_Boolean => TypeSerializationKind.Boolean,
            SpecialType.System_Int16 => TypeSerializationKind.Int16,
            SpecialType.System_UInt16 => TypeSerializationKind.UInt16,
            SpecialType.System_Int32 => TypeSerializationKind.Int32,
            SpecialType.System_UInt32 => TypeSerializationKind.UInt32,
            SpecialType.System_UInt64 => TypeSerializationKind.UInt64,
            SpecialType.System_Double => TypeSerializationKind.Double,
            SpecialType.System_String => TypeSerializationKind.String,
            _ => TypeSerializationKind.Unsupported,
        };
    }

    private static void AddPropertyDiagnostics(
        ImmutableArrayBuilder<DiagnosticInfo> diagnostics,
        INamedTypeSymbol typeSymbol,
        IPropertySymbol property,
        PropertySerializationInfo propInfo,
        bool generateSerialization)
    {
        if (propInfo.ParseKind is not ParseKind.Default && propInfo.SerializationKind is not TypeSerializationKind.String)
        {
            diagnostics.Add(DiagnosticDescriptors.ParseStringAsRequiresString, property, typeSymbol.Name, property.Name, propInfo.TypeFullName);
        }

        if (propInfo.SerializationKind is TypeSerializationKind.ShockwaveItem) return;

        if (propInfo.SerializationKind is TypeSerializationKind.Unsupported)
        {
            diagnostics.Add(DiagnosticDescriptors.UnsupportedShockwaveItemPropertyType, property, typeSymbol.Name, property.Name, propInfo.TypeFullName);
        }

        if (!generateSerialization) return;

        if (propInfo.Kind is PropertyKind.Sequential && propInfo.IsNullable && propInfo.Condition is null)
        {
            diagnostics.Add(DiagnosticDescriptors.NullableSequentialPropertyRequiresCondition, property, typeSymbol.Name, property.Name);
        }

        if (propInfo.Kind is PropertyKind.Sequential && propInfo.Condition is not null && !propInfo.IsNullable)
        {
            diagnostics.Add(DiagnosticDescriptors.ConditionalPropertyCannotBeSized, property, typeSymbol.Name, property.Name);
        }

        if (propInfo.Kind is PropertyKind.Entry && !propInfo.IsNullable)
        {
            diagnostics.Add(DiagnosticDescriptors.NonNullableEntryMayBeAbsent, property, typeSymbol.Name, property.Name);
        }
    }

    private static bool HasGetBodySizeMethod(INamedTypeSymbol typeSymbol)
    {
        foreach (ISymbol symbol in typeSymbol.GetMembers("GetBodySize"))
        {
            if (symbol is IMethodSymbol { IsStatic: false, Parameters.Length: 1 } method &&
                method.Parameters[0].Type.HasFullyQualifiedMetadataName(FullyQualifiedWriterOptionsMetadataName))
            {
                return true;
            }
        }

        return false;
    }

    private static bool HasWriteToMethod(INamedTypeSymbol typeSymbol)
    {
        foreach (ISymbol symbol in typeSymbol.GetMembers("WriteTo"))
        {
            if (symbol is IMethodSymbol { IsStatic: false, Parameters.Length: 2 } method &&
                method.Parameters[0].Type.HasFullyQualifiedMetadataName(FullyQualifiedShockwaveWriterMetadataName) &&
                method.Parameters[1].Type.HasFullyQualifiedMetadataName(FullyQualifiedWriterOptionsMetadataName))
            {
                return true;
            }
        }

        return false;
    }
}