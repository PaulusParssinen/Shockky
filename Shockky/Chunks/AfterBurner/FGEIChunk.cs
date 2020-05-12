using System;
using System.Collections.Generic;

using Shockky.IO;

namespace Shockky.Chunks
{
    public class FGEIChunk : ChunkItem
    {
        public FGEIChunk(ref ShockwaveReader input, ChunkHeader header)
            : base(header)
        { }
        
        public InitialLoadSegmentChunk ReadInitialLoadSegment(ref ShockwaveReader input, AfterBurnerMapEntry ilsEntry)
        {
            input.AdvanceTo(ilsEntry.Offset);
            
            return Read(ref input, ilsEntry.Header) as InitialLoadSegmentChunk;
        }

        public void ReadChunks(ref ShockwaveReader input, IEnumerable<AfterBurnerMapEntry> entries, Action<ChunkItem> callback)
        {
            foreach (AfterBurnerMapEntry entry in entries)
            {
                if (entry.Offset < 1) continue;

                input.AdvanceTo(Header.Offset + entry.Offset);

                throw new NotImplementedException();
                //ShockwaveReader chunkInput = (entry.IsCompressed ?
                //    _input.WrapDecompressor(entry.CompressedLength) : _input);
                //
                //callback?.Invoke(Read(chunkInput, entry.Header));
            }
        }

        public override int GetBodySize()
        {
            throw new NotImplementedException();
        }

        public override void WriteBodyTo(ShockwaveWriter output)
        {
            throw new NotImplementedException();
        }
    }
}
