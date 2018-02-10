using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Shockky.IO.Utils;
using Shockky.Shockwave;

namespace Shockky.IO //TODO: System.Memory
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

        public List<T> ReadBigEndianList<T>(int count, int offset = 0)
            where T : struct
        {
            if (offset > 0)
                Position = offset;

            var array = new T[count];
            for (int i = 0; i < count; i++)
            {
                array[i] = ReadBigEndian<T>();
            }

            return new List<T>(array);
        }

        public List<T> ReadList<T>(int count, int offset = 0) //TODO: Get rid of these, useless imo
            where T : IConvertible
        {
            object Read()
            {
                if (typeof(T) == typeof(int)) return ReadInt32();
                if (typeof(T) == typeof(short)) return ReadInt16();
                if (typeof(T) == typeof(string)) return ReadString();
                if (typeof(T) == typeof(uint)) return ReadUInt32();
                if (typeof(T) == typeof(ushort)) return ReadUInt16();

                throw new NotSupportedException();
            }

            if (offset > 0)
                Position = offset;

            var array = new T[count];
            for (int i = 0; i < count; i++)
            {
                array[i] = (T) Read();
            }

            return new List<T>(array);
        }

    }

	public static class ShockwaveReaderExtensions
	{
		public static ShockwaveReader Cut(this ShockwaveReader reader, int length) //TODO: Instead some alignment tricky, this is way too heavy
		    => new ShockwaveReader(reader.ReadBytes(length));
	}
}