using System.Diagnostics;
using Shockky.IO;

namespace Shockky.Shockwave.Chunks
{
    [DebuggerDisplay("{Header.Name} | Length: {Header.Length}")]
    public abstract class ChunkItem : ShockwaveItem
    {
        public ChunkHeader Header { get; set; }

        protected ChunkItem(ShockwaveReader input)
            : this(new ChunkHeader(input))
        { }
        
        protected ChunkItem(ChunkHeader header)
        {
            Header = header;
        }
    }
}
