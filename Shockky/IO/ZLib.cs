using System;
using System.IO;
using System.IO.Compression;

namespace Shockky.IO
{
    public static class ZLib
    {
        //           Prayer circle
        //            🕯      🕯
        //    🕯                       🕯
        //           Spanified API
        // 🕯        For Deflate           🕯 
        //           [De]compression
        //    🕯                       🕯
        //            🕯      🕯
        public static unsafe int Decompress(ReadOnlySpan<byte> input, Span<byte> output)
        {
            fixed (byte* pBuffer = &input.Slice(2)[0]) //Skip ZLib header
            {
                using var stream = new UnmanagedMemoryStream(pBuffer, input.Length);
                using var deflateStream = new DeflateStream(stream, CompressionMode.Decompress);

                return deflateStream.Read(output);
            }
        }
    }
}
