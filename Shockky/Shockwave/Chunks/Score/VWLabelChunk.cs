using System.Collections.Generic;
using System;

using Shockky.IO;
using System.Linq;

namespace Shockky.Shockwave.Chunks
{
    public class WVLabelChunk : ChunkItem
    {
        public Dictionary<int, string> Labels { get; set; }

        public WVLabelChunk(ShockwaveReader input, ChunkHeader header) 
            : base(header)
        {
            (short frame, int offset)[] offsetMap = new (short frame, int offset)[input.ReadBigEndian<short>()];
            Labels = new Dictionary<int, string>(offsetMap.Length);

            for (int i = 0; i < offsetMap.Length; i++)
            {
                short frame = input.ReadBigEndian<short>();
                short offset = input.ReadBigEndian<short>();
                offsetMap[i] = (frame, offset);
            }

            Span<char> labelsBuffer = stackalloc char[input.ReadBigEndian<int>()];
            input.Read(labelsBuffer);

            for (int i = 0; i < offsetMap.Length; i++)
            {
                bool isLast = (i == offsetMap.Length - 1);

                string label = (isLast ? labelsBuffer.Slice(offsetMap[i].offset) :
                    labelsBuffer.Slice(offsetMap[i].offset, offsetMap[i + 1].offset)).ToString();

                Labels[offsetMap[i].frame] = label;
            }
        }

        public override int GetBodySize()
        {
            int size = 0;
            size += sizeof(short);
            size += Labels.Count * (sizeof(short) * 2);
            size += sizeof(int);
            size += Labels.Values.Sum(l => l.Length);
            return size;
        }

        public override void WriteBodyTo(ShockwaveWriter output)
        {
            throw new NotImplementedException();
        }
    }
}
