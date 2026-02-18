using Shockky.IO;

using Xunit;

namespace Shockky.Tests.IO;

public class ShockwaveWriterTests
{
    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(127)]
    [InlineData(128)]
    [InlineData(8192)]
    [InlineData(16383)]
    [InlineData(16384)]
    [InlineData(2097151)]
    [InlineData(2097152)]
    [InlineData(13371337)]
    [InlineData(134217728)]
    [InlineData(268435455)]
    public void Write_VariableInteger_AreEqual(int value)
    {
        Span<byte> buffer = stackalloc byte[ShockwaveWriter.GetVarIntSize(value)];

        var output = new ShockwaveWriter(buffer, reverseEndianness: false);
        var input = new ShockwaveReader(buffer, reverseEndianness: false);

        output.Write7BitEncodedInt(value);

        Assert.Equal(value, input.Read7BitEncodedInt());
        Assert.False(input.IsDataAvailable);
    }

    [Fact]
    public void Write_BigEndian_NumericValues_AreEqual() => Write_NumericValues_AreEqual(reverseEndianness: true);

    [Fact]
    public void Write_LittleEndian_NumericValues_AreEqual() => Write_NumericValues_AreEqual(reverseEndianness: false);

    private void Write_NumericValues_AreEqual(bool reverseEndianness)
    {
        const int OUTPUT_SIZE = sizeof(byte)
            + sizeof(short) * 2
            + sizeof(ushort) * 2
            + sizeof(int) * 2
            + sizeof(uint) * 2;

        Span<byte> buffer = stackalloc byte[OUTPUT_SIZE];
        var output = new ShockwaveWriter(buffer, reverseEndianness);
        var input = new ShockwaveReader(buffer, reverseEndianness);

        output.WriteByte(42);

        output.WriteInt16LittleEndian(4242);
        output.WriteInt16BigEndian(4242);

        output.WriteUInt16LittleEndian(4242);
        output.WriteUInt16BigEndian(4242);

        output.WriteInt32LittleEndian(123456789);
        output.WriteInt32BigEndian(123456789);

        output.WriteUInt32LittleEndian(123456789);
        output.WriteUInt32BigEndian(123456789);

        Assert.Equal(42, input.ReadByte());

        Assert.Equal(4242, input.ReadInt16LittleEndian());
        Assert.Equal(4242, input.ReadInt16BigEndian());

        Assert.Equal((ushort)4242, input.ReadUInt16LittleEndian());
        Assert.Equal((ushort)4242, input.ReadUInt16BigEndian());

        Assert.Equal(123456789, input.ReadInt32LittleEndian());
        Assert.Equal(123456789, input.ReadInt32BigEndian());

        Assert.Equal((uint)123456789, input.ReadUInt32LittleEndian());
        Assert.Equal((uint)123456789, input.ReadUInt32BigEndian());
    }
}