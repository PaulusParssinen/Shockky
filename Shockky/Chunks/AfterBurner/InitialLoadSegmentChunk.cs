using System.Collections.Generic;
using System.Linq;
using System;

using Shockky.IO;

namespace Shockky.Chunks
{
    public class InitialLoadSegmentChunk : ChunkItem
    {
        public InitialLoadSegmentChunk(ref ShockwaveReader input, ChunkHeader header)
            : base(header)
        { }
        
        public void ReadChunks(ref ShockwaveReader input, List<AfterBurnerMapEntry> entries, Action<ChunkItem> callback)
        {
            throw new NotImplementedException();

            int ilsChunkCount = entries.Count(e => e.Offset == -1);

            for (int i = 0; i < ilsChunkCount; i++)
            {
                if (input.Position >= Header.Length)
                    break;

                int id = input.Read7BitEncodedInt();

                AfterBurnerMapEntry entry = entries.FirstOrDefault(e => e.Header.Id == id);
                if (entry == null)
                    break; //TODO:

                if (entry.Header.Kind == ChunkKind.Unknown) //TODO: Probably temporary (as )
                    continue;

                entry.Header.Offset = input.Position;
                callback?.Invoke(Read(ref input, entry.Header));
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
