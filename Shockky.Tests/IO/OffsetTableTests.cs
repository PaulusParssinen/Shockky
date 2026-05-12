using Shockky.IO;

using Xunit;

namespace Shockky.Tests.IO;

public sealed class OffsetTableTests
{
    [Fact]
    public void GetEntrySize_ReturnsCorrectSizes()
    {
        var table = new OffsetTable([0, 10, 10, 25]);

        Assert.Equal(3, table.Length);
        Assert.Equal(10, table.GetEntrySize(0));
        Assert.Equal(0, table.GetEntrySize(1));
        Assert.Equal(15, table.GetEntrySize(2));
        Assert.Equal(0, table.GetEntrySize(3)); // out of range
    }

    [Fact]
    public void HasEntry_ReturnsTrueForNonZeroEntries()
    {
        var table = new OffsetTable([0, 5, 5, 12]);

        Assert.True(table.HasEntry(0));
        Assert.False(table.HasEntry(1));
        Assert.True(table.HasEntry(2));
        Assert.False(table.HasEntry(99));
    }

    [Fact]
    public void Default_HasZeroLength()
    {
        OffsetTable table = default;

        Assert.Equal(0, table.Length);
        Assert.Equal(0, table.GetEntrySize(0));
        Assert.Equal(0, table.PayloadSize);
    }

    [Fact]
    public void Create_PreservesTrailingZeros()
    {
        var table = OffsetTable.Create([10, 0, 5, 0, 0]);

        Assert.Equal(5, table.Length);
        Assert.Equal(10, table.GetEntrySize(0));
        Assert.Equal(0, table.GetEntrySize(1));
        Assert.Equal(5, table.GetEntrySize(2));
        Assert.Equal(0, table.GetEntrySize(3));
        Assert.Equal(0, table.GetEntrySize(4));
        Assert.Equal(15, table.PayloadSize);
    }

    [Fact]
    public void Create_AllZeroSizes_PreservesEntries()
    {
        var table = OffsetTable.Create([0, 0, 0]);

        Assert.Equal(3, table.Length);
        Assert.Equal(0, table.PayloadSize);
    }

    [Fact]
    public void Create_NoZeros_PreservesAll()
    {
        var table = OffsetTable.Create([4, 8, 2]);

        Assert.Equal(3, table.Length);
        Assert.Equal(4, table.GetEntrySize(0));
        Assert.Equal(8, table.GetEntrySize(1));
        Assert.Equal(2, table.GetEntrySize(2));
        Assert.Equal(14, table.PayloadSize);
    }

    [Fact]
    public void Create_SparseWithGaps_PreservesIndices()
    {
        var table = OffsetTable.Create([10, 0, 0, 0, 5]);

        // All entries preserved (last is non-zero so nothing trimmed)
        Assert.Equal(5, table.Length);
        Assert.Equal(10, table.GetEntrySize(0));
        Assert.Equal(0, table.GetEntrySize(1));
        Assert.Equal(0, table.GetEntrySize(2));
        Assert.Equal(0, table.GetEntrySize(3));
        Assert.Equal(5, table.GetEntrySize(4));
        Assert.Equal(15, table.PayloadSize);
    }

    [Fact]
    public void Create_MixedSparse_PreservesAll()
    {
        // Simulates CastMemberMetadata-like: entries at 0,1,10,20 only
        int[] sizes = new int[21];
        sizes[0] = 6;
        sizes[1] = 3;
        sizes[10] = 4;
        sizes[20] = 8;
        var table = OffsetTable.Create(sizes);

        Assert.Equal(21, table.Length);
        Assert.Equal(6, table.GetEntrySize(0));
        Assert.Equal(3, table.GetEntrySize(1));
        Assert.Equal(0, table.GetEntrySize(2));
        Assert.Equal(4, table.GetEntrySize(10));
        Assert.Equal(8, table.GetEntrySize(20));
        Assert.Equal(21, table.PayloadSize);
    }

    [Fact]
    public void GetTableSize_IncludesCountAndOffsets()
    {
        var table = OffsetTable.Create([4, 8]);

        // sizeof(ushort) + 3 * sizeof(int) = 2 + 12 = 14
        Assert.Equal(14, table.GetTableSize());
    }

    [Fact]
    public void WriteTo_ProducesReadableTable()
    {
        var original = OffsetTable.Create([10, 0, 5]);

        byte[] buffer = new byte[original.GetTableSize()];
        var writer = new ShockwaveWriter(buffer, reverseEndianness: false);
        original.WriteTo(ref writer);

        Assert.Equal(buffer.Length, writer.Position);

        var reader = new ShockwaveReader(buffer, reverseEndianness: false);
        var roundTripped = OffsetTable.Read(ref reader);

        Assert.Equal(original.Length, roundTripped.Length);
        Assert.Equal(original.PayloadSize, roundTripped.PayloadSize);
        for (int i = 0; i < original.Length; i++)
        {
            Assert.Equal(original.GetEntrySize(i), roundTripped.GetEntrySize(i));
        }
    }
}