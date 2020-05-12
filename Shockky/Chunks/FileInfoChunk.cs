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
            /*
             * def parseSubstrings(self, data, hasHeader=True):
		if hasHeader:
			ci_offset = read32(data)
			unk2 = read32(data) # not int!
			unk3 = read32(data) # not int!
			entryType = read32(data)
			data.seek(ci_offset)
		else:
			unk2 = 0
			unk3 = 0
			entryType = 0

		count = read16(data) + 1
		entries = []
		for i in range(count):
			entries.append(read32(data))
		rawdata = data.read(entries[-1])
		assert entries[0] == 0
		assert entries[-1] == len(rawdata)

		strings = []
		for i in range(count-1):
			strings.append(rawdata[entries[i]:entries[i+1]])

		return (strings, unk2, unk3, entryType)

             * 
             * */

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
