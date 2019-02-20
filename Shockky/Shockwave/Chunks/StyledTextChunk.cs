using System.Diagnostics;
using System.Text;

using Shockky.IO;

namespace Shockky.Shockwave.Chunks
{
    public class StyledTextChunk : ChunkItem
    {
        public string Text { get; set; }
        public TextFormat[] Formattings { get; set; }

        public StyledTextChunk(ShockwaveReader input, ChunkHeader header)
            : base(header)
        {
            input.ReadBigEndian<int>();
            int textLength = input.ReadBigEndian<int>();
            input.ReadBigEndian<int>();

            Text = Encoding.UTF8.GetString(input.ReadBytes(textLength)); //TODO:

            Formattings = new TextFormat[input.ReadBigEndian<short>()];
            for (int i = 0; i < Formattings.Length; i++)
            {
                Formattings[i] = new TextFormat(input);
            }
        }

        public override void WriteBodyTo(ShockwaveWriter output)
        {
            const int TEXT_OFFSET = 12; //I guess
            const int TEXT_FORMAT_SIZE = 20;

            output.WriteBigEndian(TEXT_OFFSET);
            output.WriteBigEndian(Text.Length);
            output.WriteBigEndian(sizeof(short) + (Formattings.Length * TEXT_FORMAT_SIZE));

            output.Write(Text.ToCharArray());

            output.WriteBigEndian((short)(Formattings?.Length ?? 0));
            for (int i = 0; i < Formattings?.Length; i++)
            {
                Formattings[i].WriteTo(output);
            }
        }

        public override int GetBodySize()
        {
            const int TEXT_FORMAT_SIZE = 20;

            int size = 0;
            size += sizeof(int);
            size += sizeof(int);
            size += sizeof(int);
            size += Text.Length;
            size += sizeof(short);
            size += Formattings?.Length ?? 0 * TEXT_FORMAT_SIZE;
            return size;
        }
    }
}
