using System;

using Shockky.IO;

namespace Shockky.Shockwave.Chunks.Score
{
    public class SpriteProperties : ShockwaveItem
    {
        public int StartFrame { get; set; }
        public int EndFrame { get; set; }

        public int Channel { get; set; }
        public int SpritePropertiesOffsetsOffset { get; set; }

        public SpriteProperties(ShockwaveReader input)
        {
            StartFrame = input.ReadBigEndian<int>(); 
            EndFrame = input.ReadBigEndian<int>();

            input.ReadBigEndian<int>();

            Channel = input.ReadBigEndian<int>();
            SpritePropertiesOffsetsOffset = input.ReadBigEndian<int>();

            input.Position += 28;
        }

        public override int GetBodySize()
        {
            int size = 0;
            size += sizeof(int);
            size += sizeof(int);

            size += sizeof(int);

            size += sizeof(int);
            size += sizeof(int);
            size += 28;
            return size;
        }

        public override void WriteTo(ShockwaveWriter output)
        {
            throw new NotImplementedException();
            output.WriteBigEndian(StartFrame);
            output.WriteBigEndian(EndFrame);
            output.WriteBigEndian(0);
            output.WriteBigEndian(Channel);
            output.WriteBigEndian(SpritePropertiesOffsetsOffset);
        }
    }
}
