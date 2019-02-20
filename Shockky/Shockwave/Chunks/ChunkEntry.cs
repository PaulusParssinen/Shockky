using System.Diagnostics;

using Shockky.IO;

namespace Shockky.Shockwave.Chunks
{
    [DebuggerDisplay("{Header.Name} | Length: {Header.Length} | Offset: {Offset}")]
    public class ChunkEntry : ShockwaveItem
    {
        public ChunkHeader Header { get; set; }

        public int Offset { get; set; }
        public ChunkEntryFlags Flags { get; set; }
        public short Unknown { get; set; }
        public int Link { get; set; }

        public ChunkEntry(ChunkHeader header)
        {
            Header = header;
        }
        public ChunkEntry(ShockwaveReader input)
            : this(new ChunkHeader(input))
        {
            Offset = input.ReadInt32();
            Flags = (ChunkEntryFlags)input.ReadInt16();
            Unknown = input.ReadInt16();
            Link = input.ReadInt32();
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

            output.Write(Offset);
            output.Write((short)Flags);
            output.Write(Unknown);
            output.Write(Link);
        }
    }
}