using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO.Compression;
using System.Runtime.InteropServices;

namespace Shockky.IO
{
    public class ShockwaveReader : BinaryReader
    {
        public long Position
        {
            get => BaseStream.Position;
            set => BaseStream.Position = value;
        }
        public long Length => BaseStream.Length;
        public bool IsDataAvailable => (Position < Length);

        public ShockwaveReader(byte[] data)
            : this(new MemoryStream(data))
        { }
        public ShockwaveReader(Stream input)
            : base(input)
        { }
        public ShockwaveReader(Stream input, bool leaveOpen)
            : this(input, Encoding.ASCII, leaveOpen)
        { }
        public ShockwaveReader(Stream input, Encoding encoding)
            : base(input, encoding)
        { }
        public ShockwaveReader(Stream input, Encoding encoding, bool leaveOpen)
            : base(input, encoding, leaveOpen)
        { }

        public T ReadBigEndian<T>()
            where T : struct
        {
            int size = Marshal.SizeOf<T>();
            Span<byte> buffer = stackalloc byte[size];

            int read = BaseStream.Read(buffer);
            buffer.Reverse();

            return MemoryMarshal.Read<T>(buffer);
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
            while ((lastByte & 128) >> 7 == 1);
            return (result >> 7);
        }

        public string ReadString(int length)
            => new string(ReadChars(length));

        public string ReadReversedString(int length)
        {
            char[] characters = ReadChars(length);
            Array.Reverse(characters);

            return new string(characters);
        }
        public Rectangle ReadRect(bool bigEndian)
        {
            short x1 = ReadBigEndian<short>();
            short x2 = ReadBigEndian<short>();
            short y1 = ReadBigEndian<short>();
            short y2 = ReadBigEndian<short>();
            return new Rectangle(x1, y1, x2/* - x1*/, y2/* - y1*/); //TODO
        }

        public ShockwaveReader WrapDecompressor(int decompressedLength)
        {
            BaseStream.Seek(2, SeekOrigin.Current);

            var data = ReadBytes(decompressedLength);
            return new ShockwaveReader(new DeflateStream(new MemoryStream(data), CompressionMode.Decompress));
        }
    }
}