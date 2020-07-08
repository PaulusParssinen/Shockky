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
        private readonly bool _isBigEndian;

        private readonly Span<byte> _data;

        public ShockwaveWriter(Span<byte> data, bool isBigEndian)
        {
            _data = data;
            _position = 0;
            _isBigEndian = isBigEndian;
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

        //TODO: Verify behaviour on different archs
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write(bool value) 
        {
            _data[_position++] = Unsafe.As<bool, byte>(ref value);
        }

        public void Write(short value)
        {
            if (_isBigEndian)
            {
                value = BinaryPrimitives.ReverseEndianness(value);
            }
        
            MemoryMarshal.Write(_data.Slice(_position), ref value);
            _position += sizeof(short);
        }
        public void WriteBE(short value)
        {
            if (!_isBigEndian)
            {
                value = BinaryPrimitives.ReverseEndianness(value);
            }

            MemoryMarshal.Write(_data.Slice(_position), ref value);
            _position += sizeof(short);
        }

        public void Write(ushort value)
        {
            if (_isBigEndian)
            {
                value = BinaryPrimitives.ReverseEndianness(value);
            }

            MemoryMarshal.Write(_data.Slice(_position), ref value);
            _position += sizeof(ushort);
        }
        public void WriteBE(ushort value)
        {
            if (!_isBigEndian)
            {
                value = BinaryPrimitives.ReverseEndianness(value);
            }

            MemoryMarshal.Write(_data.Slice(_position), ref value);
            _position += sizeof(ushort);
        }

        public void Write(int value)
        {
            if (_isBigEndian)
            {
                value = BinaryPrimitives.ReverseEndianness(value);
            }

            MemoryMarshal.Write(_data.Slice(_position), ref value);
            _position += sizeof(int);
        }
        public void WriteBE(int value)
        {
            if (!_isBigEndian)
            {
                value = BinaryPrimitives.ReverseEndianness(value);
            }

            MemoryMarshal.Write(_data.Slice(_position), ref value);
            _position += sizeof(int);
        }

        public void Write(uint value)
        {
            if (_isBigEndian)
            {
                value = BinaryPrimitives.ReverseEndianness(value);
            }

            MemoryMarshal.Write(_data.Slice(_position), ref value);
            _position += sizeof(uint);
        }
        public void WriteBE(uint value)
        {
            if (!_isBigEndian)
            {
                value = BinaryPrimitives.ReverseEndianness(value);
            }

            MemoryMarshal.Write(_data.Slice(_position), ref value);
            _position += sizeof(uint);
        }

        public void WriteVarInt(int value)
        {
            int size = GetVarIntSize(value);
            int pos = size - 1;

            Span<byte> buffer = _data.Slice(_position, size);
            buffer[pos] = (byte)(value & 0x7F);

            while ((value >>= 7) != 0)
            {
                buffer[--pos] = (byte)(0x80 | (value & 0x7F));
            }
            _position += size;
        }

        public static int GetVarIntSize(int number)
        {
            //TODO: C# 9.0 - Relational match pattern
            if (number > 268435455)
                return 5;
            if (number > 2097151)
                return 4;
            if (number > 16383)
                return 3;
            if (number > 127)
                return 2;
            return 1;
        }

        public void Write(ReadOnlySpan<char> value)
        {
            WriteVarInt(value.Length);

            int len = Encoding.UTF8.GetBytes(value, _data.Slice(_position));
            _position += len;
        }
        public void WriteNullString(ReadOnlySpan<char> value)
        {
            int len = Encoding.UTF8.GetBytes(value, _data.Slice(_position));
            _data[_position + len] = 0;

            _position += len + 1;
        }

        public void Write(Color value)
        {
            Write(value.R);
            Write(value.R);
            Write(value.G);
            Write(value.G);
            Write(value.B);
            Write(value.B);
        }
        public void Write(Point value)
        {
            Write((short)value.X);
            Write((short)value.Y);
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
