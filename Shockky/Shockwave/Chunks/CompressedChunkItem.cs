using System;
using Shockky.Compression;
using Shockky.IO;

namespace Shockky.Shockwave.Chunks
{
    public abstract class CompressedChunkItem : ChunkItem
    {
        private long _offset;

        protected CompressedChunkItem(ref ShockwaveReader input, ChunkHeader header, bool wrapDecompressor)
            : base(header)
        {
            _offset = input.Position;

            if(wrapDecompressor)
                input = WrapDecompressor(input, header.Length);
        }
        protected ShockwaveReader WrapDecompressor(ShockwaveReader input)
        {
            return WrapDecompressor(input, Header.Length - (input.Position - _offset));
        }
        protected ShockwaveReader WrapDecompressor(ShockwaveReader input, long length)
        {
            input = input.Cut(length);
            return ZLIB.WrapDecompressor(input.BaseStream);
        } 

        public override int GetBodySize()
        {
            throw new NotSupportedException();
        }

        public override void WriteTo(ShockwaveWriter output)
        {
            //Wrap somekind of compressor here?

            base.WriteTo(output);
        }
    }
}
