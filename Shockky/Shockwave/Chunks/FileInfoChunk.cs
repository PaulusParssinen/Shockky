using System;
using System.Collections.Generic;
using System.Text;
using Shockky.IO;

namespace Shockky.Shockwave.Chunks
{
    public class FileInfoChunk : ChunkItem
    {
        public byte[] BitField { get; set; }

        public List<int> Offsets { get; set; }

        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }

        public string FilePath { get; set; }

        public FileInfoChunk(ShockwaveReader input, ChunkHeader header)
            : base(header)
        {
            int bitfieldLen = input.ReadBigEndian<int>();
            BitField = input.ReadBytes(bitfieldLen);

	        short offsetCount = input.ReadBigEndian<short>();

            int[] offsets = new int[offsetCount];

            input.ReadByte();

            for (short i = 0; i < offsetCount; i++)
            {
                offsets[i] = input.ReadBigEndian<int>();
            }

            input.ReadByte();
            CreatedBy = input.ReadString();
            input.ReadByte();
            ModifiedBy = input.ReadString();
            input.ReadByte();
            FilePath = input.ReadString();
        }

        public override void WriteBodyTo(ShockwaveWriter output)
        {
            output.Write(BitField.Length);
            output.Write(BitField);

            output.Write((ushort)Offsets.Count);
            //TODO
        }

        public override int GetBodySize()
        {
            int size = 0;
            size += sizeof(int);
            size += BitField.Length;

            size += sizeof(short);
            size += sizeof(byte);
            size += (sizeof(int) * Offsets.Count);
            size += sizeof(byte);

            size += (Encoding.ASCII.GetByteCount(CreatedBy) + 1);
            size += sizeof(byte);
            size += (Encoding.ASCII.GetByteCount(ModifiedBy) + 1);
            size += sizeof(byte);
            size += (Encoding.ASCII.GetByteCount(FilePath) + 1);

            return size;
        }
    }
}
