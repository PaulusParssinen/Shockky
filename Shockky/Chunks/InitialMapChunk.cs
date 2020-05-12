using Shockky.IO;

namespace Shockky.Chunks
{
    public class InitialMapChunk : ChunkItem
    {
        public int[] MemoryMapOffsets { get; set; }

        public DirectorVersion Version { get; set; }

        public InitialMapChunk()
            : base(ChunkKind.imap)
        { }
        public InitialMapChunk(ref ShockwaveReader input, ChunkHeader header)
            : base(header)
        {
            MemoryMapOffsets = new int[input.ReadBEInt32()];
            for (int i = 0; i < MemoryMapOffsets.Length; i++)
            {
                MemoryMapOffsets[i] = input.ReadBEInt32();
            }
            Version = (DirectorVersion)input.ReadBEInt32();
        }

        public override int GetBodySize()
        {
            int size = 0;
            size += sizeof(int);
            size += sizeof(int) * MemoryMapOffsets.Length;
            size += sizeof(int);
            return size;
        }

        public override void WriteBodyTo(ShockwaveWriter output)
        {
            output.WriteBE(MemoryMapOffsets.Length);
            for (int i = 0; i < MemoryMapOffsets.Length; i++)
            {
                output.WriteBE(MemoryMapOffsets[i]);
            }
            output.WriteBE((int)Version);
        }
    }
}
