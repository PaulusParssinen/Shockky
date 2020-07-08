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

        public int ReadVarInt()
        {
            int value = 0;
            byte b;
            do
            {
                b = ReadByte();
                value = (value << 7) + (b & 0x7F);
            }
            while (b >> 7 != 0);
            return value;
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
