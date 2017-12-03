using System;
using Shockky.IO;

namespace Shockky.Shockwave.Chunks
{
    public class DRCFChunk : ChunkItem
    {
        public string VersionHex { get; set; }

        public DRCFChunk(ShockwaveReader input, ChunkEntry entry)
            : base(entry.Header)
        {
            input.Position += 36; //yeah no idea what data is in there
            var versionBytes = input.ReadBytes(2);
            
            VersionHex = BitConverter.ToString(versionBytes)
                .Replace("-", string.Empty);;
        }
    }
}
