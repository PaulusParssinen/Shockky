using System;

using Shockky.IO;

namespace Shockky.Shockwave.Chunks
{
    public class FileCompressionTypesChunk : ChunkItem
    {
        public FileCompressionTypesChunk(ShockwaveReader input, ChunkHeader header)
            : base(header)
        {
            //TODO:
            var dataTest = input.ReadBytes((int)header.Length);
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
