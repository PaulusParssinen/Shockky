using Shockky.SourceGeneration.Extensions;
using Shockky.SourceGeneration.Helpers;
using Shockky.SourceGeneration.Models;

namespace Shockky.SourceGeneration;

public partial class ShockwaveItemGenerator
{
    private const string FullyQualifiedShockwaveReaderName = "global::Shockky.IO.ShockwaveReader";
    private const string FullyQualifiedReaderContextName = "global::Shockky.IO.ReaderContext";
    private const string FullyQualifiedOffsetTableName = "global::Shockky.IO.OffsetTable";

    private static void WriteShockwaveItemReadMethod(IndentedTextWriter writer, ShockwaveItemInfo item)
    {
        item.Hierarchy.WriteSyntax(item, writer, [], [static (info, writer) => WriteShockwaveItemReadMethodBody(info, writer)]);
    }

    private static void WriteShockwaveItemReadMethodBody(ShockwaveItemInfo item, IndentedTextWriter writer)
    {
        string typeName = item.Hierarchy.Hierarchy[0].QualifiedName;
        string suffix = item.BigEndian ? "BigEndian" : "LittleEndian";

        string ctorParams = item.SizePrefixed
            ? $"ref {FullyQualifiedShockwaveReaderName} input, {FullyQualifiedReaderContextName} context, int bodySize"
            : $"ref {FullyQualifiedShockwaveReaderName} input, {FullyQualifiedReaderContextName} context";

        writer.WriteGeneratedAttributes(nameof(ShockwaveItemGenerator));
        writer.WriteLine("[global::System.Diagnostics.CodeAnalysis.SetsRequiredMembers]");
        using (writer.WriteBlock($"public {typeName}({ctorParams})"))
        {
            if (!item.ExpectedSizes.IsEmpty)
            {
                var sizes = string.Join(" || ", item.ExpectedSizes.AsSpan().ToArray().Select(size => $"bodySize == {size}"));
                writer.WriteLine($"global::System.Diagnostics.Debug.Assert({sizes}, $\"Unexpected body size {{bodySize}}\");");
                writer.WriteLine();
            }

            if (item.IgnoreContainerEndianness)
            {
                writer.WriteLine("bool previousReverseEndianness = input.ReverseEndianness;");
                writer.WriteLine("input.ReverseEndianness = false;");
            }

            using (item.IgnoreContainerEndianness ? writer.WriteBlock("try") : default)
            {
                foreach (var prop in item.Properties.AsSpan())
                {
                    if (prop.Kind is PropertyKind.Sequential)
                    {
                        if (prop.SerializationKind is TypeSerializationKind.ShockwaveItem)
                        {
                            WriteShockwaveItemPropertyRead(writer, prop, suffix);
                        }
                        else
                        {
                            WriteSequentialPropertyRead(writer, prop, suffix);
                        }
                    }
                }

                if (item.MaxEntryIndex.HasValue)
                {
                    WriteOffsetTableEntriesRead(writer, item, suffix);
                }
            }

            if (item.IgnoreContainerEndianness)
            {
                using (writer.WriteBlock("finally"))
                {
                    writer.WriteLine("input.ReverseEndianness = previousReverseEndianness;");
                }
            }
        }

        writer.WriteLine();
        writer.WriteLine($"public {typeName}() {{ }}");

        writer.WriteLine();
        if (item.SizePrefixed)
        {
            writer.WriteLine($"public static {typeName} Read(ref {FullyQualifiedShockwaveReaderName} input, {FullyQualifiedReaderContextName} context, int bodySize) => new(ref input, context, bodySize);");
        }
        else
        {
            writer.WriteLine($"public static {typeName} Read(ref {FullyQualifiedShockwaveReaderName} input, {FullyQualifiedReaderContextName} context) => new(ref input, context);");
        }

    }

    private static void WriteSequentialPropertyRead(IndentedTextWriter writer, PropertySerializationInfo prop, string suffix)
    {
        if (prop.PadBefore > 0)
            writer.WriteLine($"input.Advance({prop.PadBefore});");

        if (prop.Condition is not null)
        {
            writer.WriteLine($"if ({prop.Condition})");
            writer.IncreaseIndent();
        }

        string readExpr = GetReadExpression(prop, suffix, null);
        writer.WriteLine($"{prop.Name} = {readExpr};");

        if (prop.Condition is not null)
            writer.DecreaseIndent();

        if (prop.PadAfter > 0)
            writer.WriteLine($"input.Advance({prop.PadAfter});");
    }

    private static void WriteShockwaveItemPropertyRead(IndentedTextWriter writer, PropertySerializationInfo prop, string suffix)
    {
        string itemType = prop.TypeFullName.TrimEnd('?');
        if (prop.IsSizePrefixed)
        {
            writer.WriteLine($"int bodySize = input.ReadInt32{suffix}();");
            writer.WriteLine($"{prop.Name} = {itemType}.Read(ref input, context, bodySize);");
        }
        else
        {
            writer.WriteLine($"{prop.Name} = {itemType}.Read(ref input, context);");
        }
        writer.WriteLine();
    }

    private static void WriteOffsetTableEntriesRead(IndentedTextWriter writer, ShockwaveItemInfo item, string suffix)
    {
        PropertySerializationInfo[] entries = GetEntryProperties(item);

        writer.WriteLine($"{FullyQualifiedOffsetTableName} offsets = {FullyQualifiedOffsetTableName}.Read(ref input);");
        writer.WriteLine();

        using (writer.WriteBlock("for (int entryIndex = 0; entryIndex < offsets.Length; entryIndex++)"))
        {
            writer.WriteLine("int entrySize = offsets.GetEntrySize(entryIndex);");
            writer.WriteLine("if (entrySize == 0) continue;");
            writer.WriteLine();
            writer.WriteLine("int entryStart = input.Position;");
            using (writer.WriteBlock("switch (entryIndex)"))
            {
                foreach (PropertySerializationInfo entry in entries)
                {
                    writer.WriteLine($"case {entry.EntryIndex}:");
                    writer.IncreaseIndent();
                    writer.WriteLine($"{entry.Name} = {GetReadExpression(entry, suffix, "entrySize")};");
                    writer.WriteLine("break;");
                    writer.DecreaseIndent();
                }

                writer.WriteLine("default:");
                writer.IncreaseIndent();
                writer.WriteLine("input.Advance(entrySize);");
                writer.WriteLine("break;");
                writer.DecreaseIndent();
            }
            writer.WriteLine();
            writer.WriteLine("int consumed = input.Position - entryStart;");
            using (writer.WriteBlock("if (consumed < entrySize)"))
            {
                writer.WriteLine("input.Advance(entrySize - consumed);");
            }
        }
        writer.WriteLine();
    }

    private static string GetReadExpression(PropertySerializationInfo prop, string suffix, string? entrySize)
    {
        if (prop.ParseKind == ParseKind.PString) return "input.ReadPString()";
        if (prop.ParseKind == ParseKind.CString) return "input.ReadCString()";
        if (prop.ParseKind == ParseKind.FixedBytes && entrySize is not null)
            return $"input.ReadString({entrySize})";

        return prop.SerializationKind switch
        {
            TypeSerializationKind.Byte => "input.ReadByte()",
            TypeSerializationKind.Boolean => "input.ReadBoolean()",
            TypeSerializationKind.Int16 => $"input.ReadInt16{suffix}()",
            TypeSerializationKind.UInt16 => $"input.ReadUInt16{suffix}()",
            TypeSerializationKind.Int32 => $"input.ReadInt32{suffix}()",
            TypeSerializationKind.UInt32 => $"input.ReadUInt32{suffix}()",
            TypeSerializationKind.UInt64 => $"input.ReadUInt64{suffix}()",
            TypeSerializationKind.Double => $"input.ReadDouble{suffix}()",
            TypeSerializationKind.String => "input.ReadPString()",
            TypeSerializationKind.Enum => $"({GetBaseTypeName(prop)}){GetReadExpressionForUnderlying(prop.EnumUnderlyingTypeFullName, suffix)}",
            _ => $"default({GetBaseTypeName(prop)})"
        };
    }

    private static string GetReadExpressionForUnderlying(string? underlyingTypeName, string suffix)
    {
        return GetSerializationKindForTypeName(underlyingTypeName) switch
        {
            TypeSerializationKind.Byte => "input.ReadByte()",
            TypeSerializationKind.Int16 => $"input.ReadInt16{suffix}()",
            TypeSerializationKind.UInt16 => $"input.ReadUInt16{suffix}()",
            TypeSerializationKind.Int32 => $"input.ReadInt32{suffix}()",
            TypeSerializationKind.UInt32 => $"input.ReadUInt32{suffix}()",
            TypeSerializationKind.UInt64 => $"input.ReadUInt64{suffix}()",
            _ => $"input.ReadInt32{suffix}()",
        };
    }

    private static string GetBaseTypeName(PropertySerializationInfo prop) => prop.TypeFullName.TrimEnd('?');

    private static TypeSerializationKind GetSerializationKindForTypeName(string? typeName)
    {
        // lang=C#
        return typeName switch
        {
            "byte" or "global::System.Byte" => TypeSerializationKind.Byte,
            "sbyte" or "global::System.SByte" => TypeSerializationKind.SByte,
            "bool" or "global::System.Boolean" => TypeSerializationKind.Boolean,
            "short" or "global::System.Int16" => TypeSerializationKind.Int16,
            "ushort" or "global::System.UInt16" => TypeSerializationKind.UInt16,
            "int" or "global::System.Int32" => TypeSerializationKind.Int32,
            "uint" or "global::System.UInt32" => TypeSerializationKind.UInt32,
            "ulong" or "global::System.UInt64" => TypeSerializationKind.UInt64,
            "double" or "global::System.Double" => TypeSerializationKind.Double,
            _ => TypeSerializationKind.Unsupported,
        };
    }

    private static PropertySerializationInfo[] GetEntryProperties(ShockwaveItemInfo item)
    {
        return item.Properties.AsSpan().ToArray()
            .Where(static prop => prop.Kind is PropertyKind.Entry)
            .OrderBy(static prop => prop.EntryIndex)
            .ToArray();
    }
}