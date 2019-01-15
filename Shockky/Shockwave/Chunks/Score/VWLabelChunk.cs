using System.Collections.Generic;
using System.Linq;
using System.Text;

using Shockky.IO;

namespace Shockky.Shockwave.Chunks
{
    public class WVLabelChunk : ChunkItem
    {
        public Dictionary<short, string> Labels { get; set; }

        public WVLabelChunk(ShockwaveReader input, ChunkHeader header)
            : base(header)
        {
            var offsetMap = new (short frame, int offset)[input.ReadBigEndian<short>()];
            Labels = new Dictionary<short, string>(offsetMap.Length);

            for (int i = 0; i < offsetMap.Length; i++)
            {
                short frame = input.ReadBigEndian<short>();
                short offset = input.ReadBigEndian<short>();
                offsetMap[i] = (frame, offset);
            }

            string labels = input.ReadString(input.ReadBigEndian<int>());

            for (int i = 0; i < offsetMap.Length; i++)
            {
                var (frame, offset) = offsetMap[i];

                if (i == offsetMap.Length - 1)
                    Labels[frame] = labels.Substring(offset);
                else
                    Labels[frame] = labels.Substring(offset, offsetMap[i + 1].offset);
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

            output.WriteBigEndian(Labels.Count);
            foreach (var entry in Labels)
            {
                output.WriteBigEndian(entry.Key);
                output.WriteBigEndian(labels.Length);

                labels += entry.Value;
            }

            output.WriteBigEndian(labels.Length);
            output.Write(Encoding.UTF8.GetBytes(labels));
        }
    }
}