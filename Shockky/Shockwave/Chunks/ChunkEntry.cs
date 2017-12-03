using System.Collections.Generic;
using System.Diagnostics;
using Shockky.IO;

namespace Shockky.Shockwave.Chunks
{
    [DebuggerDisplay("{Header.Name} | Length: {Header.Length} | Offset: {Offset}")]
    public class ChunkEntry
    {
        public List<int> LinkedEntries { get; set; }

        public ChunkHeader Header { get; set; }

        public int Offset { get; set; }
        public short Padding { get; set; }
        public int Link { get; set; }

        public ChunkEntry(ref ShockwaveReader input, bool hasEntryData = true)
        {
            Header = new ChunkHeader(ref input); // 

            if (hasEntryData)
            {
                Offset = input.ReadInt32(); //offset of the chunk
                Padding = input.ReadInt16(); //no fuckign idea how this is even used yet
                input.ReadInt16(); //tf
                Link = input.ReadInt32(); //some kind of linki
            }
        }
    }
}
