using Shockky.IO;

namespace Shockky.Shockwave.Chunks.Interface
{
    public interface IChunkEntry
    {
        ChunkHeader Header { get; set; }

        int Id { get; set; }
        int Offset { get; set; }

        bool IsCompressed { get; }

        int GetBodySize();
        void WriteTo(ShockwaveWriter output);
    }
}
