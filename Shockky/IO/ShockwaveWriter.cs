using System;
using System.Text;
using System.Drawing;
using System.Buffers.Binary;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

namespace Shockky.IO
{
    //TODO: Out-of-bounds checks and exceptions/TryWrite?
    public ref struct ShockwaveWriter
    {
        private int _position;
        private readonly bool _bigEndian;

        private Span<byte> _data;

        public ShockwaveWriter(Span<byte> data, bool bigEndian)
        {
            _data = data;
            _position = 0;
            _bigEndian = bigEndian;
        }

        //TODO: Measure, with and without inlining
        //Advance? - Zero fill variant?
        //AdvanceTo - ohno

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write(byte value) => _data[_position++] = value;
        public void Write(ReadOnlySpan<byte> value)
        {
            value.CopyTo(_data.Slice(_position));
            _position += value.Length;
        }

        //TODO: Branchless Unsafe.As<bool, byte>(ref val) & MemoryMarshal: CreateReadOnlyspan ref val => AsBytes => ..[0] 
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write(bool value) 
        {
            _data[_position++] = value ? (byte)1 : (byte)0;
        }

        public void Write(short value)
        {
            if (_bigEndian)
            {
                value = BinaryPrimitives.ReverseEndianness(value);
            }
        
            MemoryMarshal.Write(_data.Slice(_position), ref value);
            _position += sizeof(short);
        }
        public void WriteBE(short value)
        {
            if (!_bigEndian)
            {
                value = BinaryPrimitives.ReverseEndianness(value);
            }

            MemoryMarshal.Write(_data.Slice(_position), ref value);
            _position += sizeof(short);
        }

        public void Write(ushort value)
        {
            if (_bigEndian)
            {
                value = BinaryPrimitives.ReverseEndianness(value);
            }

            MemoryMarshal.Write(_data.Slice(_position), ref value);
            _position += sizeof(ushort);
        }
        public void WriteBE(ushort value)
        {
            if (!_bigEndian)
            {
                value = BinaryPrimitives.ReverseEndianness(value);
            }

            MemoryMarshal.Write(_data.Slice(_position), ref value);
            _position += sizeof(ushort);
        }

        public void Write(int value)
        {
            if (_bigEndian)
            {
                value = BinaryPrimitives.ReverseEndianness(value);
            }

            MemoryMarshal.Write(_data.Slice(_position), ref value);
            _position += sizeof(int);
        }
        public void WriteBE(int value)
        {
            if (!_bigEndian)
            {
                value = BinaryPrimitives.ReverseEndianness(value);
            }

            MemoryMarshal.Write(_data.Slice(_position), ref value);
            _position += sizeof(int);
        }

        public void Write(uint value)
        {
            if (_bigEndian)
            {
                value = BinaryPrimitives.ReverseEndianness(value);
            }

            MemoryMarshal.Write(_data.Slice(_position), ref value);
            _position += sizeof(uint);
        }
        public void WriteBE(uint value)
        {
            if (!_bigEndian)
            {
                value = BinaryPrimitives.ReverseEndianness(value);
            }

            MemoryMarshal.Write(_data.Slice(_position), ref value);
            _position += sizeof(uint);
        }

        //TODO: Verify the impl accuracy
        public void Write7BitEncodedInt(int value)
        {
            uint v = (uint)value;
            while (v >= 0x80)
            {
                Write((byte)(v | 0x80));
                v >>= 7;
            }
            Write((byte)v);
        }

        //TODO: Measure Encoding performances
        //TODO: Increment inside accessor vs. later: _position += len + 1
        public void Write(ReadOnlySpan<char> value)
        {
            //Write((byte)value.Length)
            _data[_position++] = (byte)value.Length;

            int len = Encoding.UTF8.GetBytes(value, _data.Slice(_position));
            _position += len;
        }
        public void WriteNullString(ReadOnlySpan<char> value)
        {
            int len = Encoding.UTF8.GetBytes(value, _data.Slice(_position));
            _data[_position + len] = (byte)0;

            _position += len + 1;
        }

        public void Write(Color value)
        {
            //TODO: Endianness
            //TODO: ENSURE INLINED + compare to single offset increment by 6
            Write(value.R);
            Write(value.R);
            Write(value.G);
            Write(value.G);
            Write(value.B);
            Write(value.B);
        }
        public void Write(Rectangle value)
        {
            Write((short)value.Top);
            Write((short)value.Left);
            Write((short)value.Bottom);
            Write((short)value.Right);
        }
    }
}
