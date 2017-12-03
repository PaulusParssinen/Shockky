using Shockky.IO;

namespace Shockky.Shockwave.Chunks
{
    public class ScriptableTextChunk : ChunkItem
    {
        public string Text { get; set; }

        public ScriptableTextChunk(ShockwaveReader input, ChunkEntry entry)
            : base(entry.Header)
        {
            int headerLength = input.ReadInt32(true);
            int textLength = input.ReadInt32(true);
            int footerLength = input.ReadInt32(true);

            Text = input.ReadString(textLength);

            //TODO: if footer Length > 0? and more validation

            short arrLength = input.ReadInt16(true);
            for (short i2 = 0; i2 < arrLength; i2++)
            {
                int offset = input.ReadInt32(true);
                int constantiguess = input.ReadInt32(true);
                int unk3 = input.ReadInt32();
                short unk4 = input.ReadInt16(true);
                string color = input.ReadString(6);
            }
        }
    }
}
