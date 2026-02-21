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
        item.Hierarchy.WriteSyntax(item, writer, [], [static (info, w) => WriteShockwaveItemReadMethodBody(info, w)]);
    }

    private static void WriteShockwaveItemReadMethodBody(ShockwaveItemInfo item, IndentedTextWriter writer)
    {
        string typeName = item.Hierarchy.Hierarchy[0].QualifiedName;
        string suffix = item.BigEndian ? "BigEndian" : "LittleEndian";
        bool hasOffsetTableProperty = item.OffsetTablePropertyName is not null;

        writer.WriteGeneratedAttributes(nameof(ShockwaveItemGenerator));
        using (writer.WriteBlock($"public {typeName}(ref {FullyQualifiedShockwaveReaderName} input, {FullyQualifiedReaderContextName} context)"))
        {
            // TODO: Revisit handling, hardcoded for FileInfo
            writer.WriteLine($"input.ReverseEndianness = false;");
            writer.WriteLine();

            foreach (var prop in item.Properties.AsSpan())
            {
                switch (prop.Kind)
                {
                    case PropertyKind.Sequential:
                        WriteSequentialPropertyRead(writer, prop, suffix);
                        break;
                    case PropertyKind.Header:
                        WriteHeaderPropertyRead(writer, prop, suffix);
                        break;
                    case PropertyKind.OffsetTable:
                        WriteOffsetTableRead(writer, prop);
                        break;
                    case PropertyKind.Entry:
                        WriteOffsetTableEntryPropertyRead(writer, prop, suffix, item.OffsetTablePropertyName!);
                        break;
                }
            }

            // Skip remaining entries if we have offset table
            if (item.MaxEntryIndex.HasValue)
            {
                writer.WriteLine();
                writer.WriteLine($"int skipStart = {item.MaxEntryIndex.Value + 1};");
                using (writer.WriteBlock($"if ({item.OffsetTablePropertyName}.Length > skipStart)"))
                {
                    writer.WriteLine($"uint remainingSize = {item.OffsetTablePropertyName}.GetEntryRangeSize(skipStart);");
                    writer.WriteLine("input.Advance((int)remainingSize);");
                }
            }
        }

        writer.WriteLine();
        writer.WriteLine($"public static {typeName} Read(ref {FullyQualifiedShockwaveReaderName} input, {FullyQualifiedReaderContextName} context) => new(ref input, context);");
    }

    private static void WriteHeaderReadMethod(IndentedTextWriter writer, HeaderInfo header)
    {
        header.Hierarchy.WriteSyntax(header, writer, [], [static (info, w) => WriteHeaderReadMethodBody(info, w)]);
    }

    private static void WriteHeaderReadMethodBody(HeaderInfo header, IndentedTextWriter writer)
    {
        string typeName = header.Hierarchy.Hierarchy[0].QualifiedName;
        string suffix = header.BigEndian ? "BigEndian" : "LittleEndian";

        writer.WriteGeneratedAttributes(nameof(ShockwaveItemGenerator));
        using (writer.WriteBlock($"public {typeName}(ref {FullyQualifiedShockwaveReaderName} input, {FullyQualifiedReaderContextName} context, int headerSize)"))
        {
            if (!header.ExpectedSizes.IsEmpty)
            {
                var sizes = string.Join(" || ", header.ExpectedSizes.AsSpan().ToArray().Select(size => $"headerSize == {size}"));
                writer.WriteLine($"global::System.Diagnostics.Debug.Assert({sizes}, $\"Unexpected header size {{headerSize}}\");");
                writer.WriteLine();
            }

            foreach (var prop in header.Properties.AsSpan())
            {
                WriteSequentialPropertyRead(writer, prop, suffix);
            }
        }

        writer.WriteLine();
        writer.WriteLine($"public static {typeName} Read(ref {FullyQualifiedShockwaveReaderName} input, {FullyQualifiedReaderContextName} context, int headerSize) => new(ref input, context, headerSize);");
    }

    private static void WriteSequentialPropertyRead(IndentedTextWriter writer, PropertyReadInfo prop, string suffix)
    {
        if (prop.PadBefore > 0)
            writer.WriteLine($"input.Advance({prop.PadBefore});");

        if (prop.Condition is not null)
        {
            writer.WriteLine($"if ({prop.Condition})");
            writer.IncreaseIndent();
        }

        string readExpr = GetReadExpression(prop.TypeFullName, prop.ParseKind, suffix, null);
        writer.WriteLine($"{prop.Name} = {readExpr};");

        if (prop.Condition is not null)
            writer.DecreaseIndent();

        if (prop.PadAfter > 0)
            writer.WriteLine($"input.Advance({prop.PadAfter});");
    }

    private static void WriteHeaderPropertyRead(IndentedTextWriter writer, PropertyReadInfo prop, string suffix)
    {
        string headerType = prop.TypeFullName.TrimEnd('?');
        writer.WriteLine($"int headerSize = input.ReadInt32{suffix}();");
        writer.WriteLine($"{prop.Name} = {headerType}.Read(ref input, context, headerSize);");
        writer.WriteLine();
    }

    private static void WriteOffsetTableRead(IndentedTextWriter writer, PropertyReadInfo prop)
    {
        writer.WriteLine($"{prop.Name} = {FullyQualifiedOffsetTableName}.Read(ref input);");
        writer.WriteLine();
    }

    private static void WriteOffsetTableEntryPropertyRead(IndentedTextWriter writer, PropertyReadInfo prop, string suffix, string offsetsName)
    {
        writer.WriteLine($"uint entry{prop.EntryIndex}Size = {offsetsName}.GetEntrySize({prop.EntryIndex});");
        using (writer.WriteBlock($"if (entry{prop.EntryIndex}Size > 0)"))
        {
            if (prop.ParseKind == ParseKind.PString)
            {
                writer.WriteLine($"{prop.Name} = input.ReadPString();");
                writer.WriteLine($"int consumed = {prop.Name}?.Length + 1 ?? 1;");
                writer.WriteLine($"input.Advance((int)entry{prop.EntryIndex}Size - consumed);");
            }
            else if (prop.ParseKind == ParseKind.FixedBytes)
            {
                writer.WriteLine($"{prop.Name} = input.ReadString((int)entry{prop.EntryIndex}Size);");
            }
            else
            {
                string readExpr = GetReadExpression(prop.TypeFullName, prop.ParseKind, suffix, $"entry{prop.EntryIndex}Size");
                writer.WriteLine($"{prop.Name} = {readExpr};");

                int typeSize = GetTypeSize(prop.TypeFullName);
                if (typeSize > 0)
                {
                    writer.WriteLine($"input.Advance((int)entry{prop.EntryIndex}Size - {typeSize});");
                }
            }
        }
        writer.WriteLine();
    }

    private static string GetReadExpression(string typeName, ParseKind parseKind, string suffix, string? entrySize)
    {
        if (parseKind == ParseKind.PString) return "input.ReadPString()";
        if (parseKind == ParseKind.CString) return "input.ReadCString()";
        if (parseKind == ParseKind.FixedBytes && entrySize is not null)
            return $"input.ReadString((int){entrySize})";

        string baseType = typeName.TrimEnd('?');

        // lang=C#
        return baseType switch
        {
            "byte" => "input.ReadByte()",
            "sbyte" => "input.ReadSByte()",
            "short" or "global::System.Int16" => $"input.ReadInt16{suffix}()",
            "ushort" or "global::System.UInt16" => $"input.ReadUInt16{suffix}()",
            "int" or "global::System.Int32" => $"input.ReadInt32{suffix}()",
            "uint" or "global::System.UInt32" => $"input.ReadUInt32{suffix}()",
            "long" or "global::System.Int64" => $"input.ReadInt64{suffix}()",
            "ulong" or "global::System.UInt64" => $"input.ReadUInt64{suffix}()",
            "float" or "global::System.Single" => $"input.ReadSingle{suffix}()",
            "double" or "global::System.Double" => $"input.ReadDouble{suffix}()",
            "string" => "input.ReadPString()",
            _ => $"({baseType})input.ReadInt32{suffix}()" // Assume enum
        };
    }

    private static int GetTypeSize(string typeName)
    {
        string baseType = typeName.TrimEnd('?');

        // lang=C#
        return baseType switch
        {
            "byte" or "sbyte" => 1,
            "short" or "ushort" or "global::System.Int16" or "global::System.UInt16" => 2,
            "int" or "uint" or "global::System.Int32" or "global::System.UInt32" or "float" or "global::System.Single" => 4,
            "long" or "ulong" or "global::System.Int64" or "global::System.UInt64" or "double" or "global::System.Double" => 8,
            _ => 0
        };
    }
}