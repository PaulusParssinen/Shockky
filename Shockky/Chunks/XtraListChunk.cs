using System;

using Shockky.IO;

namespace Shockky.Chunks
{
    public class XtraListChunk : ChunkItem
    {
        public XtraListChunk()
            : base(ChunkKind.XTRl)
        { }
        public XtraListChunk(ShockwaveReader input, ChunkHeader header)
            : base(header)
        {
            Remnants.Enqueue(input.ReadBigEndian<int>());

            int xtraCount = input.ReadBigEndian<int>();
            for (int i = 0; i < xtraCount; i++)
            {
                long xtraEndOffset = input.Position + input.ReadBigEndian<int>() + 4;

                int unk2 = input.ReadBigEndian<int>();
                int unk3 = input.ReadBigEndian<int>(); //Flags or just booleans?
                Guid guid = new Guid(input.ReadBytes(16));

                int[] offsets = new int[input.ReadBigEndian<short>() + 1];
                for (int j = 0; j < offsets.Length; j++)
                {
                    offsets[j] = input.ReadBigEndian<int>();
                }

                do
                {
                    byte unk4 = input.ReadByte(); // 1 when kind = URL
                    byte kind = input.ReadByte(); // 2 -> Name | 0 -> URL, 5 -> File, x32 ?!? no idea

                    string str = input.ReadString();
                    input.ReadByte();
                }
                while (input.Position != xtraEndOffset);
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
