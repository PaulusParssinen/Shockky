using Shockky.IO;

namespace Shockky.Shockwave.Chunks
{
    public class FileInfoChunk : ChunkItem
    {
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }

        public string FilePath { get; set; }

        public FileInfoChunk(ShockwaveReader input, ChunkEntry entry)
            : base(entry.Header)
        {
            int bitfieldLen = input.ReadInt32(true);
            byte[] bitField = input.ReadBytes(bitfieldLen);

            short offsetCount = input.ReadInt16(true);
            int[] offsets = new int[offsetCount];

            input.ReadByte();

            for (short i = 0; i < offsetCount; i++)
            {
                offsets[i] = input.ReadInt32(true);
            }

            input.ReadByte();
            CreatedBy = input.ReadString();
            input.ReadByte();
            ModifiedBy = input.ReadString();
            input.ReadByte();
            FilePath = input.ReadString();
        }
    }
}
