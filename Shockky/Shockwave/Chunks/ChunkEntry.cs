using System.Collections.Generic;
using System.Diagnostics;
using Shockky.IO;

namespace Shockky.Shockwave.Chunks
{
    [DebuggerDisplay("{Header.Name} | Length: {Header.Length} | Offset: {Offset}")]
    public class ChunkEntry : ShockwaveItem
    {
        public int Index { get; set; }

        public List<int> LinkedEntries { get; set; }

        public ChunkHeader Header { get; set; }

        public int Offset { get; set; }
        public short Padding { get; set; }
        public int Link { get; set; }

        public ChunkEntry(ShockwaveReader input)
        {
            Header = new ChunkHeader(input);

            Offset = input.ReadInt32();
            Padding = input.ReadInt16();
            input.ReadInt16();
            Link = input.ReadInt32();
        }

        public ChunkEntry(ShockwaveReader input, int index, bool hasEntryData = true)
        {
            Index = index;

            Header = new ChunkHeader(input, index);

            if (!hasEntryData) return;

            Offset = input.ReadInt32();
            Padding = input.ReadInt16();
            input.ReadInt16();
            Link = input.ReadInt32();
        }

        public override int GetBodySize()
        {
            int size = 0;
            size += Header.GetBodySize();
            //TODO
            return size;
        }

        public override void WriteTo(ShockwaveWriter output)
        {
            output.Write(Header);
            //TODO
        }
    }
}