using System.Collections.Generic;
using System.Linq;
using System;

using Shockky.IO;

namespace Shockky.Chunks
{
    public class InitialLoadSegmentChunk : ChunkItem
    {
        private readonly ShockwaveReader _input;

        public InitialLoadSegmentChunk(ShockwaveReader input, ChunkHeader header)
            : base(header)
        {
            _input = WrapDecompressor(input);
        }
        
        public void ReadChunks(List<AfterBurnerMapEntry> entries, Action<ChunkItem> callback)
        {
            int ilsChunkCount = entries.Count(e => e.Offset == -1);

            for (int i = 0; i < ilsChunkCount; i++)
            {
                if (_input.Position >= Header.Length) break;

                int id = _input.Read7BitEncodedInt();

                AfterBurnerMapEntry entry = entries.FirstOrDefault(e => e.Header.Id == id);
                if (entry == null) break; //TODO:

                entry.Header.Offset = _input.Position;
                callback?.Invoke(Read(_input, entry.Header));
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
