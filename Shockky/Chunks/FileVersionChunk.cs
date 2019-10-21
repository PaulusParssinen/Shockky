using System.Text;

using Shockky.IO;

namespace Shockky.Chunks
{
    public class FileVersionChunk : ChunkItem
    {
        public byte[] What { get; }
        public string Version { get; set; }

        public FileVersionChunk()
            : base(ChunkKind.Fver)
        { }
        public FileVersionChunk(ShockwaveReader input, ChunkHeader header)
            : base(header)
        {
            What = input.ReadBytes(5);
            Version = input.ReadString();
        }

        public override int GetBodySize()
        {
            int size = 0;
            size += 5; //TODO
            size += Encoding.UTF8.GetByteCount(Version) + 1;
            return size;
        }

        public override void WriteBodyTo(ShockwaveWriter output)
        {
            output.Write(What);
            output.Write(Version);
        }
    }
}
