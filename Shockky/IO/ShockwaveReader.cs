using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

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
        {
            byte[] data = ReadBytes(Marshal.SizeOf<T>());

            if (data.All(b => b == 0)) return default(T);
            Array.Reverse(data);

            IntPtr valuePtr = Marshal.AllocHGlobal(data.Length);
            Marshal.Copy(data, 0, valuePtr, data.Length);

            var value = (T)Marshal.PtrToStructure(valuePtr, typeof(T));
            Marshal.FreeHGlobal(valuePtr);

            return value;
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
    }
}