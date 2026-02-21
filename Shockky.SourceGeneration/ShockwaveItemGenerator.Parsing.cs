using System.Collections.Immutable;

using Microsoft.CodeAnalysis;

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

        using ImmutableArrayBuilder<PropertyReadInfo> properties = ImmutableArrayBuilder<PropertyReadInfo>.Rent();
        string? offsetTableProperty = null;
        int? maxEntryIndex = null;

        foreach (IPropertySymbol property in typeSymbol.GetMembers().OfType<IPropertySymbol>())
        {
            token.ThrowIfCancellationRequested();
            if (property.IsStatic || property.SetMethod is null) continue;

            var propInfo = GetPropertyInfo(property);
            properties.Add(propInfo);

            if (propInfo.Kind is PropertyKind.OffsetTable)
            {
                offsetTableProperty = property.Name;
            }
            else if (propInfo.Kind is PropertyKind.Entry)
            {
                maxEntryIndex = maxEntryIndex.HasValue ? Math.Max(maxEntryIndex.Value, propInfo.EntryIndex) : propInfo.EntryIndex;
            }
        }

        var info = new ShockwaveItemInfo(
            HierarchyInfo.From(typeSymbol),
            bigEndian,
            properties.ToImmutable(),
            offsetTableProperty,
            maxEntryIndex);

        return new Result<ShockwaveItemInfo?>(info, diagnostics.ToImmutable());
    }

    private static Result<HeaderInfo?> GetHeaderInfo(
        GeneratorAttributeSyntaxContext context,
        CancellationToken token)
    {
        using ImmutableArrayBuilder<DiagnosticInfo> diagnostics = ImmutableArrayBuilder<DiagnosticInfo>.Rent();

        var typeSymbol = (INamedTypeSymbol)context.TargetSymbol;

        if (!typeSymbol.TryGetAttributeWithFullyQualifiedMetadataName(HeaderAttributeName, out AttributeData? attrData))
        {
            return new Result<HeaderInfo?>(null, diagnostics.ToImmutable());
        }

        int[]? expectedSizes = attrData.GetNamedArrayArgument<int>("ExpectedSizes");

        // Inherit endianness from containing type if it has [ShockwaveItem]
        bool bigEndian = true;
        if (typeSymbol.ContainingType is { } containingType &&
            containingType.TryGetAttributeWithFullyQualifiedMetadataName(ShockwaveItemAttributeName, out var parentAttr))
        {
            bigEndian = parentAttr.GetNamedArgument("BigEndian", true);
        }

        using ImmutableArrayBuilder<PropertyReadInfo> properties = ImmutableArrayBuilder<PropertyReadInfo>.Rent();

        foreach (IPropertySymbol prop in typeSymbol.GetMembers().OfType<IPropertySymbol>())
        {
            token.ThrowIfCancellationRequested();
            if (prop.IsStatic || prop.SetMethod is null) continue;

            properties.Add(GetPropertyInfo(prop));
        }

        var info = new HeaderInfo(
            HierarchyInfo.From(typeSymbol),
            bigEndian,
            expectedSizes.ToImmutableArray(),
            properties.ToImmutable());

        return new Result<HeaderInfo?>(info, diagnostics.ToImmutable());
    }

    private static PropertyReadInfo GetPropertyInfo(IPropertySymbol property)
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
                case ParseAsAttributeName when attrData.ConstructorArguments is [{ Value: int value }]:
                    parseKind = (ParseKind)value;
                    break;
                case OffsetTableAttributeName:
                    kind = PropertyKind.OffsetTable;
                    break;
                case EntryAttributeName when attrData.ConstructorArguments is [{ Value: int value }]:
                    kind = PropertyKind.Entry;
                    entryIndex = value;
                    break;
            }
        }

        // Check if property type has [Header] attribute
        if (kind is PropertyKind.Sequential &&
            property.Type is INamedTypeSymbol typeSymbol &&
            typeSymbol.HasAttributeWithFullyQualifiedMetadataName(HeaderAttributeName))
        {
            kind = PropertyKind.Header;
        }

        bool isNullable = property.Type is { NullableAnnotation: NullableAnnotation.Annotated } or
            INamedTypeSymbol { IsGenericType: true, ConstructedFrom.SpecialType: SpecialType.System_Nullable_T };

        return new PropertyReadInfo(
            property.Name,
            property.Type.GetFullyQualifiedNameWithNullabilityAnnotations(),
            padBefore,
            padAfter,
            condition,
            parseKind,
            isNullable,
            kind,
            entryIndex);
    }
}