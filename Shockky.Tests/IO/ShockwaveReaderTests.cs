using Shockky.IO;

using Xunit;

namespace Shockky.Tests.IO;

public class ShockwaveReaderTests
{
    //TODO: Cover rest of the reader implementation

    [Fact]
    public void Read_ValidVarInts()
    {
        Span<byte> encodedValueBuffer = [
            0x00,                     // 0
            0x01,                     // 1 
            0x7F,                     // 127
            0x81, 0x00,               // 128
            0xC0, 0x00,               // 8192
            0xFF, 0x7F,               // 16383
            0x81, 0x80, 0x00,         // 16384
            0xFF, 0xFF, 0x7F,         // 2097151
            0x81, 0x80, 0x80, 0x00,   // 2097152
            0xC0, 0x80, 0x80, 0x00,   // 134217728
            0xFF, 0xFF, 0xFF, 0x7F,   // 268435455
            0xC0, 0x00,               // 8192
            0x86, 0xB0, 0x8F, 0x49    // 13371337
        ];

        ShockwaveReader input = new(encodedValueBuffer);

        Assert.Equal(0, input.Read7BitEncodedInt());
        Assert.Equal(1, input.Read7BitEncodedInt());
        Assert.Equal(127, input.Read7BitEncodedInt());
        Assert.Equal(128, input.Read7BitEncodedInt());
        Assert.Equal(8192, input.Read7BitEncodedInt());
        Assert.Equal(16383, input.Read7BitEncodedInt());
        Assert.Equal(16384, input.Read7BitEncodedInt());
        Assert.Equal(2097151, input.Read7BitEncodedInt());
        Assert.Equal(2097152, input.Read7BitEncodedInt());
        Assert.Equal(134217728, input.Read7BitEncodedInt());
        Assert.Equal(268435455, input.Read7BitEncodedInt());
        Assert.Equal(8192, input.Read7BitEncodedInt());
        Assert.Equal(13371337, input.Read7BitEncodedInt());
    }
}