using System;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

namespace Shockky.IO
{
    //TODO: Measure measure. BPrimitives vs Bitshifting - BPrimitives.ReverseEndiannes fast tho
    public ref struct ShockwaveReader
    {
        private readonly bool _bigEndian;
        private readonly ReadOnlySpan<byte> _data;

        private int _position;
        public int Position => _position; // { get; private set; }
        public int Length => _data.Length;

        //TODO: _currentSpan => _data.Slice(_position);

        public bool IsDataAvailable => _position < _data.Length;

        public ShockwaveReader(ReadOnlySpan<byte> data, bool bigEndian = false)
        {
            _data = data;
            _position = 0;
            _bigEndian = bigEndian;
        }

        //TODO: Measure, measure and measure
        public T ReadReverseEndian<T>()
            where T : struct
        {
            int size = Marshal.SizeOf<T>();
            T value;

            if (!_bigEndian)
            {
                Span<byte> buffer = stackalloc byte[size];
                _data.Slice(_position, size).CopyTo(buffer);

                buffer.Reverse();
                value = MemoryMarshal.Read<T>(buffer);
            }
            else value = MemoryMarshal.Read<T>(_data.Slice(_position));

            _position += size;
            return value;
        }

        //TODO: Try-methods? yoloing this right now
        //TODO: Check inlining

        //TODO: V Position { get; set; }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Advance(int count) => _position += count;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AdvanceTo(int offset) => _position = offset;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte ReadByte() => _data[_position++];

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadOnlySpan<byte> ReadBytes(int count)
        {
            ReadOnlySpan<byte> data = _data.Slice(_position, count);
            Advance(count);
            return data;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ReadBytes(Span<byte> buffer)
        {
            _data.Slice(_position, buffer.Length).CopyTo(buffer);
            Advance(buffer.Length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool ReadBoolean() => _data[_position++] == 1;

        //Should be one less branch (only emit bswap when), should ignore machine endianness afaik
        //TODO: BigEndian => "ReverseEndian"
        public short ReadInt16()
        {
            short value = MemoryMarshal.Read<short>(_data.Slice(_position));
            Advance(sizeof(short));
        
            return _bigEndian ? 
                BinaryPrimitives.ReverseEndianness(value) : value;
        }
        public short ReadBEInt16()
        {
            short value = MemoryMarshal.Read<short>(_data.Slice(_position));
            Advance(sizeof(short));

            return _bigEndian ?
                value : BinaryPrimitives.ReverseEndianness(value);
        }

        public ushort ReadUInt16()
        {
            ushort value = MemoryMarshal.Read<ushort>(_data.Slice(_position));
            Advance(sizeof(ushort));

            return _bigEndian ?
                BinaryPrimitives.ReverseEndianness(value) : value;
        }
        public ushort ReadBEUInt16()
        {
            ushort value = MemoryMarshal.Read<ushort>(_data.Slice(_position));
            Advance(sizeof(ushort));

            return _bigEndian ?
                value : BinaryPrimitives.ReverseEndianness(value);
        }

        public int ReadInt32()
        {
            int value = MemoryMarshal.Read<int>(_data.Slice(_position));
            Advance(sizeof(int));

            return _bigEndian ?
                BinaryPrimitives.ReverseEndianness(value) : value;
        }
        public int ReadBEInt32()
        {
            int value = MemoryMarshal.Read<int>(_data.Slice(_position));
            Advance(sizeof(int));

            return _bigEndian ?
                value : BinaryPrimitives.ReverseEndianness(value);
        }

        public uint ReadUInt32()
        {
            uint value = MemoryMarshal.Read<uint>(_data.Slice(_position));
            Advance(sizeof(uint));

            return _bigEndian ?
                BinaryPrimitives.ReverseEndianness(value) : value;
        }
        public uint ReadBEUInt32()
        {
            uint value = MemoryMarshal.Read<uint>(_data.Slice(_position));
            Advance(sizeof(uint));

            return _bigEndian ?
                value : BinaryPrimitives.ReverseEndianness(value);
        }

        public double ReadDouble()
        {
            double value = _bigEndian ?
                BitConverter.Int64BitsToDouble(BinaryPrimitives.ReverseEndianness(MemoryMarshal.Read<long>(_data.Slice(_position)))) :
                MemoryMarshal.Read<double>(_data.Slice(_position));

            Advance(sizeof(double));
            return value;
        }
        public double ReadBEDouble()
        {
            double value = _bigEndian ?
                MemoryMarshal.Read<double>(_data.Slice(_position)) :
                BitConverter.Int64BitsToDouble(BinaryPrimitives.ReverseEndianness(MemoryMarshal.Read<long>(_data.Slice(_position))));

            Advance(sizeof(double));
            return value;
        }

        public int Read7BitEncodedInt()
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
        
        // TODO: Check inlining. Compare Encoding path perfs. aand how implicit casts to ROS<char> work out in IL
        public string ReadString()
        {
            byte length = ReadByte();
            return Encoding.UTF8.GetString(ReadBytes(length));
        }
        public string ReadString(int length)
        {
            return Encoding.UTF8.GetString(ReadBytes(length));
        }
        public string ReadNullString()
        {
            int length = _data.Slice(_position).IndexOf((byte)0);
            string value = Encoding.UTF8.GetString(_data.Slice(_position, length));

            //TODO: Check? maxlength?
            //if (length == -1) 
            //  length = _data.Length - _position;

            _position += length + 1; //+ terminator

            return value;
        }

        public Color ReadColor()
        {
            // [R, R, G, G, B, B]
            byte r = _data[_position];
            byte g = _data[_position + 2];
            byte b = _data[_position + 4];

            Advance(6);
            return Color.FromArgb(r, g, b);
        }

        public Rectangle ReadRect()
        {
            short top = ReadInt16();
            short left = ReadInt16();
            short bottom = ReadInt16();
            short right = ReadInt16();

            return Rectangle.FromLTRB(left, top, right, bottom);
        }

        //TODO: PrefixedReader
        public void PopulateVList<T>(int offset,
            List<T> list, Func<T> reader, bool forceLengthCheck = true)
        {
            if (forceLengthCheck && list.Capacity == 0) return;

            AdvanceTo(offset);

            for (int i = 0; i < list.Capacity; i++)
            {
                T value = reader();
                list.Add(value);
            }
        }
    }
}