using System.Collections.Generic;
using System.IO;
using Shockky.IO;
using Shockky.Shockwave.Chunks.Enum;

namespace Shockky.Shockwave.Chunks
{
    public class CastChunk : ChunkItem
    {
        private readonly ShockwaveReader _input;

        public byte[] BitField { get; set; }
        public List<int> Offsets { get; }

        private readonly int _specificDataEndOffset;
        private readonly long _dataAlignmentOffset;
        
        public CastChunk(ShockwaveReader input, ChunkEntry entry)
            : base(entry.Header)
        {
            _input = input;

            var castType = (CastType) input.ReadBigEndian<int>();
            _specificDataEndOffset = input.ReadBigEndian<int>();
            int castDataLength = input.ReadBigEndian<int>();

            int bitFieldLength = input.ReadBigEndian<int>();
            BitField = input.ReadBytes(bitFieldLength - 4);

            short offsetCount = input.ReadBigEndian<short>();

            Offsets = input.ReadBigEndianList<int>(offsetCount);

            int finalDataLength = input.ReadBigEndian<int>();

            _dataAlignmentOffset = input.Position;

            for (int i = 0; i < offsetCount; i++)
            {
                
            }

            string scriptText = ReadProperty<string>(0);

            string name = ReadProperty<string>(1, true);
            var unk1 = ReadProperty<byte[]>(2);
            var unk2 = ReadProperty<byte[]>(3);
            string comment = ReadProperty<string>(4);
	        var unk3 = ReadProperty<byte[]>(5);
	        var unk5 = ReadProperty<byte[]>(6);
	        var unk6 = ReadProperty<byte[]>(7);
	        var unk7 = ReadProperty<byte[]>(8);
	        byte[] xtraGuid = ReadProperty<byte[]>(9);
	        var unk8 = ReadProperty<byte[]>(10);
	        var unk9 = ReadProperty<byte[]>(11);
	        var unk10 = ReadProperty<byte[]>(12);
	        var unk11 = ReadProperty<byte[]>(13);
	        var unk12 = ReadProperty<byte[]>(14);
	        var unk13 = ReadProperty<byte[]>(15);
	        string fileFormat = ReadProperty<string>(16);
	        int created = ReadProperty<int>(17);
	        int modified = ReadProperty<int>(18);
	        var unk14 = ReadProperty<byte[]>(19);
	        var unk15 = ReadProperty<byte[]>(20);
	        var imageCompression = ReadProperty<byte[]>(21);

		}

        private T ReadProperty<T>(int index, bool prefixedString = false) //TODO: Didn't want to make own method for prefixed strings
        {
            int offsetTableLen = Offsets.Count - 1; //TODO: whatsup with the last offset? maybe cut it out
            if (index >= offsetTableLen)
	            return default(T);

            _input.Position = _dataAlignmentOffset + Offsets[index];

	        var nextOffset = index < offsetTableLen ?
		        Offsets[++index] : _specificDataEndOffset;

	        object value = null;

			if (typeof(T) == typeof(string))
            {
                if (prefixedString)
	                value = _input.ReadString();
				else value = _input.ReadString(nextOffset - Offsets[index]);
            }else if (typeof(T) == typeof(int))
				value = _input.ReadBigEndian<int>();
		//	else value = _input.ReadBytes(nextOffset - Offsets

			return (T) value;
        }

        public override int GetBodySize()
        {
            int size = 0;
            size += sizeof(int);
            size += sizeof(int);
            size += sizeof(int);
            size += sizeof(int);
            size += BitField.Length;
            size += sizeof(short);
	        size += sizeof(int)* Offsets.Capacity;

			return size;
        }

        public override void WriteTo(ShockwaveWriter output)
        {
            throw new System.NotImplementedException();
        }
    }
}
