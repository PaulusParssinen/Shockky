using System.Diagnostics;

using Shockky.IO;

namespace Shockky.Chunks
{
    [DebuggerDisplay("{Header.Kind} | Length: {Header.Length}, Offset: {Offset}")]
    public class ChunkEntry : ShockwaveItem
    {
        public ChunkHeader Header { get; set; }

        public int Offset { get; set; } //TODO: vs. ChunkHeader offsets
        public ChunkEntryFlags Flags { get; set; }
        public short Unknown { get; set; }
        public int Link { get; set; }

        public ChunkEntry(ChunkHeader header)
        {
            Header = header;
        }
        public ChunkEntry(ref ShockwaveReader input)
            : this(new ChunkHeader(ref input))
        {
            Offset = input.ReadBEInt32();
            Flags = (ChunkEntryFlags)input.ReadBEInt16();
            Unknown = input.ReadBEInt16();
            Link = input.ReadBEInt32();
        }

        public override int GetBodySize()
        {
            int size = 0;
            size += Header.GetBodySize();
            size += sizeof(int);
            size += sizeof(short);
            size += sizeof(short);
            size += sizeof(int);
            return size;
        }

        public override void WriteTo(ShockwaveWriter output)
        {
            Header.WriteTo(output);
            output.WriteBE(Offset);
            output.WriteBE((short)Flags);
            output.WriteBE(Unknown);
            output.WriteBE(Link);
        }
    }
}