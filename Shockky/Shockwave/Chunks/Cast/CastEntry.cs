using System.Diagnostics;
using Shockky.IO;
using Shockky.Shockwave.Chunks.Enum;
using Shockky.Utilities;

namespace Shockky.Shockwave.Chunks.Cast
{
    [DebuggerDisplay("{Name} | File Slot: {FileSlot} | Slot: {Slot}")]
    public class CastEntry : ShockwaveItem
    {
        public int FileSlot { get; set; }
        public int Slot { get; set; }

        public string Name { get; set; }
        public ChunkType Type { get; set; }

        public CastEntry(ref ShockwaveReader input)
        {
            FileSlot = input.ReadInt32();
            Slot = input.ReadInt32();

            Name = input.ReadString(4, true);
            Type = Name.ToChunkType();
        }
    }
}
