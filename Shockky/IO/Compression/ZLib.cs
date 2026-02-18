using System.IO.Compression;

namespace Shockky.IO;

public static class ZLib
{
    public static readonly Guid MoaId = new(0xAC99E904, 0x0070, 0x0B36, 0x00, 0x00, 0x08, 0x00, 0x07, 0x37, 0x7A, 0x34);

    //           🙏 Summoning circle 🙏 
    //
    //             🕯       🕯       🕯
    //     🕯                               🕯
    // https://github.com/dotnet/runtime/issues/39327
    // 🕯                                       🕯 
    //
    //     🕯                               🕯
    //             🕯       🕯       🕯
    internal static unsafe void DecompressUnsafe(ReadOnlySpan<byte> input, Span<byte> output)
    {
        fixed (byte* inputPtr = input)
        {
            using var stream = new UnmanagedMemoryStream(inputPtr, input.Length);
            using var zlibStream = new ZLibStream(stream, CompressionMode.Decompress);

            zlibStream.ReadExactly(output);
        }
    }

    // TODO: Isn't this GC hole lmao
    internal static unsafe ZLibShockwaveReader CreateDeflateReaderUnsafe(ref ShockwaveReader input)
    {
        int dataRemaining = input.Length - input.Position;
        fixed (byte* bufferPtr = input.ReadBytes(dataRemaining))
        {
            var stream = new UnmanagedMemoryStream(bufferPtr, input.Length);
            return new ZLibShockwaveReader(stream, input.ReverseEndianness, leaveOpen: false);
        }
    }
}
