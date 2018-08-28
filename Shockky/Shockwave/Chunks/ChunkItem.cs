using System.Collections.Generic;
using System.IO.Compression;
using System.Diagnostics;
using System.IO;

using Shockky.IO;

namespace Shockky.Shockwave.Chunks
{
    [DebuggerDisplay("{Kind}")]
    public abstract class ChunkItem : ShockwaveItem
    {
        private long _offset;

        public ChunkKind Kind => Header.Kind;
        public ChunkHeader Header { get; set; }

        public Queue<object> Remnants { get; set; }

        protected ChunkItem(ShockwaveReader input)
            : this(new ChunkHeader(input))
        {
            _offset = input.Position;
        }
        protected ChunkItem(ChunkHeader header)
        {
            Header = header;

            Remnants = new Queue<object>();
        }

        public ShockwaveReader WrapDecompressor(ShockwaveReader input)
        {
            input.BaseStream.Seek(2, SeekOrigin.Current);

            var data = input.ReadBytes((int)(Header.Length - (input.Position - _offset)));
            return new ShockwaveReader(new DeflateStream(new MemoryStream(data), CompressionMode.Decompress));
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
