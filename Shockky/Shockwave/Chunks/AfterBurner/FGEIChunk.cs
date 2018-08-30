using System;
using System.Collections.Generic;

using Shockky.IO;

namespace Shockky.Shockwave.Chunks
{
    //TODO: Naming of this thing
    public class FGEIChunk : ChunkItem
    {
        private readonly ShockwaveReader _input;
        private readonly long _offset;

        public FGEIChunk(ShockwaveReader input, ChunkHeader header)
            : base(input)
        {
            _input = input;
            _offset = input.Position;
        }
        
        //Gotta confirm that the first entry is ILS..
        public InitialLoadSegmentChunk ReadInitialLoadSegment(AfterBurnerMapEntry ilsEntry)
        {
            _input.Position += ilsEntry.Offset;
            
            return Read(_input, ilsEntry.Header) as InitialLoadSegmentChunk;
        }

        public ChunkItem[] ReadChunks(List<AfterBurnerMapEntry> entries)
        {
            var chunks = new ChunkItem[entries.Count];

            for (int i = 1; i < entries.Count; i++)
            {
                var entry = entries[i];

                if (entry.Offset == -1) continue;

                if (entry.IsCompressed)
                {
                    _input.Position = _offset + entry.Offset;
                    var decompressedInput = _input.WrapDecompressor(entry.DecompressedLength);

                    chunks[i] = Read(decompressedInput, entry.Header);
                }
                else throw new NotImplementedException("Gotta find an example movie which has this shit");
            }
            return chunks;
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
