using System.Diagnostics;
using Shockky.IO;
using Shockky.Shockwave.Chunks.Enum;
using Shockky.Utilities;

namespace Shockky.Shockwave.Chunks
{
    [DebuggerDisplay("{Name} | {Length}")]
    public class ChunkHeader
    {
        public string Name { get; set; }
        public ChunkType Type { get; set; }

        public int Length { get; set; }

        public ChunkHeader(ref ShockwaveReader input)
        {
            Name = input.ReadString(4, true); //that last boolean tells it to flip those damn bytes
            Type = Name.ToChunkType();
            Length = input.ReadInt32(); //Length
        }
    }
}
