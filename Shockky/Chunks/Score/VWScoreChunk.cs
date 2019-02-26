using System.Diagnostics;
using System;

using Shockky.Chunks.Score;
using Shockky.IO;

namespace Shockky.Chunks
{
    public class VWScoreChunk : ChunkItem
    {
        public Frame[] Frames { get; }

        public VWScoreChunk(ShockwaveReader input, ChunkHeader header)
            : base(header)
        {
            int totalLength = input.ReadBigEndian<int>();
            int headerType = input.ReadBigEndian<int>(); //-3

            int spritePropertiesOffsetThingy = input.ReadBigEndian<int>();
            int[] spritePropertyOffsets = new int[input.ReadBigEndian<int>() + 1];

            int notationOffset = input.ReadBigEndian<int>() * 4 + 12 + spritePropertiesOffsetThingy;
            int lastSpritePropertyOffset = input.ReadBigEndian<int>();

            for (int i  = 0; i < spritePropertyOffsets.Length; i++)
            {
                spritePropertyOffsets[i] = input.ReadBigEndian<int>();
            }

            Debug.Assert(input.Position == (Header.Offset + notationOffset), "What");
            
            int frameEndOffset = input.ReadBigEndian<int>();

            Remnants.Enqueue(input.ReadBigEndian<int>());

            Frames = new Frame[input.ReadBigEndian<int>()];
            short framesType = input.ReadBigEndian<short>(); //13, 14

            short channelLength = input.ReadBigEndian<short>();
            short lastChannelMax = input.ReadBigEndian<short>(); //1006
            short lastChannel = input.ReadBigEndian<short>();

            for (int i = 0; i < Frames.Length; i++)
            {
                Frames[i] = new Frame(input);
            }

            int[] spritePropertyOffsetIndices = new int[input.ReadBigEndian<int>()];
            for (int i = 0; i < spritePropertyOffsetIndices.Length; i++)
            {
                spritePropertyOffsetIndices[i] = input.ReadBigEndian<int>();
            }

            SpriteProperties[] spriteProperties = new SpriteProperties[spritePropertyOffsetIndices.Length];
            for (int i = 0; i < spritePropertyOffsetIndices.Length; i++)
            {
                int spritePropertyOffset = spritePropertyOffsets[spritePropertyOffsetIndices[i]];

                input.Position = Header.Offset + notationOffset + spritePropertyOffset;
                spriteProperties[i] = new SpriteProperties(input);
            }
        }

        public override int GetBodySize()
        {
            throw new NotImplementedException();
            int size = 0;
            size += sizeof(int);
            size += sizeof(int);
            return size;
        }

        public override void WriteBodyTo(ShockwaveWriter output)
        {
            throw new NotImplementedException();
        }
    }
}
