using Shockky.IO;

namespace Shockky.Shockwave.Chunks
{
    public class StyledTextChunk : ChunkItem
    {
        public string Text { get; set; }

        public StyledTextChunk(ShockwaveReader input, ChunkHeader header)
            : base(header)
        {
            int headerLength = input.ReadBigEndian<int>();
            int textLength = input.ReadBigEndian<int>();
            int footerLength = input.ReadBigEndian<int>();

            Text = input.ReadString(textLength);

            short formattingCount = input.ReadBigEndian<short>();
            for (int i = 0; i < formattingCount; i++)
            {
                int offset = input.ReadBigEndian<int>();
                short height = input.ReadBigEndian<short>();
                short ascent = input.ReadBigEndian<short>();

                short fontId = input.ReadBigEndian<short>();
                bool slant = input.ReadBoolean();
                byte padding = input.ReadByte();
                short fontsize = input.ReadBigEndian<short>();

                //?
                short r = input.ReadBigEndian<short>();
                short g = input.ReadBigEndian<short>();
                short b = input.ReadBigEndian<short>();
            }
        }

        public override void WriteBodyTo(ShockwaveWriter output)
        {
            throw new System.NotImplementedException();
        }

        public override int GetBodySize()
        {
            int size = 0;
            size += sizeof(int);
            size += sizeof(int);
            size += sizeof(int);
            return size;
        }
    }
}
