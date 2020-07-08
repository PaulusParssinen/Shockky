using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Buffers.Binary;
using System.IO.Compression;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

using Shockky.Chunks;

namespace Shockky.IO
{
    public ref struct ShockwaveReader
    {
        private readonly ReadOnlySpan<byte> _data;

        public bool IsBigEndian { get; set; }
        public int Position { get; set; }

        public readonly int Length => _data.Length;
        public readonly bool IsDataAvailable => Position < _data.Length;

        private readonly ReadOnlySpan<byte> _currentSpan => _data.Slice(Position);

        public ShockwaveReader(ReadOnlySpan<byte> data, bool isBigEndian = false)
        {
            _data = data;

            Position = 0;
            IsBigEndian = isBigEndian;
        }

        //TODO: Measure, measure and measure
        public T ReadReverseEndian<T>()
            where T : struct
        {
            int size = Marshal.SizeOf<T>();
            T value;

            if (!IsBigEndian)
            {
                Span<byte> buffer = stackalloc byte[size];
                _data.Slice(Position, size).CopyTo(buffer);

                buffer.Reverse();
                value = MemoryMarshal.Read<T>(buffer);
            }
            else value = MemoryMarshal.Read<T>(_currentSpan);

            Position += size;
            return value;
        }

        //TODO: Try-methods? yoloing this right now
        //TODO: Check inlining

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Advance(int count) => Position += count;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte ReadByte() => _data[Position++];

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadOnlySpan<byte> ReadBytes(int count)
        {
            ReadOnlySpan<byte> data = _data.Slice(Position, count);
            Advance(count);
            return data;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ReadBytes(Span<byte> buffer)
        {
            _data.Slice(Position, buffer.Length).CopyTo(buffer);
            Advance(buffer.Length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool ReadBoolean() => _data[Position++] == 1;

        //Should be one less branch (only emit bswap when), should ignore machine endianness afaik
        //TODO: BigEndian => "ReverseEndian"
        public short ReadInt16()
        {
            short value = MemoryMarshal.Read<short>(_currentSpan);
            Advance(sizeof(short));
        
            return IsBigEndian ? 
                BinaryPrimitives.ReverseEndianness(value) : value;
        }
        public short ReadBEInt16()
        {
            short value = MemoryMarshal.Read<short>(_currentSpan);
            Advance(sizeof(short));

            return IsBigEndian ?
                value : BinaryPrimitives.ReverseEndianness(value);
        }

        public ushort ReadUInt16()
        {
            ushort value = MemoryMarshal.Read<ushort>(_currentSpan);
            Advance(sizeof(ushort));

            return IsBigEndian ?
                BinaryPrimitives.ReverseEndianness(value) : value;
        }
        public ushort ReadBEUInt16()
        {
            ushort value = MemoryMarshal.Read<ushort>(_currentSpan);
            Advance(sizeof(ushort));

            return IsBigEndian ?
                value : BinaryPrimitives.ReverseEndianness(value);
        }

        public int ReadInt32()
        {
            int value = MemoryMarshal.Read<int>(_currentSpan);
            Advance(sizeof(int));

            return IsBigEndian ?
                BinaryPrimitives.ReverseEndianness(value) : value;
        }
        public int ReadBEInt32()
        {
            int value = MemoryMarshal.Read<int>(_currentSpan);
            Advance(sizeof(int));

            return IsBigEndian ?
                value : BinaryPrimitives.ReverseEndianness(value);
        }

        public uint ReadUInt32()
        {
            uint value = MemoryMarshal.Read<uint>(_currentSpan);
            Advance(sizeof(uint));

            return IsBigEndian ?
                BinaryPrimitives.ReverseEndianness(value) : value;
        }
        public uint ReadBEUInt32()
        {
            uint value = MemoryMarshal.Read<uint>(_currentSpan);
            Advance(sizeof(uint));

            return IsBigEndian ?
                value : BinaryPrimitives.ReverseEndianness(value);
        }

        public double ReadDouble()
        {
            double value = IsBigEndian ?
                BitConverter.Int64BitsToDouble(BinaryPrimitives.ReverseEndianness(MemoryMarshal.Read<long>(_currentSpan))) :
                MemoryMarshal.Read<double>(_currentSpan);

            Advance(sizeof(double));
            return value;
        }
        public double ReadBEDouble()
        {
            double value = IsBigEndian ?
                MemoryMarshal.Read<double>(_currentSpan) :
                BitConverter.Int64BitsToDouble(BinaryPrimitives.ReverseEndianness(MemoryMarshal.Read<long>(_currentSpan)));

            Advance(sizeof(double));
            return value;
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

        // TODO: Check inlining. Compare Encoding path perfs. aand how implicit casts to ROS<char> work out in IL
        public string ReadString()
        {
            int length = ReadVarInt();
            return Encoding.UTF8.GetString(ReadBytes(length));
        }
        public string ReadString(int length)
        {
            return Encoding.UTF8.GetString(ReadBytes(length));
        }
        public string ReadNullString()
        {
            int length = _currentSpan.IndexOf((byte)0);
            string value = Encoding.UTF8.GetString(_data.Slice(Position, length));

            //TODO: Check? maxlength?
            //if (length == -1) 
            //  length = _data.Length - Position;

            Position += length + 1; //+ terminator

            return value;
        }
        public string ReadInternationalString()
        {
            //Read 32-bit scalar values
            byte length = ReadByte();
            ReadOnlySpan<byte> data = ReadBytes(length * sizeof(int));

            return Encoding.UTF32.GetString(data);
        }

        public Color ReadColor()
        {
            // [R, R, G, G, B, B]
            byte r = _data[Position];
            byte g = _data[Position + 2];
            byte b = _data[Position + 4];

            Advance(6);
            return Color.FromArgb(r, g, b);
        }
        public Point ReadPoint()
        {
            return new Point(ReadInt16(), ReadInt16());
        }
        public Rectangle ReadRect()
        {
            short top = ReadInt16();
            short left = ReadInt16();
            short bottom = ReadInt16();
            short right = ReadInt16();

            return Rectangle.FromLTRB(left, top, right, bottom);
        }

        public unsafe ChunkItem ReadCompressedChunk(AfterBurnerMapEntry entry)
        {
            Span<byte> decompressedData = entry.DecompressedLength <= 1024 ?
                    stackalloc byte[entry.DecompressedLength] : new byte[entry.DecompressedLength];

            fixed (byte* pBuffer = &_data.Slice(Position + 2)[0]) //Skip ZLib header
            {
                using var stream = new UnmanagedMemoryStream(pBuffer, entry.Length - 2);
                using var deflateStream = new DeflateStream(stream, CompressionMode.Decompress);

                deflateStream.Read(decompressedData);
            }
            Advance(entry.Length);

            ShockwaveReader input = new ShockwaveReader(decompressedData, IsBigEndian);
            return ChunkItem.Read(ref input, entry.Header);
        }

        //TODO: PrefixedReader
        public void PopulateVList<T>(int offset,
            List<T> list, Func<T> reader, bool forceLengthCheck = true)
        {
            if (forceLengthCheck && list.Capacity == 0) return;

            Position = offset;

            for (int i = 0; i < list.Capacity; i++)
            {
                T value = reader();
                list.Add(value);
            }
        }
    }
}