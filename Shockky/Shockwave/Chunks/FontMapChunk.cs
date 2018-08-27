using System.Text;
using Shockky.IO;

namespace Shockky.Shockwave.Chunks
{
    public class FontMapChunk : ChunkItem
    {
        public string FontMap { get; set; }

        public FontMapChunk(ShockwaveReader input, ChunkHeader header)
            : base(header)
        {
            FontMap = input.ReadString((int)header.Length);
        }

        public override int GetBodySize()
        {
            int size = 0;
            size += FontMap.Length;
            return size;
        }

        public override void WriteBodyTo(ShockwaveWriter output)
        {
            output.Write(Encoding.UTF8.GetBytes(FontMap));
        }
    }
}
