using System;

using Shockky.IO;

namespace Shockky.Chunks
{
    //TODO: Is this always big endian?
    public class GridChunk : ChunkItem
    {
        public short Width { get; set; }
        public short Height { get; set; }
        public GridDisplay Display { get; set; }
        public short GridColor { get; set; }

        public short GuideColor { get; set; }
        public Guide[] Guides { get; set; }

        public GridChunk() 
            : base(ChunkKind.GRID)
        { }
        public GridChunk(ref ShockwaveReader input, ChunkHeader header) 
            : base(header)
        {
            Remnants.Enqueue(input.ReadInt32());

            Width = input.ReadInt16();
            Height = input.ReadInt16();
            Display = (GridDisplay)input.ReadInt16();
            GridColor = input.ReadInt16();

            Guides = new Guide[input.ReadInt16()];
            GuideColor = input.ReadInt16();
            for (int i = 0; i < Guides.Length; i++)
            {
                Guides[i] = new Guide(ref input);
            }
        }

        public override int GetBodySize()
        {
            const int GUIDE_ENTRY_SIZE = sizeof(short) + sizeof(short);

            int size = 0;
            size += sizeof(int);

            size += sizeof(short);
            size += sizeof(short);
            size += sizeof(short);
            size += sizeof(short);

            size += sizeof(short);
            size += sizeof(short);
            size += Guides.Length * GUIDE_ENTRY_SIZE;
            return size;
        }

        public override void WriteBodyTo(ShockwaveWriter output)
        {
            output.Write((int)Remnants.Dequeue());

            output.Write(Height);
            output.Write(Width);
            output.Write((short)Display);
            output.Write(GridColor);

            output.Write((short)Guides.Length);
            output.Write(GuideColor);
            foreach (Guide guide in Guides)
            {
                guide.WriteTo(output);
            }
        }
    }
}
