using Shockky.IO;

namespace Shockky.Chunks
{
    public class StyledTextChunk : ChunkItem
    {
        public string Text { get; set; }
        public TextFormat[] Formattings { get; set; }

        public StyledTextChunk()
            : base(ChunkKind.STXT)
        { }
        public StyledTextChunk(ref ShockwaveReader input, ChunkHeader header)
            : base(header)
        {
            input.ReadInt32();
            int textLength = input.ReadInt32();
            input.ReadInt32();

            Text = input.ReadString(textLength);

            Formattings = new TextFormat[input.ReadInt16()];
            for (int i = 0; i < Formattings.Length; i++)
            {
                Formattings[i] = new TextFormat(ref input);
            }
        }

        public override void WriteBodyTo(ShockwaveWriter output)
        {
            const int TEXT_OFFSET = 12;
            const int TEXT_FORMAT_SIZE = 20;

            output.Write(TEXT_OFFSET);
            output.Write(Text.Length);
            output.Write(sizeof(short) + (Formattings.Length * TEXT_FORMAT_SIZE));

            output.Write(Text); //TODO: 

            output.Write((short)(Formattings?.Length ?? 0));
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
