using System.Runtime.CompilerServices;

namespace Shockky.IO;

/// <summary>
/// Offset table used storing 'sparse' structures.
/// </summary>
public readonly struct OffsetTable(uint[] offsets)
{
    public int Length => offsets.Length - 1;

    public static OffsetTable Read(ref ShockwaveReader input)
    {
        int count = input.ReadUInt16BigEndian();
        uint[] offsets = new uint[count + 1];
        for (int i = 0; i < offsets.Length; i++)
        {
            offsets[i] = input.ReadUInt32BigEndian();
        }
        return new OffsetTable(offsets);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public uint GetEntrySize(int index)
    {
        if (index >= offsets.Length - 1) return 0;
        return offsets[index + 1] - offsets[index];
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool HasEntry(int index) => GetEntrySize(index) > 0;

    public uint GetEntryRangeSize(int startIndex)
    {
        if (startIndex >= offsets.Length - 1) return 0;
        return offsets[offsets.Length - 1] - offsets[startIndex];
    }
}