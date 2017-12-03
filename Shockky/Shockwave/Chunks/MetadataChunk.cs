using Shockky.IO;

namespace Shockky.Shockwave.Chunks
{
    public class MetadataChunk : ChunkItem //Useless for now, I don't feel like I need this
    {
        public int FileLength { get; set; }
        public string Codec { get; set; }

        public MetadataChunk(ref ShockwaveReader input, ChunkEntry entry)
            : base(entry.Header)
        {
            //This is odd shit..
            //FileLength = input.ReadInt32();
            //Codec = input.ReadString();
        }
    }
}
