using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using Shockky.Shockwave;

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

        protected int BitPosition { get; set; }
        protected int BitContainer { get; set; }

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

        public void WriteBigEndian<T>(T value) //yoloing dis, prob gonna just go bitconverter and do overloads on every type, rip | TODO
        {
            int size = Marshal.SizeOf(value);
            byte[] bytes = new byte[size];

            IntPtr valPtr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(value, valPtr, true);
            Marshal.Copy(valPtr, bytes, 0, size);

            Array.Reverse(bytes);
            Write(bytes);
        }
    }
}
