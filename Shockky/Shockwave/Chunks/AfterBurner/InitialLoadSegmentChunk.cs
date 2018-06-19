using System;
using System.Collections.Generic;
using Shockky.IO;
using Shockky.Shockwave.Chunks.Container;
using Shockky.Shockwave.Chunks.Interface;

namespace Shockky.Shockwave.Chunks
{
    public class InitialLoadSegmentChunk : ChunkItem
    {
        private readonly ShockwaveReader _input;

        public InitialLoadSegmentChunk(ShockwaveReader input, ChunkHeader header)
            : base(header)
        {
            _input = input;
        }
        
        public bool TryReadChunks(Dictionary<int, IChunkEntry> entries, out List<ChunkItem> chunks)
        {
            chunks = new List<ChunkItem>(entries.Count);

            //TODO: Remove "unnecessary" checks and locals
            for(int i = 0; i < entries.Count; i++)
            {
                int id = _input.Read7BitEncodedInt();

                if (!entries.ContainsKey(id))
                    return false;

                var entry = entries[id];
                var chunk = ChunkContainer.ReadChunk(_input, entry.Header);
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
