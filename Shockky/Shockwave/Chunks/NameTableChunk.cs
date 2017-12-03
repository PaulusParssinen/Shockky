using System.Collections.Generic;
using Shockky.IO;

namespace Shockky.Shockwave.Chunks
{
    public class NameTableChunk : ChunkItem
    {
        public List<string> Names { get; set; }

        public NameTableChunk(ShockwaveReader input, ChunkEntry entry)
            : base(entry.Header)
        {
            int unk0 = input.ReadInt32(true);
            int unk1 = input.ReadInt32(true);
            int length = input.ReadInt32(true);
            int length2 = input.ReadInt32(true);
            short nameOffset = input.ReadInt16(true);
            short nameCount = input.ReadInt16(true);

            Names = input.ReadStringList(nameCount, -1, nameOffset); //Reads shit load of strings
        }
    }
}
