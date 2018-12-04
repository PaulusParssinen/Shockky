using System;

using Shockky.IO;

namespace Shockky.Shockwave.Chunks.Cast
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

        public int[] RegistrationPoints { get; set; } //Film and video stuff?

        public string ClipboardFormat { get; set; }

        public int CreationDate { get; set; }
        public int ModifiedDate { get; set; }

        public string ModifiedBy { get; set; }
        public string Comments { get; set; }
        
        public int ImageCompression { get; set; }
        public int ImageQuality { get; set; }

        public CommonMemberProperties(ShockwaveReader input)
        {
            int[] propertyOffsets = new int[input.ReadBigEndian<short>() + 1];
            for (int i = 0; i < propertyOffsets.Length; i++)
            {
                propertyOffsets[i] = input.ReadBigEndian<int>();
            }

            for (int i = 0; i < propertyOffsets.Length - 1; i++)
            {
                int length = propertyOffsets[i + 1] - propertyOffsets[i];
                if (length == 0) continue;

                ReadProperty(input, i, length);
            }
        }

        private void ReadProperty(ShockwaveReader input, int index, int length)
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
                    break;
                case 10:
                    XtraName = input.ReadNullString();
                    break;
                case 12:
                    RegistrationPoints = new int[length / 4];
                    for (int i = 0; i < RegistrationPoints.Length; i++)
                    {
                        RegistrationPoints[i] = input.ReadBigEndian<int>();
                    }
                    break;
                case 16:
                    ClipboardFormat = input.ReadString(length);
                    break;
                case 17:
                    CreationDate = input.ReadBigEndian<int>() * 1000;
                    break;
                case 18:
                    ModifiedDate = input.ReadBigEndian<int>() * 1000;
                    break;
                case 19:
                    ModifiedBy = input.ReadString(length);
                    break;
                case 20:
                    Comments = input.ReadString(length);
                    break;
                case 21:
                    byte[] imageFlags = input.ReadBytes(length); //4

                    ImageCompression = imageFlags[0] >> 4;
                    ImageQuality = imageFlags[1];
                    break;
                case 7:
                default:
                    byte[] unknown = input.ReadBytes(length);
                    break;
            }
        }

        public override int GetBodySize()
        {
            throw new NotImplementedException();
        }

        public override void WriteTo(ShockwaveWriter output)
        {
            throw new NotImplementedException();
        }
    }
}
