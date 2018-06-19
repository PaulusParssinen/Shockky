using Shockky.IO;
using System.Diagnostics;

namespace Shockky.Shockwave.Chunks
{
    [DebuggerDisplay("{Kind}")]
    public abstract class ChunkItem : ShockwaveItem
    {
        public ChunkKind Kind => Header.Kind;
        public ChunkHeader Header { get; set; }

        protected ChunkItem(ShockwaveReader input)
            : this(new ChunkHeader(input))
        { }
        
        protected ChunkItem(ChunkHeader header)
        {
            Header = header;
        }

        public override void WriteTo(ShockwaveWriter output)
        {
            Header.Length = GetBodySize();
            output.Write(Header);
            WriteBodyTo(output);
        }

        public abstract void WriteBodyTo(ShockwaveWriter output);
    }
}
