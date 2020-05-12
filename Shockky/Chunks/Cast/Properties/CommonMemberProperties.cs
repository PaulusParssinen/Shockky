using System;

using Shockky.IO;

namespace Shockky.Chunks.Cast
{
    public class CommonMemberProperties : ShockwaveItem
    {
        public string ScriptText { get; set; }
        public string Name { get; set; }

        public string FilePath { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }

        public Guid XtraGUID { get; set; }
        public string XtraName { get; set; }

        public int[] RegistrationPoints { get; set; }

        public string ClipboardFormat { get; set; }

        public int CreationDate { get; set; }
        public int ModifiedDate { get; set; }

        public string ModifiedBy { get; set; }
        public string Comments { get; set; }
        
        public int ImageCompression { get; set; }
        public int ImageQuality { get; set; }

        public CommonMemberProperties(ref ShockwaveReader input)
        {
            int[] propertyOffsets = new int[input.ReadInt16() + 1];
            for (int i = 0; i < propertyOffsets.Length; i++)
            {
                propertyOffsets[i] = input.ReadInt32();
            }

            for (int i = 0; i < propertyOffsets.Length - 1; i++)
            {
                int length = propertyOffsets[i + 1] - propertyOffsets[i];
                if (length < 1) continue;

                ReadProperty(ref input, i, length);
            }
        }
        
        private void ReadProperty(ref ShockwaveReader input, int index, int length)
        {
            switch (index)
            {
                case 0:
                    ScriptText = input.ReadString(length);
                    break;
                case 1:
                    Name = input.ReadString();
                    break;
                case 2:
                    FilePath = input.ReadString(length);
                    break;
                case 3:
                    FileName = input.ReadString(length);
                    break;
                case 4:
                    FileType = input.ReadString(length);
                    break;
                case 9:
                    XtraGUID = new Guid(input.ReadBytes(length));
                    //XtraGUID = input.Read<Guid>();
                    break;
                case 10:
                    XtraName = input.ReadNullString();
                    break;
                case 11: //TODO:
                    break;
                case 12:
                    RegistrationPoints = new int[length / 4];
                    for (int i = 0; i < RegistrationPoints.Length; i++)
                    {
                        RegistrationPoints[i] = input.ReadInt32();
                    }
                    break;
                case 16:
                    ClipboardFormat = input.ReadString(length);
                    break;
                case 17:
                    CreationDate = input.ReadInt32() * 1000;
                    break;
                case 18:
                    ModifiedDate = input.ReadInt32() * 1000;
                    break;
                case 19:
                    ModifiedBy = input.ReadNullString();
                    break;
                case 20:
                    Comments = input.ReadString(length);
                    break;
                case 21:
                    ReadOnlySpan<byte> imageFlags = input.ReadBytes(length); //4

                    ImageCompression = imageFlags[0] >> 4;
                    ImageQuality = imageFlags[1];
                    break;
                case 7: //TODO:
                default:
                    ReadOnlySpan<byte> unknown = input.ReadBytes(length);
                    break;
            }
        }

        //TODO:
        public override int GetBodySize()
        {
            int size = 0;
            throw new NotImplementedException();
            size += sizeof(int);
            size += 22 * sizeof(int); 

            //var output = new ShockwaveWriter();
            //for (int i = 0; i < 22; i++)
            //    WriteProperty(output, i, ref size);

            return size;
        }

        private void WriteProperty(ShockwaveWriter output, int index, ref int endOffset)
        {
            //TODO: Better checks & possibly "compression"?
            switch (index)
            {
                case 0 when !string.IsNullOrEmpty(ScriptText):
                    output.Write(ScriptText);
                    endOffset += ScriptText.Length;
                    break;
                case 1 when !string.IsNullOrEmpty(Name):
                    output.Write(Name);
                    endOffset += Name.Length + 1;
                    break;
                case 2 when !string.IsNullOrEmpty(FilePath):
                    output.Write(FilePath);
                    endOffset += FilePath.Length;
                    break;
                case 3 when !string.IsNullOrEmpty(FileName):
                    output.Write(FileName);
                    endOffset += FileName.Length;
                    break;
                case 4 when !string.IsNullOrEmpty(FileType):
                    output.Write(FileType);
                    endOffset += FileType.Length;
                    break;
                case 9 when XtraGUID != null:
                    output.Write(XtraGUID.ToByteArray());
                    //output.Write<T>()
                    endOffset += 16;
                    break;
                case 10 when !string.IsNullOrEmpty(XtraName):
                    output.WriteNullString(XtraName);
                    endOffset += XtraName.Length + 1;
                    break;
                case 12 when RegistrationPoints != null:
                    for (int i = 0; i < RegistrationPoints.Length; i++)
                    {
                        output.Write(RegistrationPoints[i]);
                    }
                    endOffset += RegistrationPoints.Length * sizeof(int);
                    break;
                case 16 when !string.IsNullOrEmpty(ClipboardFormat):
                    output.Write(ClipboardFormat);
                    endOffset += ClipboardFormat.Length;
                    break;
                case 17 when CreationDate != 0:
                    output.Write(CreationDate / 1000);
                    endOffset += sizeof(int);
                    break;
                case 18 when ModifiedDate != 0:
                    output.Write(ModifiedDate / 1000);
                    endOffset += sizeof(int);
                    break;
                case 19 when !string.IsNullOrEmpty(ModifiedBy):
                    output.WriteNullString(ModifiedBy);
                    endOffset += ModifiedBy.Length + 1; //TODO:
                    break;
                case 20 when !string.IsNullOrEmpty(Comments):
                    output.Write(Comments);
                    endOffset += Comments.Length;
                    break;
                case 21 when ImageCompression != 0 || ImageQuality != 0:
                    output.Write((byte)(ImageCompression << 4));
                    output.Write((byte)ImageQuality);
                    output.Write((byte)0);
                    output.Write((byte)0);
                    endOffset += 4;
                    break;
                default:
                    break;
            }
        }

        public override void WriteTo(ShockwaveWriter output)
        {
            throw new NotImplementedException(nameof(CommonMemberProperties));
            //output.Write(21); //TODO: "compress"
            //
            //int currentOffset = 0;
            //int[] propertyOffsets = new int[22];
            //
            //output.Position += 22 * sizeof(int);
            //
            //for (int i = 0; i < 22; i++)
            //{
            //    propertyOffsets[i] = currentOffset;
            //    WriteProperty(output, i, ref currentOffset);
            //}
            //
            //output.Position -= 22 * sizeof(int);
            //output.Position -= currentOffset;
            //
            //for (int i = 0; i < 22; i++)
            //{
            //    output.Write(propertyOffsets[i]);
            //}
            //
            //output.Position += currentOffset; //We are finished here, go to end
        }
    }
}
