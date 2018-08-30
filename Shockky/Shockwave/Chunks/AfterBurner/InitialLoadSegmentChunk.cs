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
        
        public bool TryReadChunks(List<AfterBurnerMapEntry> entries, out List<ChunkItem> chunks)
        {
            chunks = new List<ChunkItem>(entries.Count);

            for(int i = 0; i < entries.Count; i++)
            {
                int id = _input.Read7BitEncodedInt();

                var entry = entries.FirstOrDefault(e => e.Id == id);
                if (entry == null) throw new Exception("fuq"); //TODO: Is this possible?

                var chunk = Read(_input, entry.Header);
                chunks.Add(chunk);
            }
            return true;
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
