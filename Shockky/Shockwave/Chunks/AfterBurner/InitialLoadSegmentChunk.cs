using System;
using System.Collections.Generic;
using System.Linq;
using Shockky.IO;

namespace Shockky.Shockwave.Chunks
{
    public class InitialLoadSegmentChunk : ChunkItem
    {
        private readonly ShockwaveReader _input;

        public InitialLoadSegmentChunk(ShockwaveReader input, ChunkHeader header)
            : base(header)
        {
            _input = WrapDecompressor(input);
        }
        
        public IEnumerable<ChunkItem> ReadChunks(List<AfterBurnerMapEntry> entries)
        {
            for(int i = 0; i < entries.Count; i++)
            {
                int id = _input.Read7BitEncodedInt();

                var entry = entries.FirstOrDefault(e => e.Id == id);
                if (entry == null || entry.Offset != -1) break; //That's enough I guess
                
                yield return Read(_input, entry.Header);
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
