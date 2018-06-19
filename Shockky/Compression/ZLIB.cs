using System.IO;

using Ionic.Zlib;
using Shockky.IO;

namespace Shockky.Compression
{
    public static class ZLIB
    {
        public static byte[] Compress(byte[] data)
        {
            using (var output = new MemoryStream())
            using (var compressor = new ZlibStream(output, CompressionMode.Compress, CompressionLevel.BestCompression))
            {
                ZlibBaseStream.CompressBuffer(data, compressor);
                return output.ToArray();
            }
        }
        public static byte[] Decompress(byte[] data)
        {
            using (var input = new MemoryStream(data))
            using (var decompressor = new ZlibStream(input, CompressionMode.Decompress))
            {
                return ZlibBaseStream.UncompressBuffer(decompressor);
            }
        }

        public static ShockwaveReader WrapDecompressor(Stream input)
        {
            return new ShockwaveReader(new ZlibStream(input, CompressionMode.Decompress));
        }
        public static ShockwaveWriter WrapCompressor(Stream output, bool leaveOpen = false)
        {
            return new ShockwaveWriter(new ZlibStream(output, CompressionMode.Compress, CompressionLevel.BestCompression, leaveOpen));
        }
    }
}