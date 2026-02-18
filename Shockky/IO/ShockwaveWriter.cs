using System.Text;
using System.Drawing;
using System.Numerics;
using System.Buffers.Binary;
using System.Runtime.CompilerServices;

using Shockky.Resources.Cast;

namespace Shockky.IO;

public ref struct ShockwaveWriter
{
    private int _position;
    private readonly bool _reverseEndianness;
    private readonly Span<byte> _data;

    public readonly Span<byte> CurrentSpan => _data.Slice(_position);

    public ShockwaveWriter(Span<byte> data, bool reverseEndianness)
    {
        _data = data;
        _position = 0;
        _reverseEndianness = reverseEndianness;
    }

    //TODO: Measure, with and without inlining
    //Advance? - Zero fill variant?

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Advance(int count) => _position += count;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void WriteByte(byte value) => _data[_position++] = value;

    public void WriteBytes(ReadOnlySpan<byte> value)
    {
        value.CopyTo(_data.Slice(_position));
        _position += value.Length;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void WriteBoolean(bool value)
    {
        _data[_position++] = (byte)(value ? 1 : 0);
    }

    public void WriteInt16LittleEndian(short value)
    {
        if (_reverseEndianness)
        {
            value = BinaryPrimitives.ReverseEndianness(value);
        }

        BinaryPrimitives.WriteInt16LittleEndian(_data.Slice(_position), value);
        _position += sizeof(short);
    }
    public void WriteInt16BigEndian(short value)
    {
        if (_reverseEndianness)
        {
            value = BinaryPrimitives.ReverseEndianness(value);
        }

        BinaryPrimitives.WriteInt16BigEndian(_data.Slice(_position), value);
        _position += sizeof(short);
    }

    public void WriteUInt16LittleEndian(ushort value)
    {
        if (_reverseEndianness)
        {
            value = BinaryPrimitives.ReverseEndianness(value);
        }

        BinaryPrimitives.WriteUInt16LittleEndian(_data.Slice(_position), value);
        _position += sizeof(ushort);
    }
    public void WriteUInt16BigEndian(ushort value)
    {
        if (_reverseEndianness)
        {
            value = BinaryPrimitives.ReverseEndianness(value);
        }

        BinaryPrimitives.WriteUInt16BigEndian(_data.Slice(_position), value);
        _position += sizeof(ushort);
    }

    public void WriteInt32LittleEndian(int value)
    {
        if (_reverseEndianness)
        {
            value = BinaryPrimitives.ReverseEndianness(value);
        }

        BinaryPrimitives.WriteInt32LittleEndian(_data.Slice(_position), value);
        _position += sizeof(int);
    }
    public void WriteInt32BigEndian(int value)
    {
        if (_reverseEndianness)
        {
            value = BinaryPrimitives.ReverseEndianness(value);
        }

        BinaryPrimitives.WriteInt32BigEndian(_data.Slice(_position), value);
        _position += sizeof(int);
    }

    public void WriteUInt32LittleEndian(uint value)
    {
        if (_reverseEndianness)
        {
            value = BinaryPrimitives.ReverseEndianness(value);
        }

        BinaryPrimitives.WriteUInt32LittleEndian(_data.Slice(_position), value);
        _position += sizeof(uint);
    }
    public void WriteUInt32BigEndian(uint value)
    {
        if (_reverseEndianness)
        {
            value = BinaryPrimitives.ReverseEndianness(value);
        }

        BinaryPrimitives.WriteUInt32BigEndian(_data.Slice(_position), value);
        _position += sizeof(uint);
    }

    public void WriteUInt64LittleEndian(ulong value)
    {
        if (_reverseEndianness)
        {
            value = BinaryPrimitives.ReverseEndianness(value);
        }

        BinaryPrimitives.WriteUInt64LittleEndian(_data.Slice(_position), value);
        _position += sizeof(ulong);
    }
    public void WriteUInt64BigEndian(ulong value)
    {
        if (_reverseEndianness)
        {
            value = BinaryPrimitives.ReverseEndianness(value);
        }

        BinaryPrimitives.WriteUInt64BigEndian(_data.Slice(_position), value);
        _position += sizeof(ulong);
    }

    public void Write7BitEncodedInt(int value) => Write7BitEncodedUInt((uint)value);
    public void Write7BitEncodedUInt(uint value)
    {
        // TODO: Optimize
        int size = GetVarUIntSize(value);
        int pos = size - 1;

        Span<byte> buffer = _data.Slice(_position, size);
        buffer[pos] = (byte)(value & 0x7F);

        while ((value >>= 7) != 0)
        {
            buffer[--pos] = (byte)(0x80 | (value & 0x7F));
        }
        _position += size;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int GetVarIntSize(int value) => GetVarUIntSize((uint)value);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int GetVarUIntSize(uint value)
    {
        // bits_to_encode = (data != 0) ? 32 - CLZ(x) : 1  // 32 - CLZ(data | 1) 
        // bytes = ceil(bits_to_encode / 7.0);             // (6 + bits_to_encode) / 7
        int x = 6 + 32 - BitOperations.LeadingZeroCount(value | 1);
        // Division by 7 is done by (x * 37) >> 8 where 37 = ceil(256 / 7).
        // This works for 0 <= x < 256 / (7 * 37 - 256), i.e. 0 <= x <= 85.
        return (x * 37) >> 8;
    }

    /// <summary>
    /// Writes length-prefixed UTF-8 string. 
    /// </summary>
    /// <param name="value">The UTF-8 string to write.</param>
    public void WriteString(ReadOnlySpan<char> value)
    {
        Write7BitEncodedUInt((uint)value.Length);

        int len = Encoding.UTF8.GetBytes(value, _data.Slice(_position));
        _position += len;
    }
    
    /// <summary>
    /// Writes a null-terminated UTF-8 string.
    /// </summary>
    /// <param name="value">The UTF-8 string to write.</param>
    public void WriteCString(ReadOnlySpan<char> value)
    {
        int len = Encoding.UTF8.GetBytes(value, _data.Slice(_position));
        _data[_position + len] = 0;
        _position += len + 1;
    }

    public void WriteColor(Color color) => WriteColor(color.R, color.G, color.B);
    public void WriteColor(byte r, byte g, byte b)
    {
        // Eliminate bounds-checks.
        var buffer = _data.Slice(_position, 6);

        buffer[0] = r;
        buffer[1] = r;

        buffer[2] = g;
        buffer[3] = g;

        buffer[4] = b;
        buffer[5] = b;

        Advance(6);
    }
    
    // TODO: Endianness
    public void WritePoint(Point value)
    {
        WriteInt16LittleEndian((short)value.X);
        WriteInt16LittleEndian((short)value.Y);
    }
    public void WriteRect(Rectangle value)
    {
        WriteInt16LittleEndian((short)value.Top);
        WriteInt16LittleEndian((short)value.Left);
        WriteInt16LittleEndian((short)value.Bottom);
        WriteInt16LittleEndian((short)value.Right);
    }
    public void WriteMemberIdLittleEndian(CastMemberId memberId)
    {
        WriteInt16LittleEndian(memberId.CastLib);
        WriteInt16LittleEndian(memberId.MemberNum);
    }
    public void WriteMemberIdBigEndian(CastMemberId memberId)
    {
        WriteInt16BigEndian(memberId.CastLib);
        WriteInt16BigEndian(memberId.MemberNum);
    }
}
