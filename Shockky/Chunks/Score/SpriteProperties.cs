using System;

using Shockky.IO;

namespace Shockky.Chunks.Score
{
    public class SpriteProperties : ShockwaveItem
    {
        public int StartFrame { get; set; }
        public int EndFrame { get; set; }

        public int Channel { get; set; }
        public int SpritePropertiesOffsetsOffset { get; set; }

        public SpriteProperties()
        { }
        public SpriteProperties(ref ShockwaveReader input)
        {
            StartFrame = input.ReadInt32(); 
            EndFrame = input.ReadInt32();

            input.ReadInt32();

            Channel = input.ReadInt32();
            SpritePropertiesOffsetsOffset = input.ReadInt32();

            input.Advance(28);
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
            output.Write(StartFrame);
            output.Write(EndFrame);
            output.Write(0);
            output.Write(Channel);
            output.Write(SpritePropertiesOffsetsOffset);
        }
    }
}
