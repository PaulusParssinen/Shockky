using Shockky.IO;

namespace Shockky.Shockwave.Chunks
{
    //Called styledtext actually
    public class ScriptableTextChunk : ChunkItem
    {
        public string Text { get; set; }

        public ScriptableTextChunk(ShockwaveReader input, ChunkHeader header)
            : base(header)
        {
            int headerLength = input.ReadBigEndian<int>();
            int textLength = input.ReadBigEndian<int>();
            int footerLength = input.ReadBigEndian<int>();

            Text = input.ReadString(textLength);

            //TODO: if footer Length > 0? and more validation

            short arrLength = input.ReadBigEndian<short>();
            for (short i2 = 0; i2 < arrLength; i2++)
            {
                int offset = input.ReadBigEndian<int>();
                int constantiguess = input.ReadBigEndian<int>();
                int unk3 = input.ReadInt32();
                short unk4 = input.ReadBigEndian<short>();
				/*string color = */
				input.ReadBytes(6); //input.ReadString(6);

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
