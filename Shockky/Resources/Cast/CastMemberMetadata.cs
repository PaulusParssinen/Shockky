using System.Diagnostics;

using Shockky.IO;

namespace Shockky.Resources.Cast;

// TODO: Generalize VList parsing logic
public sealed class CastMemberMetadata : IResource, IShockwaveItem
{
    public OsType Kind => OsType.VWCI;

    public MetadataHeader Header { get; set; }
    public MetadataEntries Entries { get; set; }

    public CastMemberMetadata(ref ShockwaveReader input, ReaderContext context)
    {
        Header = new MetadataHeader(ref input, context);
        Entries = new MetadataEntries(ref input, context);
    }

    public sealed class MetadataHeader
    {
        public CastMemberInfoFlags Flags { get; set; }

        /// <summary>
        /// The script number of the Lingo script for this cast member in
        /// the cast library’s Lingo environment.
        /// </summary>
        public int? ScriptContextNum { get; set; }

        public MetadataHeader(ref ShockwaveReader input, ReaderContext context)
        {
            int headerSize = input.ReadInt32BigEndian();
            Debug.Assert(headerSize == 16 || headerSize == 20);

            int scriptGarbagePtr = input.ReadInt32BigEndian();
            int legacyFlags = input.ReadInt32BigEndian();
            Flags = (CastMemberInfoFlags)input.ReadInt32BigEndian();
            if (headerSize >= 20)
            {
                ScriptContextNum = input.ReadInt32BigEndian();
            }
        }
    }

    public sealed class MetadataEntries
    {
        public MetadataEntries(ref ShockwaveReader input, ReaderContext context)
        {
            int[] propertyOffsets = new int[input.ReadInt16BigEndian() + 1];
            for (int i = 0; i < propertyOffsets.Length; i++)
            {
                propertyOffsets[i] = input.ReadInt32BigEndian();
            }

            // TODO: Serialize the values
        }

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
                case 5: // TODO: script rel? - string - prop 44
                    break;
                case 7: // TODO: prop 45 - string
                    string prop45 = input.ReadString(length);
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
                    //TODO: MoaRect - TLBR
                    RegistrationPoints = new int[length / 4];
                    for (int i = 0; i < RegistrationPoints.Length; i++)
                    {
                        RegistrationPoints[i] = input.ReadInt32BigEndian();
                    }
                    break;
                //15 - MoA ID?
                case 16:
                    ClipboardFormat = input.ReadString(length);
                    break;
                case 17:
                    CreationDate = input.ReadInt32BigEndian() * 1000;
                    break;
                case 18:
                    ModifiedDate = input.ReadInt32BigEndian() * 1000;
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
                default:
                    ReadOnlySpan<byte> unknown = input.ReadBytes(length);
                    break;
            }
        }
    }
    
    public int GetBodySize(WriterOptions options)
    {
        throw new NotImplementedException();
    }
    public void WriteTo(ShockwaveWriter output, WriterOptions options)
    {
        throw new NotImplementedException();
    }
}
