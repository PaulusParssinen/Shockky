using System;
using System.Text;

using Shockky.IO;

namespace Shockky.Chunks
{
    public class FileVersionChunk : ChunkItem
    {
        public string Version { get; set; }

        public FileVersionChunk()
            : base(ChunkKind.Fver)
        { }
        public FileVersionChunk(ref ShockwaveReader input, ChunkHeader header)
            : base(header)
        {
            var unknown = input.ReadBytes(3); // likely varints?
            var unknown2 = input.ReadBytes(2);
            Version = new string(input.ReadString());
        }

        public override int GetBodySize()
        {
            int size = 0;
            size += 5; //TODO:
            size += Encoding.UTF8.GetByteCount(Version) + 1;
            return size;
        }

        public override void WriteBodyTo(ShockwaveWriter output)
        {
            throw new NotImplementedException();
            output.Write(Version);
        }
    }
}
