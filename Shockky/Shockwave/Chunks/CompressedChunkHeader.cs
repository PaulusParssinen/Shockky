using Shockky.IO;
using Shockky.Shockwave.Chunks.Enum;
using System;

namespace Shockky.Shockwave.Chunks
{
    public class CompressedChunkHeader : ChunkHeader // inaccurate name also
    {
        public CompressedChunkHeader(ChunkKind kind)
            : base(kind)
        { }
        public CompressedChunkHeader(ShockwaveReader input)
            : base(input.ReadReversedString(4))
        {
            int length = input.Read7BitEncodedInt();
            
            Length = length < 1 ?
                input.Length - input.Position : length;
        }

        public override int GetBodySize()
        {
            throw new NotImplementedException();
            int size = 0;
            //size += lenvarint
            return size;
        }

        public override void WriteTo(ShockwaveWriter output)
        {
            throw new NotImplementedException();
        }
    }
}
