using Shockky.IO;
using System;
using System.Collections.Generic;

namespace Shockky.Shockwave.Chunks.Cast.Member.Properties
{
    public class CommonMemberProperties
    {
        public void ReadProperty(ShockwaveReader input,
            int index, int length)
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
                    RequiredComponentGUID = input.ReadBytes(length);
                    break;
                case 10:
                    RequiredComponentName = input.ReadString(length);
                    break;
                case 16:
                    var data = input.ReadBytes(length);
                    //ClipboardFormat = input.ReadString();
                    break;
                case 17:
                    CreationDate = input.ReadBigEndian<int>();
                    break;
                case 18:
                    CreationDate = input.ReadBigEndian<int>();
                    break;
                case 19:
                    ModifiedBy = input.ReadString(length);
                    break;
                case 20:
                    Comments = input.ReadString(length);
                    break;
                case 21:
                    byte[] temp = input.ReadBytes(2);
                    break;
            }
        }

        public string ScriptText { get; set; }
        public string Name { get; set; }

        public string FilePath { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }

        public byte[] RequiredComponentGUID { get; set; } //Xtra
        public string RequiredComponentName { get; set; }

        public string ClipboardFormat { get; set; }

        public int CreationDate { get; set; } //Date format, TODO
        public int ModifiedDate { get; set; }

        public string ModifiedBy { get; set; }
        public string Comments { get; set; }
        
        public bool ImageCompression { get; set; }
        public bool Quality { get; set; }
    }
}
