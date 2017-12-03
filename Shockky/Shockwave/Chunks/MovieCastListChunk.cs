using System;
using Shockky.IO;

namespace Shockky.Shockwave.Chunks
{
    public class MovieCastListChunk : ChunkItem
    {
        public int CastCount { get; set; }

        public MovieCastListChunk(ShockwaveReader input, ChunkEntry entry)
            : base(entry.Header)
        {
            int unk1 = input.ReadInt32();
            CastCount = input.ReadInt32();
            int unk2 = input.ReadInt32();
            int arraySize = input.ReadInt32();
            
            for(int i = 0; i < CastCount; i++)
            {
                int ar0 = input.ReadInt32();
                int ar1 = input.ReadInt32();
                int ar2 = input.ReadInt32();
                int ar3 = input.ReadInt32();

                Console.WriteLine($"[MCsL] (Reading) {i}. - 0: {ar0} | 1: {ar1} | 2: {ar2} | 3: {ar3}");
            }

            short unk3 = input.ReadInt16();
            int castLibrariesLength = input.ReadInt32();

            for (int i = 0; i < CastCount; i++)
            {
                int id = i;
                string name = input.ReadString();
            }
        }
    }
}
