using Shockky.SourceGeneration.Extensions;
using Shockky.SourceGeneration.Helpers;
using Shockky.SourceGeneration.Models;

namespace Shockky.SourceGeneration;

public partial class ShockwaveItemGenerator
{
    private const string FullyQualifiedShockwaveWriterName = "global::Shockky.IO.ShockwaveWriter";
    private const string FullyQualifiedWriterOptionsName = "global::Shockky.IO.WriterOptions";

    private static void WriteShockwaveItemWriteMethod(IndentedTextWriter writer, ShockwaveItemInfo item)
    {
        item.Hierarchy.WriteSyntax(item, writer, [], [static (info, writer) => WriteShockwaveItemWriteMethodBody(info, writer)]);
    }

    private static void WriteShockwaveItemWriteMethodBody(ShockwaveItemInfo item, IndentedTextWriter writer)
    {
        if (!item.HasExistingGetBodySize)
        {
            WriteShockwaveItemGetBodySize(writer, item);
        }

        if (!item.HasExistingWriteTo)
        {
            if (!item.HasExistingGetBodySize) writer.WriteLine();
            WriteShockwaveItemWriteTo(writer, item);
        }
    }

    private static void WriteShockwaveItemGetBodySize(IndentedTextWriter writer, ShockwaveItemInfo item)
    {
        writer.WriteGeneratedAttributes(nameof(ShockwaveItemGenerator));
        using (writer.WriteBlock($"public int GetBodySize({FullyQualifiedWriterOptionsName} options)"))
        {
            if (item.SizePrefixed)
            {
                writer.WriteLine("int size = sizeof(int);");
            }
            else
            {
                writer.WriteLine("int size = 0;");
            }
            writer.WriteLine();

            int pendingFixed = 0;
            foreach (PropertySerializationInfo prop in item.Properties.AsSpan())
            {
                if (prop.Kind is not PropertyKind.Sequential) continue;

                if (prop.IsArray || prop.IsList)
                {
                    int fixedSize = TryGetFixedPropertySizeContribution(prop);
                    if (fixedSize > 0)
                    {
                        pendingFixed += fixedSize;
                    }
                    else
                    {
                        FlushPendingSize(writer, ref pendingFixed);
                        WriteCollectionPropertySize(writer, prop);
                    }
                }
                else if (prop.SerializationKind is TypeSerializationKind.ShockwaveItem)
                {
                    FlushPendingSize(writer, ref pendingFixed);
                    writer.WriteLine($"size += {prop.Name}.GetBodySize(options);");
                }
                else
                {
                    int fixedSize = TryGetFixedPropertySizeContribution(prop);
                    if (fixedSize > 0)
                    {
                        pendingFixed += fixedSize;
                    }
                    else
                    {
                        FlushPendingSize(writer, ref pendingFixed);
                        WriteSequentialPropertySize(writer, prop);
                    }
                }
            }
            FlushPendingSize(writer, ref pendingFixed);

            if (item.MaxEntryIndex.HasValue)
            {
                WriteOffsetTableSize(writer, item);
            }

            if (!item.ExpectedSizes.IsEmpty)
            {
                writer.WriteLine();
                var sizes = string.Join(" || ", item.ExpectedSizes.AsSpan().ToArray().Select(size => $"size == {size}"));
                writer.WriteLine($"global::System.Diagnostics.Debug.Assert({sizes}, $\"Unexpected body size {{size}}\");");
            }

            writer.WriteLine();
            writer.WriteLine("return size;");
        }
    }

    private static void WriteShockwaveItemWriteTo(IndentedTextWriter writer, ShockwaveItemInfo item)
    {
        string suffix = item.BigEndian ? "BigEndian" : "LittleEndian";

        if (item.SizePrefixed)
        {
            writer.WriteGeneratedAttributes(nameof(ShockwaveItemGenerator));
            using (writer.WriteBlock($"public void WriteTo(ref {FullyQualifiedShockwaveWriterName} output, {FullyQualifiedWriterOptionsName} options)"))
            {
                writer.WriteLine("WriteTo(ref output, options, GetBodySize(options));");
            }

            writer.WriteLine();
            writer.WriteGeneratedAttributes(nameof(ShockwaveItemGenerator));
            using (writer.WriteBlock($"internal void WriteTo(ref {FullyQualifiedShockwaveWriterName} output, {FullyQualifiedWriterOptionsName} options, int bodySize)"))
            {
                if (!item.ExpectedSizes.IsEmpty)
                {
                    var sizes = string.Join(" || ", item.ExpectedSizes.AsSpan().ToArray().Select(size => $"bodySize == {size}"));
                    writer.WriteLine($"global::System.Diagnostics.Debug.Assert({sizes}, $\"Unexpected body size {{bodySize}}\");");
                    writer.WriteLine();
                }

                foreach (PropertySerializationInfo prop in item.Properties.AsSpan())
                {
                    if (prop.Kind is PropertyKind.Sequential)
                    {
                        if (prop.IsArray || prop.IsList)
                        {
                            WriteCollectionPropertyWrite(writer, prop, suffix);
                        }
                        else if (prop.SerializationKind is TypeSerializationKind.ShockwaveItem)
                        {
                            WriteShockwaveItemPropertyWrite(writer, prop, suffix);
                        }
                        else
                        {
                            WriteSequentialPropertyWrite(writer, prop, suffix);
                        }
                    }
                }

                if (item.MaxEntryIndex.HasValue)
                {
                    WriteOffsetTableWrite(writer, item);
                }
            }
        }
        else
        {
            writer.WriteGeneratedAttributes(nameof(ShockwaveItemGenerator));
            using (writer.WriteBlock($"public void WriteTo(ref {FullyQualifiedShockwaveWriterName} output, {FullyQualifiedWriterOptionsName} options)"))
            {
                foreach (PropertySerializationInfo prop in item.Properties.AsSpan())
                {
                    if (prop.Kind is PropertyKind.Sequential)
                    {
                        if (prop.IsArray || prop.IsList)
                        {
                            WriteCollectionPropertyWrite(writer, prop, suffix);
                        }
                        else if (prop.SerializationKind is TypeSerializationKind.ShockwaveItem)
                        {
                            WriteShockwaveItemPropertyWrite(writer, prop, suffix);
                        }
                        else
                        {
                            WriteSequentialPropertyWrite(writer, prop, suffix);
                        }
                    }
                }

                if (item.MaxEntryIndex.HasValue)
                {
                    WriteOffsetTableWrite(writer, item);
                }
            }
        }
    }

    private static void WriteShockwaveItemPropertyWrite(IndentedTextWriter writer, PropertySerializationInfo prop, string suffix)
    {
        if (prop.IsSizePrefixed)
        {
            string localName = $"{char.ToLowerInvariant(prop.Name[0])}{prop.Name[1..]}BodySize";
            writer.WriteLine($"int {localName} = {prop.Name}.GetBodySize(options);");
            writer.WriteLine($"output.WriteInt32{suffix}({localName});");
            writer.WriteLine($"{prop.Name}.WriteTo(ref output, options, {localName});");
        }
        else
        {
            writer.WriteLine($"{prop.Name}.WriteTo(ref output, options);");
        }
    }

    private static void WriteCollectionPropertySize(IndentedTextWriter writer, PropertySerializationInfo prop)
    {
        if (prop.PadBefore > 0) writer.WriteLine($"size += {prop.PadBefore};");

        string lenProp = prop.IsArray ? $"{prop.Name}.Length" : $"{prop.Name}.Count";
        int elemFixedSize = GetFixedSize(prop.ArrayElementSerializationKind);

        if (elemFixedSize > 0)
        {
            writer.WriteLine($"size += {lenProp} * {elemFixedSize};");
        }
        else if (prop.ArrayElementSerializationKind is TypeSerializationKind.ShockwaveItem)
        {
            using (writer.WriteBlock($"for (int i = 0; i < {lenProp}; i++)"))
            {
                writer.WriteLine($"size += {prop.Name}[i].GetBodySize(options);");
            }
        }
        else if (prop.ArrayElementSerializationKind is TypeSerializationKind.String)
        {
            using (writer.WriteBlock($"for (int i = 0; i < {lenProp}; i++)"))
            {
                writer.WriteLine($"size += {FullyQualifiedShockwaveWriterName}.GetPStringSize({prop.Name}[i]);");
            }
        }

        if (prop.PadAfter > 0) writer.WriteLine($"size += {prop.PadAfter};");
    }

    private static void WriteCollectionPropertyWrite(IndentedTextWriter writer, PropertySerializationInfo prop, string suffix)
    {
        if (prop.PadBefore > 0) writer.WriteLine($"output.WriteZeroes({prop.PadBefore});");

        string lenProp = prop.IsArray ? $"{prop.Name}.Length" : $"{prop.Name}.Count";
        using (writer.WriteBlock($"for (int i = 0; i < {lenProp}; i++)"))
        {
            WriteElementValue(writer, prop.ArrayElementSerializationKind, prop.ArrayElementEnumUnderlyingTypeFullName, $"{prop.Name}[i]", suffix);
        }

        if (prop.PadAfter > 0) writer.WriteLine($"output.WriteZeroes({prop.PadAfter});");
    }

    private static void WriteSequentialPropertySize(IndentedTextWriter writer, PropertySerializationInfo prop)
    {
        if (prop.PadBefore > 0) writer.WriteLine($"size += {prop.PadBefore};");

        if (prop.Condition is not null && prop.IsNullable)
        {
            using (writer.WriteBlock($"if ({prop.Name} is not null)"))
            {
                writer.WriteLine($"size += {GetSizeExpression(prop, prop.Name, "options")};");
            }
        }
        else if (prop.Condition is not null)
        {
            using (writer.WriteBlock($"if ({prop.Condition})"))
            {
                writer.WriteLine($"size += {GetSizeExpression(prop, prop.Name, "options")};");
            }
        }
        else
        {
            writer.WriteLine($"size += {GetSizeExpression(prop, prop.Name, "options")};");
        }

        if (prop.PadAfter > 0) writer.WriteLine($"size += {prop.PadAfter};");
    }

    /// <summary>
    /// Returns the total fixed-size contribution (padding + value) if the property can be coalesced,
    /// or -1 if it requires separate emission (conditional, nullable, or variable-size).
    /// </summary>
    private static int TryGetFixedPropertySizeContribution(PropertySerializationInfo prop)
    {
        if (prop.Condition is not null || prop.IsNullable) return -1;

        if (prop.IsArray || prop.IsList)
        {
            if (prop.FixedCount >= 0)
            {
                int elemSize = GetFixedSize(prop.ArrayElementSerializationKind);
                if (elemSize > 0)
                {
                    return prop.PadBefore + prop.FixedCount * elemSize + prop.PadAfter;
                }
            }
            return -1;
        }

        int fixedSize = GetFixedSize(prop);
        if (fixedSize <= 0) return -1;

        return prop.PadBefore + fixedSize + prop.PadAfter;
    }

    private static void FlushPendingSize(IndentedTextWriter writer, ref int pending)
    {
        if (pending > 0)
        {
            writer.WriteLine($"size += {pending};");
            pending = 0;
        }
    }

    private static void WriteSequentialPropertyWrite(IndentedTextWriter writer, PropertySerializationInfo prop, string suffix)
    {
        if (prop.PadBefore > 0) writer.WriteLine($"output.WriteZeroes({prop.PadBefore});");

        if (prop.Condition is not null)
        {
            using (writer.WriteBlock($"if ({prop.Condition})"))
            {
                WriteValue(writer, prop, GetWriteValueExpression(prop, prop.Name), suffix);
            }
        }
        else
        {
            WriteValue(writer, prop, GetWriteValueExpression(prop, prop.Name), suffix);
        }

        if (prop.PadAfter > 0) writer.WriteLine($"output.WriteZeroes({prop.PadAfter});");
    }

    private static void WriteOffsetTableSize(IndentedTextWriter writer, ShockwaveItemInfo item)
    {
        int maxEntryIndex = item.MaxEntryIndex.GetValueOrDefault(-1);
        int entryCount = maxEntryIndex + 1;
        PropertySerializationInfo[] entries = GetEntryProperties(item);

        int tableSize = sizeof(ushort) + (entryCount + 1) * sizeof(uint);
        writer.WriteLine($"size += {tableSize};");
        foreach (PropertySerializationInfo entry in entries)
        {
            string sizeExpr = GetSizeExpression(entry, entry.Name, "options");
            if (entry.IsNullable)
            {
                writer.WriteLine($"size += {entry.Name} is not null ? {sizeExpr} : 0;");
            }
            else
            {
                writer.WriteLine($"size += {sizeExpr};");
            }
        }
    }

    private static void WriteOffsetTableWrite(IndentedTextWriter writer, ShockwaveItemInfo item)
    {
        int maxEntryIndex = item.MaxEntryIndex.GetValueOrDefault(-1);
        int entryCount = maxEntryIndex + 1;
        string suffix = item.BigEndian ? "BigEndian" : "LittleEndian";
        PropertySerializationInfo[] entries = GetEntryProperties(item);

        WriteEntrySizesCollectionExpression(writer, entryCount, entries);
        writer.WriteLine($"{FullyQualifiedOffsetTableName}.CreateAndWriteTo(entrySizes, ref output);");
        writer.WriteLine();

        foreach (PropertySerializationInfo entry in entries)
        {
            if (entry.IsNullable)
            {
                using (writer.WriteBlock($"if ({entry.Name} is not null)"))
                {
                    WriteValue(writer, entry, GetWriteValueExpression(entry, entry.Name), suffix);
                }
            }
            else
            {
                WriteValue(writer, entry, GetWriteValueExpression(entry, entry.Name), suffix);
            }
        }
    }

    private static void WriteEntrySizesCollectionExpression(IndentedTextWriter writer, int entryCount, PropertySerializationInfo[] entries)
    {
        writer.Write("global::System.Span<int> entrySizes = [");
        writer.WriteLine();
        writer.IncreaseIndent();
        for (int i = 0; i < entryCount; i++)
        {
            PropertySerializationInfo? entry = entries.FirstOrDefault(e => e.EntryIndex == i);
            string element;
            if (entry is not null)
            {
                string sizeExpr = GetSizeExpression(entry, entry.Name, "options");
                element = entry.IsNullable ? $"{entry.Name} is not null ? {sizeExpr} : 0" : sizeExpr;
            }
            else
            {
                element = "0";
            }
            writer.WriteLine($"{element},");
        }
        writer.WriteLine("0");
        writer.DecreaseIndent();
        writer.WriteLine("];");
    }

    private static void WriteValue(IndentedTextWriter writer, PropertySerializationInfo prop, string valueExpression, string suffix)
    {
        switch (prop.ParseKind)
        {
            case ParseKind.PString:
                writer.WriteLine($"output.WritePString({valueExpression});");
                return;
            case ParseKind.CString:
                writer.WriteLine($"output.WriteCString({valueExpression});");
                return;
            case ParseKind.FixedBytes:
                writer.WriteLine($"output.WriteFixedString({valueExpression});");
                return;
        }

        if (prop.SerializationKind is TypeSerializationKind.ShockwaveItem)
        {
            writer.WriteLine($"{valueExpression}.WriteTo(ref output, options);");
            return;
        }

        WriteElementValue(writer, prop.SerializationKind, prop.EnumUnderlyingTypeFullName, valueExpression, suffix);
    }

    private static void WriteElementValue(IndentedTextWriter writer, TypeSerializationKind kind, string? enumUnderlyingTypeFullName, string valueExpression, string suffix)
    {
        switch (kind)
        {
            case TypeSerializationKind.Byte:
                writer.WriteLine($"output.WriteByte({valueExpression});");
                break;
            case TypeSerializationKind.SByte:
                writer.WriteLine($"output.WriteByte(unchecked((byte){valueExpression}));");
                break;
            case TypeSerializationKind.Boolean:
                writer.WriteLine($"output.WriteBoolean({valueExpression});");
                break;
            case TypeSerializationKind.Int16:
                writer.WriteLine($"output.WriteInt16{suffix}({valueExpression});");
                break;
            case TypeSerializationKind.UInt16:
                writer.WriteLine($"output.WriteUInt16{suffix}({valueExpression});");
                break;
            case TypeSerializationKind.Int32:
                writer.WriteLine($"output.WriteInt32{suffix}({valueExpression});");
                break;
            case TypeSerializationKind.UInt32:
                writer.WriteLine($"output.WriteUInt32{suffix}({valueExpression});");
                break;
            case TypeSerializationKind.UInt64:
                writer.WriteLine($"output.WriteUInt64{suffix}({valueExpression});");
                break;
            case TypeSerializationKind.Double:
                writer.WriteLine($"output.WriteDouble{suffix}({valueExpression});");
                break;
            case TypeSerializationKind.Color:
                writer.WriteLine($"output.WriteColor({valueExpression});");
                break;
            case TypeSerializationKind.String:
                writer.WriteLine($"output.WritePString({valueExpression});");
                break;
            case TypeSerializationKind.Enum:
                writer.WriteLine($"{GetEnumWriteExpressionForUnderlying(enumUnderlyingTypeFullName, valueExpression, suffix)};");
                break;
            case TypeSerializationKind.ShockwaveItem:
                writer.WriteLine($"{valueExpression}.WriteTo(ref output, options);");
                break;
        }
    }

    private static string GetSizeExpression(PropertySerializationInfo prop, string valueExpression, string optionsExpression)
    {
        return prop.ParseKind switch
        {
            ParseKind.PString => $"{FullyQualifiedShockwaveWriterName}.GetPStringSize({valueExpression})",
            ParseKind.CString => $"{FullyQualifiedShockwaveWriterName}.GetCStringSize({valueExpression})",
            ParseKind.FixedBytes => $"{FullyQualifiedShockwaveWriterName}.GetFixedStringSize({valueExpression})",
            _ => prop.SerializationKind switch
            {
                TypeSerializationKind.String => $"{FullyQualifiedShockwaveWriterName}.GetPStringSize({valueExpression})",
                TypeSerializationKind.ShockwaveItem => $"{valueExpression}.GetBodySize({optionsExpression})",
                _ => GetFixedSize(prop).ToString(),
            },
        };
    }

    private static int GetFixedSize(PropertySerializationInfo prop)
    {
        return prop.SerializationKind switch
        {
            TypeSerializationKind.Byte or TypeSerializationKind.SByte or TypeSerializationKind.Boolean => 1,
            TypeSerializationKind.Int16 or TypeSerializationKind.UInt16 => 2,
            TypeSerializationKind.Int32 or TypeSerializationKind.UInt32 => 4,
            TypeSerializationKind.Color => 6,
            TypeSerializationKind.UInt64 or TypeSerializationKind.Double => 8,
            TypeSerializationKind.Enum => GetFixedSize(GetSerializationKindForTypeName(prop.EnumUnderlyingTypeFullName)),
            _ => 0,
        };
    }

    private static int GetFixedSize(TypeSerializationKind kind)
    {
        return kind switch
        {
            TypeSerializationKind.Byte or TypeSerializationKind.SByte or TypeSerializationKind.Boolean => 1,
            TypeSerializationKind.Int16 or TypeSerializationKind.UInt16 => 2,
            TypeSerializationKind.Int32 or TypeSerializationKind.UInt32 => 4,
            TypeSerializationKind.Color => 6,
            TypeSerializationKind.UInt64 or TypeSerializationKind.Double => 8,
            _ => 0,
        };
    }

    private static string GetEnumWriteExpression(PropertySerializationInfo prop, string valueExpression, string suffix)
    {
        return GetEnumWriteExpressionForUnderlying(prop.EnumUnderlyingTypeFullName, valueExpression, suffix);
    }

    private static string GetEnumWriteExpressionForUnderlying(string? enumUnderlyingTypeFullName, string valueExpression, string suffix)
    {
        TypeSerializationKind underlyingKind = GetSerializationKindForTypeName(enumUnderlyingTypeFullName);
        string castType = GetTypeKeyword(enumUnderlyingTypeFullName);
        string castExpression = $"({castType}){valueExpression}";

        return underlyingKind switch
        {
            TypeSerializationKind.Byte => $"output.WriteByte({castExpression})",
            TypeSerializationKind.Int16 => $"output.WriteInt16{suffix}({castExpression})",
            TypeSerializationKind.UInt16 => $"output.WriteUInt16{suffix}({castExpression})",
            TypeSerializationKind.Int32 => $"output.WriteInt32{suffix}({castExpression})",
            TypeSerializationKind.UInt32 => $"output.WriteUInt32{suffix}({castExpression})",
            TypeSerializationKind.UInt64 => $"output.WriteUInt64{suffix}({castExpression})",
            _ => $"output.WriteInt32{suffix}((int){valueExpression})",
        };
    }

    private static string GetWriteValueExpression(PropertySerializationInfo prop, string valueExpression)
    {
        if (!prop.IsNullable || prop.SerializationKind is TypeSerializationKind.String or TypeSerializationKind.ShockwaveItem)
        {
            return valueExpression;
        }

        return $"{valueExpression}.GetValueOrDefault()";
    }

    private static string GetTypeKeyword(string? typeName)
    {
        // lang=C#
        return typeName switch
        {
            "global::System.Byte" => "byte",
            "global::System.SByte" => "sbyte",
            "global::System.Boolean" => "bool",
            "global::System.Int16" => "short",
            "global::System.UInt16" => "ushort",
            "global::System.Int32" => "int",
            "global::System.UInt32" => "uint",
            "global::System.UInt64" => "ulong",
            "global::System.Double" => "double",
            _ => typeName ?? "int",
        };
    }
}