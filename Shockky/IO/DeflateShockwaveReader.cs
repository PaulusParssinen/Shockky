using System.IO;
using System.IO.Compression;
using System.Buffers.Binary;

namespace Shockky.IO
{
    //TODO: I hate this.
    public class DeflateShockwaveReader : BinaryReader
    {
        public bool IsBigEndian { get; }

        public DeflateShockwaveReader(byte[] data, bool isBigEndian)
            : base(new DeflateStream(new MemoryStream(data), CompressionMode.Decompress))
        {
            IsBigEndian = isBigEndian;
        }

        public new int Read7BitEncodedInt()
        {
            int result = 0;
            byte lastByte;
            do
            {
                lastByte = ReadByte();
                result |= lastByte & 0x7F;
                result <<= 7;
            }
            while ((lastByte & 0x80) >> 7 == 1);
            return result >> 7;
        }

        public string ReadNullString()
        {
            char currentChar;
            string value = string.Empty;
            while ((currentChar = ReadChar()) != '\0')
            {
                value += currentChar;
            }
            return value;
        }

        public new short ReadInt16()
        {
            return IsBigEndian ? BinaryPrimitives.ReverseEndianness(base.ReadInt16()) : base.ReadInt16();
        }
        public short ReadBEInt16()
        {
            return IsBigEndian ?  base.ReadInt16() : BinaryPrimitives.ReverseEndianness(base.ReadInt16());
        }

        public new int ReadInt32()
        {
            return IsBigEndian ? BinaryPrimitives.ReverseEndianness(base.ReadInt32()) : base.ReadInt32();
        }
        public int ReadBEInt32()
        {
            return IsBigEndian ? base.ReadInt32() : BinaryPrimitives.ReverseEndianness(base.ReadInt32());
        }
    }
}
