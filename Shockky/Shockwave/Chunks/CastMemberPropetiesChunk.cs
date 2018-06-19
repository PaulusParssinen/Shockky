using System.Diagnostics;
using Shockky.IO;
using Shockky.Shockwave.Chunks.Cast.Member.Properties;
using Shockky.Shockwave.Chunks.Enum;

namespace Shockky.Shockwave.Chunks
{
    public class CastMemberPropetiesChunk : ChunkItem
    {
        public CastType Type { get; set; }

        public CommonMemberProperties Common { get; set; }
            
        public byte[] BitField { get; }

        public CastMemberPropetiesChunk(ShockwaveReader input, ChunkHeader header)
            : base(header)
        {
            Type = (CastType)input.ReadBigEndian<int>();
            int dataEndOffset = input.ReadBigEndian<int>();
            int dataLength = input.ReadBigEndian<int>();
            
            int bitFieldLength = input.ReadBigEndian<int>();
            BitField = input.ReadBytes(bitFieldLength - 4);

            short offsetCount = input.ReadBigEndian<short>();

            var offsets = new int[offsetCount];
            for (int i = 0; i < offsetCount; i++)
            {
                offsets[i] = input.ReadBigEndian<int>();
            }

            int finalDataLength = input.ReadBigEndian<int>();

            long alignmentPosition = input.Position;

            Common = new CommonMemberProperties();

            for (int i = 0; i < offsetCount; i++)
            {
                int offset = (int)alignmentPosition + offsets[i];
                int end = (i < offsetCount - 1 ? offsets[i + 1] : dataEndOffset);
                
                input.Position = offset;

                Common.ReadProperty(input, i, end - offsets[i]); //TODO: ffs the custom type props
            }
            
            switch (Type)
            {
                case CastType.Bitmap:
                case CastType.OLE:
                    break;
                case CastType.Transition:
                case CastType.StyledText:
                case CastType.Picture:
                    //UNK
                    break;
                case CastType.Sound:
                    break;
                case CastType.Text:
                case CastType.Button:
                    break;
                case CastType.Shape:
                    break;
                case CastType.Xtra:
                case CastType.Movie:
                case CastType.FilmLoop:
                case CastType.DigitalVideo:
                    break;
                case CastType.Script:
                    break;
                case CastType.Palette:
                default:
                    break;
            }


            //string scriptText = ReadString(input, 0);
            //string name = ReadPrefixedString(input, 1);

            /*string name = ReadProperty<string>(1, true);
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
            */
        }
/*
        private string ReadString(ShockwaveReader input, int index)
        {
            int offset = Offsets[index];
            int nextOffset = index < Offsets.Count - 1 ? Offsets[index + 1] : _specificDataEndOffset;

            input.Position = _dataAlignmentOffset + offset;

            return input.ReadString(nextOffset - offset);
        }

        private string ReadPrefixedString(ShockwaveReader input, int index)
        {
            input.Position = _dataAlignmentOffset + Offsets[index];
            return input.ReadString();
        }
        
      readStringProperty(dataStream, index: number) {

    if (index >= this.offsetTable.length - 1) return null;
    let offset = this.offsetTable[index];
    let nextOffset = index < this.offsetTableLen - 1 ? this.offsetTable[index + 1] : this.specificDataOffset;
    let length = nextOffset - offset;
    dataStream.seek(this.dataOffset + offset);
    return dataStream.readString(length);
  }

  readPrefixedStringProperty(dataStream, index: number) {
    if (index >= this.offsetTable.length - 1) return null;

    let offset = this.offsetTable[index];
    dataStream.seek(this.dataOffset + offset);
    let length = dataStream.readUint8();
    return dataStream.readString(length);
}  

    */

       /* private T ReadProperty<T>(int index, bool prefixedString = false) //TODO: Didn't want to make own method for prefixed strings
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
        }*/

        public override int GetBodySize()
        {
            int size = 0;
            size += sizeof(int);
            size += sizeof(int);
            size += sizeof(int);
            size += sizeof(int);
            size += BitField.Length;
            size += sizeof(short);
            size += sizeof(int);// * Offsets.Capacity;

			return size;
        }

        public override void WriteBodyTo(ShockwaveWriter output)
        {
            throw new System.NotImplementedException();
        }
    }
}
