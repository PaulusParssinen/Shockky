using System.Diagnostics;

using Shockky.IO;

namespace Shockky.Resources;

[DebuggerDisplay("[{Index}] {Kind} Offset: {Offset}")]
public sealed class AfterburnerMapEntry : IShockwaveItem
{
    public OsType Kind { get; set; }
    public int Index { get; set; }
    public int Offset { get; set; }
    public int Length { get; set; }
    public int DecompressedLength { get; set; }

    /// <summary>
    /// Represents an index in <see cref="FileCompressionTypes.CompressionTypes"/> array.
    /// </summary>
    public int CompressionTypeIndex { get; set; }

    public AfterburnerMapEntry(ZLibShockwaveReader input)
    {
        Index = input.Read7BitEncodedInt();
        Offset = input.Read7BitEncodedInt();
        Length = input.Read7BitEncodedInt();
        DecompressedLength = input.Read7BitEncodedInt();
        CompressionTypeIndex = input.Read7BitEncodedInt();
        Kind = (OsType)input.ReadInt32BigEndian();
    }

    public int GetBodySize(WriterOptions options)
    {
        int size = 0;
        size += ShockwaveWriter.GetVarIntSize(Index);
        size += ShockwaveWriter.GetVarIntSize(Offset);
        size += ShockwaveWriter.GetVarIntSize(Length);
        size += ShockwaveWriter.GetVarIntSize(DecompressedLength);
        size += ShockwaveWriter.GetVarIntSize(CompressionTypeIndex);
        size += sizeof(int);
        return size;
    }

    public void WriteTo(ShockwaveWriter output, WriterOptions options)
    {
        output.Write7BitEncodedInt(Index);
        output.Write7BitEncodedInt(Offset);
        output.Write7BitEncodedInt(Length);
        output.Write7BitEncodedInt(DecompressedLength);
        output.Write7BitEncodedInt(CompressionTypeIndex);
        output.WriteInt32BigEndian((int)Kind);
    }
}