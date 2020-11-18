using System.Text;
using System.Collections.Generic;

using Shockky.IO;

namespace Shockky.Chunks
{
    public class FileInfoChunk : ChunkItem
    {
        public byte[] BitField { get; set; }

        public List<int> Offsets { get; set; }

        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public string FilePath { get; set; }

        public FileInfoChunk()
            : base(ChunkKind.VWFI)
        { }
        public FileInfoChunk(ref ShockwaveReader input, ChunkHeader header)
            : base(header)
        {
            input.IsBigEndian = true;

            //TODO: VList
            BitField = input.ReadBytes(input.ReadInt32()).ToArray();
            
	        Offsets = new List<int>(input.ReadInt16());
            
            input.ReadByte();
            
            for (short i = 0; i < Offsets.Capacity; i++)
            {
                Offsets.Add(input.ReadInt32());
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
            for(int i = 0; i < Offsets.Count; i++)
            {
                output.Write(Offsets[i]);
            }
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
