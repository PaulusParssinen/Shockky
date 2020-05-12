using System;
using System.Diagnostics;

using Shockky.IO;
using Shockky.Chunks.Score;

namespace Shockky.Chunks
{
    public class ScoreChunk : ChunkItem
    {
        public Frame[] Frames { get; set; }

        public ScoreChunk()
            : base(ChunkKind.VWSC)
        { }
        public ScoreChunk(ref ShockwaveReader input, ChunkHeader header)
            : base(header)
        {
            int totalLength = input.ReadInt32();
            int headerType = input.ReadInt32(); //-3

            int spritePropertiesOffsetThingy = input.ReadInt32();
            int[] spritePropertyOffsets = new int[input.ReadInt32() + 1];

            int notationOffset = input.ReadInt32() * 4 + 12 + spritePropertiesOffsetThingy;
            int lastSpritePropertyOffset = input.ReadInt32();

            for (int i  = 0; i < spritePropertyOffsets.Length; i++)
            {
                spritePropertyOffsets[i] = input.ReadInt32();
            }

            Debug.Assert(input.Position == (Header.Offset + notationOffset), "What");
            
            int frameEndOffset = input.ReadInt32();

            Remnants.Enqueue(input.ReadInt32());

            Frames = new Frame[input.ReadInt32()];
            short framesType = input.ReadInt16(); //13, 14

            short channelLength = input.ReadInt16();
            short lastChannelMax = input.ReadInt16(); //1006
            short lastChannel = input.ReadInt16();

            for (int i = 0; i < Frames.Length; i++)
            {
                Frames[i] = new Frame(ref input);
            }

            int[] spritePropertyOffsetIndices = new int[input.ReadInt32()];
            for (int i = 0; i < spritePropertyOffsetIndices.Length; i++)
            {
                spritePropertyOffsetIndices[i] = input.ReadInt32();
            }

            SpriteProperties[] spriteProperties = new SpriteProperties[spritePropertyOffsetIndices.Length];
            for (int i = 0; i < spritePropertyOffsetIndices.Length; i++)
            {
                int spritePropertyOffset = spritePropertyOffsets[spritePropertyOffsetIndices[i]];

                input.AdvanceTo(Header.Offset + notationOffset + spritePropertyOffset);
                spriteProperties[i] = new SpriteProperties(ref input);
            }
        }

        public override int GetBodySize()
        {
            throw new NotImplementedException();
            int size = 0;
            size += sizeof(int);
            size += sizeof(int);

            size += sizeof(int);
            size += sizeof(int);
            
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
