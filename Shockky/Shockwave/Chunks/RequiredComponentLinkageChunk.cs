using System;

using Shockky.IO;

namespace Shockky.Shockwave.Chunks
{
    public class RequiredComponentLinkageChunk : ChunkItem
    {
        public RequiredComponentLinkageChunk(ShockwaveReader input, ChunkHeader header)
            : base(header)
        {
            int unk0 = input.ReadBigEndian<int>(); //unk for a reason
            int count = input.ReadBigEndian<int>(); 
            for (int i = 0; i < count; i++)
            {
                int count1 = input.ReadBigEndian<int>();
                int wtf = input.ReadBigEndian<int>();
                int realGuidLengthIthink = input.ReadBigEndian<int>();
                byte[] componentGUID = input.ReadBytes(realGuidLengthIthink);
                for (int j = 0; j < count1; j++)
                {
                    short unk2 = input.ReadBigEndian<short>();
                    string componentName = input.ReadString();
                    byte nullByte = input.ReadByte();
                }
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
