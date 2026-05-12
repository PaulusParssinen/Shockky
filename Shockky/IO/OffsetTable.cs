using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Shockky.IO;

/// <summary>
/// Offset table used for (de)serializing 'sparse' structures.
/// The terminal offset entry is the end of the last entry.
/// </summary>
public readonly ref struct OffsetTable
{
    private readonly ReadOnlySpan<int> _offsets;

    public OffsetTable(ReadOnlySpan<int> offsets)
    {
        _offsets = offsets;
        AssertInvariants();
    }

    public int Length => _offsets.Length > 1 ? _offsets.Length - 1 : 0;

    /// <summary>
    /// Total size of entry payloads (sum of all entry sizes).
    /// </summary>
    public int PayloadSize => _offsets.Length > 1 ? _offsets[^1] : 0;

    /// <summary>
    /// Byte size of the table itself: sizeof(ushort) + (Length + 1) * sizeof(int).
    /// Does not include entry payloads.
    /// </summary>
    public int GetTableSize() => sizeof(ushort) + _offsets.Length * sizeof(int);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int GetEntrySize(int index)
    {
        if (_offsets.Length < 2 || (uint)index >= (uint)(_offsets.Length - 1)) return 0;
        return _offsets[index + 1] - _offsets[index];
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool HasEntry(int index) => GetEntrySize(index) > 0;

    public int GetEntryRangeSize(int startIndex)
    {
        if (_offsets.Length < 2 || (uint)startIndex >= (uint)(_offsets.Length - 1)) return 0;
        return _offsets[^1] - _offsets[startIndex];
    }

    /// <summary>
    /// Writes the entry count and offset array.
    /// </summary>
    public void WriteTo(ref ShockwaveWriter output)
    {
        AssertInvariants();
        Debug.Assert(_offsets.Length > 0, "A serialized offset table must include the terminal offset.");
        Debug.Assert(Length <= ushort.MaxValue, "Offset table length must fit be representable as 16-bit unsigned integer.");

        output.WriteUInt16BigEndian((ushort)Length);
        for (int i = 0; i < _offsets.Length; i++)
        {
            output.WriteInt32BigEndian(_offsets[i]);
        }
    }

    public static OffsetTable Read(ref ShockwaveReader input)
    {
        int count = input.ReadUInt16BigEndian();
        int[] offsets = new int[count + 1];
        for (int i = 0; i < offsets.Length; i++)
        {
            offsets[i] = input.ReadInt32BigEndian();
        }
        return new OffsetTable(offsets);
    }

    /// <summary>
    /// Creates an offset table from entry sizes.
    /// </summary>
    public static OffsetTable Create(ReadOnlySpan<int> entrySizes)
    {
        int[] offsets = new int[entrySizes.Length + 1];
        SizesToOffsets(entrySizes, offsets);
        return new OffsetTable(offsets);
    }

    /// <summary>
    /// Converts entry sizes to offsets in-place and writes the offset table.
    /// <paramref name="entrySizes"/> must have length n+1 where [0..n) are sizes and [n] is a placeholder.
    /// After this call, the span contains cumulative offsets.
    /// </summary>
    internal static void CreateAndWriteTo(scoped Span<int> entrySizes, ref ShockwaveWriter output)
    {
        int count = entrySizes.Length - 1;
        SizesToOffsets(entrySizes);

        output.WriteUInt16BigEndian((ushort)count);
        for (int i = 0; i < entrySizes.Length; i++)
        {
            output.WriteInt32BigEndian(entrySizes[i]);
        }
    }

    private static void SizesToOffsets(scoped Span<int> entrySizesAndOffsets)
    {
        Debug.Assert(entrySizesAndOffsets.Length > 0, "Entry sizes span must include a terminal placeholder.");
        SizesToOffsets(entrySizesAndOffsets[..^1], entrySizesAndOffsets);
    }

    private static void SizesToOffsets(ReadOnlySpan<int> entrySizes, Span<int> offsets)
    {
        AssertCanConvertToOffsets(entrySizes, offsets);

        int offset = 0;
        for (int i = 0; i < entrySizes.Length; i++)
        {
            int size = entrySizes[i];
            offsets[i] = offset;
            if (size > 0)
            {
                offset += size;
            }
        }
        offsets[^1] = offset;
    }

    [Conditional("DEBUG")]
    private void AssertInvariants()
    {
        if (_offsets.IsEmpty) return;

        Debug.Assert(_offsets[0] == 0, "Offsets must start at zero.");

        int previous = 0;
        for (int i = 1; i < _offsets.Length; i++)
        {
            int current = _offsets[i];
            Debug.Assert(current >= previous, "Offsets must be nondecreasing.");
            previous = current;
        }
    }

    [Conditional("DEBUG")]
    private static void AssertCanConvertToOffsets(ReadOnlySpan<int> entrySizes, ReadOnlySpan<int> offsets)
    {
        Debug.Assert(offsets.Length == entrySizes.Length + 1, "Offset output must have one more element than entry sizes.");

        int totalSize = 0;
        foreach (int size in entrySizes)
        {
            Debug.Assert(size >= 0, "Entry sizes must not be negative.");
            if (size <= 0) continue;

            Debug.Assert(totalSize <= int.MaxValue - size, "Offset table payload size must fit be representable as 32-bit signed integer.");
            totalSize += size;
        }
    }
}