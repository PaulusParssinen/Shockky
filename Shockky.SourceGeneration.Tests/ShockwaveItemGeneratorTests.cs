using static Shockky.SourceGeneration.Tests.GeneratorTestHelper;

namespace Shockky.SourceGeneration.Tests;

public sealed class ShockwaveItemGeneratorTests
{
    [Fact]
    public Task SequentialProperties()
    {
        // lang=C#
        string source = """
            using Shockky;

            [ShockwaveItem(BigEndian = true)]
            public partial class BigEndianItem
            {
                public int Id { get; set; }
                public short Value { get; set; }
                public byte Flags { get; set; }
            }

            [ShockwaveItem(BigEndian = false)]
            public partial class LittleEndianItem
            {
                public int Id { get; set; }
                public ushort Value { get; set; }
            }
            """;

        return Verify(CreateDriver<ShockwaveItemGenerator>(source));
    }

    [Fact]
    public Task WithNestedSizePrefixedItem()
    {
        // lang=C#
        string source = """
            #nullable enable
            using Shockky;

            [ShockwaveItem(BigEndian = true)]
            public partial class ItemWithHeader
            {
                public HeaderData? Header { get; set; }
                public int Data { get; set; }

                [ShockwaveItem(SizePrefixed = true, GenerateSerialization = true, ExpectedSizes = [12, 16])]
                public partial class HeaderData
                {
                    public int Version { get; set; }
                    public int Length { get; set; }
                }
            }

            [ShockwaveItem(GenerateSerialization = true)]
            public partial class HeaderConditionItem
            {
                public required ConditionHeaderData Header { get; set; }

                [ShockwaveItem(SizePrefixed = true, GenerateSerialization = true, ExpectedSizes = [16, 20])]
                public partial class ConditionHeaderData
                {
                    [PadBefore(4)]
                    public int Flags { get; set; }

                    public uint Options { get; set; }

                    [Condition("bodySize >= 20")]
                    public int? ScriptContextNum { get; set; }
                }
            }
            """;

        return Verify(CreateDriver<ShockwaveItemGenerator>(source));
    }

    [Fact]
    public Task WithOffsetTableAndEntries()
    {
        // lang=C#
        string source = """
            using Shockky;

            [ShockwaveItem(BigEndian = true)]
            public partial class ItemWithEntries
            {
                [Entry(0)]
                public int FirstEntry { get; set; }

                [Entry(1)]
                public short SecondEntry { get; set; }
            }
            """;

        return Verify(CreateDriver<ShockwaveItemGenerator>(source));
    }

    [Fact]
    public Task SerializationSequentialProperties()
    {
        // lang=C#
        string source = """
            using Shockky;

            [ShockwaveItem(BigEndian = true, GenerateSerialization = true)]
            public partial class BigEndianSerializableItem
            {
                public int Id { get; set; }
                public ushort Count { get; set; }
                public string Name { get; set; }
            }

            [ShockwaveItem(BigEndian = false, GenerateSerialization = true)]
            public partial class LittleEndianSerializableItem
            {
                public int Id { get; set; }
                public ushort Count { get; set; }
            }
            """;

        return Verify(CreateDriver<ShockwaveItemGenerator>(source));
    }

    [Fact]
    public Task IgnoreContainerEndianness()
    {
        // lang=C#
        string source = """
            using Shockky;

            [ShockwaveItem(IgnoreContainerEndianness = true)]
            public partial class LocalEndianItem
            {
                public int Id { get; set; }
            }
            """;

        return Verify(CreateDriver<ShockwaveItemGenerator>(source));
    }

    [Fact]
    public Task ParseStringAsModesAndOffsetTableSerialization()
    {
        // lang=C#
        string source = """
            #nullable enable
            using Shockky;

            [ShockwaveItem(GenerateSerialization = true)]
            public partial class StringEntriesItem
            {
                [Entry(0), ParseStringAs(StringParseKind.PString)]
                public string? Name { get; set; }

                [Entry(1), ParseStringAs(StringParseKind.CString)]
                public string? Path { get; set; }

                [Entry(2), ParseStringAs(StringParseKind.FixedBytes)]
                public string? Script { get; set; }

                [Entry(5)]
                public int? Value { get; set; }
            }
            """;

        return Verify(CreateDriver<ShockwaveItemGenerator>(source));
    }

    [Fact]
    public Task EnumUnderlyingTypeSerialization()
    {
        // lang=C#
        string source = """
            using Shockky;

            public enum SmallEnum : ushort
            {
                None = 0,
                Value = 42,
            }

            [ShockwaveItem(GenerateSerialization = true)]
            public partial class EnumItem
            {
                public SmallEnum Kind { get; set; }
            }
            """;

        return Verify(CreateDriver<ShockwaveItemGenerator>(source));
    }

    [Fact]
    public Task SourceSerializationOverrides()
    {
        // lang=C#
        string source = """
            using Shockky;
            using Shockky.IO;

            [ShockwaveItem(GenerateSerialization = true)]
            public partial class SizeOverrideItem
            {
                public int Id { get; set; }

                public int GetBodySize(WriterOptions options) => 123;
            }

            [ShockwaveItem(GenerateSerialization = true)]
            public partial class WriteOverrideItem
            {
                public int Id { get; set; }

                public void WriteTo(ref ShockwaveWriter output, WriterOptions options)
                { }
            }
            """;

        return Verify(CreateDriver<ShockwaveItemGenerator>(source));
    }

    [Fact]
    public Task UnsupportedPropertyTypeDiagnostics()
    {
        // lang=C#
        string source = """
            using Shockky;

            [ShockwaveItem]
            public partial class UnsupportedItem
            {
                public object Value { get; set; } = new();
            }
            """;

        return Verify(CreateDriver<ShockwaveItemGenerator>(source));
    }

    [Fact]
    public Task ArrayAndCollectionProperties()
    {
        // lang=C#
        string source = """
            using System.Collections.Generic;
            using Shockky;

            [ShockwaveItem(BigEndian = true, GenerateSerialization = true)]
            public partial class DynamicArrayItem
            {
                public int Count { get; set; }

                [Count(nameof(Count))]
                public int[] Values { get; set; }

                [Count(nameof(Count))]
                public List<short> ShortList { get; set; }
            }

            [ShockwaveItem(BigEndian = true, GenerateSerialization = true)]
            public partial class FixedArrayItem
            {
                [Count(4)]
                public int[] Coordinates { get; set; }
            }

            [ShockwaveItem(BigEndian = true, GenerateSerialization = true)]
            public partial class NestedItemArray
            {
                public int ElementCount { get; set; }

                [Count(nameof(ElementCount))]
                public ElementData[] Elements { get; set; }

                [ShockwaveItem(GenerateSerialization = true)]
                public partial class ElementData
                {
                    public short Id { get; set; }
                    public int Value { get; set; }
                }
            }
            """;

        return Verify(CreateDriver<ShockwaveItemGenerator>(source));
    }

    [Fact]
    public Task MissingCountAttributeDiagnostics()
    {
        // lang=C#
        string source = """
            using Shockky;

            [ShockwaveItem]
            public partial class MissingCountItem
            {
                public int[] Values { get; set; }
            }
            """;

        return Verify(CreateDriver<ShockwaveItemGenerator>(source));
    }

    [Fact]
    public Task PositionalRecordSerialization()
    {
        // lang=C#
        string source = """
            using Shockky;

            [ShockwaveItem(BigEndian = true, GenerateSerialization = true)]
            public readonly partial record struct PositionalRecordItem(short A, short B);
            """;

        return Verify(CreateDriver<ShockwaveItemGenerator>(source));
    }
}