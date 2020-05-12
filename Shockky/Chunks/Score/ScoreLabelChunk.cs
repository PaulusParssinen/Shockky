using System.Linq;
using System.Text;
using System.Collections.Generic;

using Shockky.IO;

namespace Shockky.Chunks
{
    public class ScoreLabelChunk : ChunkItem
    {
        public Dictionary<short, string> Labels { get; set; }

        public ScoreLabelChunk()
            : base(ChunkKind.VWLB)
        {
            Labels = new Dictionary<short, string>();
        }
        public ScoreLabelChunk(ref ShockwaveReader input, ChunkHeader header)
            : base(header)
        {
            var offsetMap = new (short frame, int offset)[input.ReadInt16()];
            Labels = new Dictionary<short, string>(offsetMap.Length);

            for (int i = 0; i < offsetMap.Length; i++)
            {
                short frame = input.ReadInt16();
                short offset = input.ReadInt16();
                offsetMap[i] = (frame, offset);
            }

            string labels = input.ReadString(input.ReadInt32());

            for (int i = 0; i < offsetMap.Length; i++)
            {
                var (frame, offset) = offsetMap[i];

                if (i == offsetMap.Length - 1)
                    Labels[frame] = labels[offset..];
                else
                    Labels[frame] = labels[offset..offsetMap[i + 1].offset];
            }
        }

        public override int GetBodySize()
        {
            int size = 0;
            size += sizeof(short);
            size += Labels.Count * (2 * sizeof(short));
            size += sizeof(int);
            size += Labels.Values.Sum(l => l.Length);
            return size;
        }

        public override void WriteBodyTo(ShockwaveWriter output)
        {
            string labels = string.Empty;

            output.Write(Labels.Count);
            foreach (var entry in Labels)
            {
                output.Write(entry.Key);
                output.Write(labels.Length);

                labels += entry.Value;
            }

            output.Write(labels.Length);
            output.Write(Encoding.UTF8.GetBytes(labels)); //TODO:
        }
    }
}