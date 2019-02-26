using System;
using System.IO;
using System.Text;
using System.Drawing;
using System.Runtime.InteropServices;

namespace Shockky.IO
{
    public class ShockwaveWriter : BinaryWriter //TODO:
    {
        private readonly bool _leaveOpen;

        public long Position
        {
            get => BaseStream.Position;
            set => BaseStream.Position = value;
        }
        public long Length => BaseStream.Length;
        
        public ShockwaveWriter()
            : this(0)
        { }
        public ShockwaveWriter(byte[] data)
            : this(data.Length)
        {
            Write(data, 0, data.Length);
        }
        public ShockwaveWriter(int capacity)
            : this(new MemoryStream(capacity))
        { }

        public ShockwaveWriter(Stream output)
            : this(output, new ASCIIEncoding(), false)
        { }
        public ShockwaveWriter(Stream output, bool leaveOpen)
            : this(output, new ASCIIEncoding(), leaveOpen)
        { }
        public ShockwaveWriter(Stream output, Encoding encoding)
            : this(output, encoding, false)
        { }
        public ShockwaveWriter(Stream output, Encoding encoding, bool leaveOpen)
            : base(output, encoding)
        {
            _leaveOpen = leaveOpen;
        }
        
        public void Write(ShockwaveItem item)
        {
            item.WriteTo(this);
        }

        public void WriteNullString(string value)
        {
            Write(value.ToCharArray());
            Write('\0');
        }

        public void WriteReversedString(string value)
        {
            char[] chars = value.ToCharArray();
            Array.Reverse(chars);
            Write(chars);
        }

        public void Write(Rectangle rect)
        {
            WriteBigEndian((short)rect.Top);
            WriteBigEndian((short)rect.Left);
            WriteBigEndian((short)rect.Bottom);
            WriteBigEndian((short)rect.Right);
        }

        public void WriteBigEndian<T>(T value)
            where T : struct
        {
            int size = Marshal.SizeOf<T>();
            Span<byte> data = stackalloc byte[size];

            MemoryMarshal.Write(data, ref value);
            data.Reverse();

            BaseStream.Write(data);
        }

        public new void Write7BitEncodedInt(int value)
        {
            base.Write7BitEncodedInt(value); //TODO: THIS IS WRONG VAR-INT WRITING METHOD
        }
    }
}
