using System.Diagnostics;
using Shockky.IO;

namespace Shockky.Shockwave.Chunks
{
    [DebuggerDisplay("{Header.Name} | Length: {Header.Length}")]
    public class ChunkItem
    {
        public ChunkHeader Header { get; set; }

        public ChunkItem(ref ShockwaveReader input)
        {
            Header = new ChunkHeader(ref input);
        }
        public ChunkItem(ChunkHeader header)
        {
            Header = header;
        }
    }
}
