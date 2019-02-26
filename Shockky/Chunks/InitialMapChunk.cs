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
        public InitialMapChunk(ShockwaveReader input, ChunkHeader header)
            : base(header)
        {
            MemoryMapOffsets = new int[input.ReadInt32()];
            for (int i = 0; i < MemoryMapOffsets.Length; i++)
            {
                MemoryMapOffsets[i] = input.ReadInt32();
            }

            Version = (DirectorVersion)input.ReadInt32();
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
            output.Write(MemoryMapOffsets.Length);
            for (int i = 0; i < MemoryMapOffsets.Length; i++)
            {
                output.Write(MemoryMapOffsets[i]);
            }
            output.Write((int)Version);
        }
    }
}
