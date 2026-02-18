using System.Buffers;
using System.Buffers.Binary;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Text;

using Shockky.Resources;

namespace Shockky.IO;

#nullable enable

// TODO: Use extensions on ROS<byte> with ReaderContext
public ref struct ShockwaveReader
{
    private readonly ReadOnlySpan<byte> _data;

    /// <summary>
    /// If the underlying data was authored in big-endian byte-order, reverse the endianness of each value read.
    /// </summary>
    public bool ReverseEndianness { get; set; }

    public int Position { get; set; }

    public readonly int Length => _data.Length;
    public readonly bool IsDataAvailable => Position < _data.Length;

    private readonly ReadOnlySpan<byte> CurrentSpan => _data.Slice(Position);

    public ShockwaveReader(ReadOnlySpan<byte> data, bool reverseEndianness = false)
    {
        _data = data;

        Position = 0;
        ReverseEndianness = reverseEndianness;
    }

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

    public short ReadInt16LittleEndian()
    {
        short value = BinaryPrimitives.ReadInt16LittleEndian(CurrentSpan);
        Advance(sizeof(short));

        return ReverseEndianness ?
            BinaryPrimitives.ReverseEndianness(value) : value;
    }
    public short ReadInt16BigEndian()
    {
        short value = BinaryPrimitives.ReadInt16BigEndian(CurrentSpan);
        Advance(sizeof(short));

        return ReverseEndianness ?
            BinaryPrimitives.ReverseEndianness(value) : value;
    }

    public ushort ReadUInt16LittleEndian()
    {
        ushort value = BinaryPrimitives.ReadUInt16LittleEndian(CurrentSpan);
        Advance(sizeof(ushort));

        return ReverseEndianness ?
            BinaryPrimitives.ReverseEndianness(value) : value;
    }
    public ushort ReadUInt16BigEndian()
    {
        ushort value = BinaryPrimitives.ReadUInt16BigEndian(CurrentSpan);
        Advance(sizeof(ushort));

        return ReverseEndianness ?
            BinaryPrimitives.ReverseEndianness(value) : value;
    }

    public int ReadInt32LittleEndian()
    {
        int value = BinaryPrimitives.ReadInt32LittleEndian(CurrentSpan);
        Advance(sizeof(int));

        return ReverseEndianness ?
            BinaryPrimitives.ReverseEndianness(value) : value;
    }
    public int ReadInt32BigEndian()
    {
        int value = BinaryPrimitives.ReadInt32BigEndian(CurrentSpan);
        Advance(sizeof(int));

        return ReverseEndianness ?
            BinaryPrimitives.ReverseEndianness(value) : value;
    }

    public uint ReadUInt32LittleEndian()
    {
        uint value = BinaryPrimitives.ReadUInt32LittleEndian(CurrentSpan);
        Advance(sizeof(uint));

        return ReverseEndianness ?
            BinaryPrimitives.ReverseEndianness(value) : value;
    }
    public uint ReadUInt32BigEndian()
    {
        uint value = BinaryPrimitives.ReadUInt32BigEndian(CurrentSpan);
        Advance(sizeof(uint));

        return ReverseEndianness ?
            BinaryPrimitives.ReverseEndianness(value) : value;
    }

    public ulong ReadUInt64LittleEndian()
    {
        ulong value = BinaryPrimitives.ReadUInt64LittleEndian(CurrentSpan);
        Advance(sizeof(ulong));

        return ReverseEndianness ?
            BinaryPrimitives.ReverseEndianness(value) : value;
    }
    public ulong ReadUInt64BigEndian()
    {
        ulong value = BinaryPrimitives.ReadUInt64BigEndian(CurrentSpan);
        Advance(sizeof(ulong));

        return ReverseEndianness ?
            BinaryPrimitives.ReverseEndianness(value) : value;
    }
    public double ReadDoubleLittleEndian()
    {
        double value = ReverseEndianness ?
            BinaryPrimitives.ReadDoubleBigEndian(CurrentSpan) :
            BinaryPrimitives.ReadDoubleLittleEndian(CurrentSpan);

        Advance(sizeof(double));
        return value;
    }
    public double ReadDoubleBigEndian()
    {
        double value = ReverseEndianness ?
            BinaryPrimitives.ReadDoubleLittleEndian(CurrentSpan) :
            BinaryPrimitives.ReadDoubleBigEndian(CurrentSpan);

        Advance(sizeof(double));
        return value;
    }

    public int Read7BitEncodedInt()
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

    public string ReadString()
    {
        int length = Read7BitEncodedInt();
        return Encoding.UTF8.GetString(ReadBytes(length));
    }
    public string ReadString(int length)
    {
        return Encoding.UTF8.GetString(ReadBytes(length));
    }
    public string ReadNullString()
    {
        int length = CurrentSpan.IndexOf((byte)0);
        Debug.Assert(length != -1);
        string value = Encoding.UTF8.GetString(_data.Slice(Position, length));

        Position += length + 1;
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
    public Point ReadPointLittleEndian()
    {
        return new(ReadInt16LittleEndian(), ReadInt16LittleEndian());
    }
    public Rectangle ReadRectLittleEndian()
    {
        short top = ReadInt16LittleEndian();
        short left = ReadInt16LittleEndian();
        short bottom = ReadInt16LittleEndian();
        short right = ReadInt16LittleEndian();

        return Rectangle.FromLTRB(left, top, right, bottom);
    }

    public Point ReadPointBigEndian()
    {
        return new(ReadInt16BigEndian(), ReadInt16BigEndian());
    }
    public Rectangle ReadRectBigEndian()
    {
        short top = ReadInt16BigEndian();
        short left = ReadInt16BigEndian();
        short bottom = ReadInt16BigEndian();
        short right = ReadInt16BigEndian();

        return Rectangle.FromLTRB(left, top, right, bottom);
    }

    public unsafe IResource ReadCompressedResource(AfterburnerMapEntry entry, ReaderContext context)
    {
        const int StackallocThreshold = 512;

        byte[]? rentedBuffer = null;
        try
        {
            Span<byte> decompressedData = entry.DecompressedLength <= StackallocThreshold ?
                stackalloc byte[StackallocThreshold] :
                (rentedBuffer = ArrayPool<byte>.Shared.Rent(entry.DecompressedLength));

            decompressedData = decompressedData.Slice(0, entry.DecompressedLength);

            ZLib.DecompressUnsafe(_data.Slice(Position, entry.Length), decompressedData);
            Advance(entry.Length);

            var input = new ShockwaveReader(decompressedData, ReverseEndianness);
            return IResource.Read(ref input, context, entry.Kind, entry.DecompressedLength);
        }
        finally
        {
            if (rentedBuffer is not null)
                ArrayPool<byte>.Shared.Return(rentedBuffer);
        }
    }
}