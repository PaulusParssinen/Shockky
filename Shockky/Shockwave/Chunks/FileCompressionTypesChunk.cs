using System;
using Shockky.IO;

namespace Shockky.Shockwave.Chunks
{
    public class FileCompressionTypesChunk : CompressedChunkItem
    {
        public FileCompressionTypesChunk(ShockwaveReader input, ChunkHeader header)
            : base(ref input, header, true)
        {
            //TODO: finish dis chunk
            var dataTest = input.ReadBytes((int)header.Length);

            /*short test1 = input.ReadInt16();
            int test2 = input.ReadInt32();*/
            
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
