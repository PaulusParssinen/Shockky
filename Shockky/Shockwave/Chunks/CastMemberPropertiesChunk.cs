using System;
using System.Drawing;

using Shockky.IO;
using Shockky.Shockwave.Chunks.Cast;

namespace Shockky.Shockwave.Chunks
{
    public class CastMemberPropertiesChunk : ChunkItem
    {
        public CastType Type { get; set; }

        public CommonMemberProperties Common { get; set; }

        public CastMemberPropertiesChunk(ShockwaveReader input, ChunkHeader header)
            : base(header)
        {
            Type = (CastType)input.ReadBigEndian<int>();
            int commonCastDataLength = input.ReadBigEndian<int>();
            int dataLength = input.ReadBigEndian<int>();

            long commonDataStart = input.Position;

            Remnants.Enqueue(input.ReadBigEndian<int>());
            Remnants.Enqueue(input.ReadBigEndian<int>());
            Remnants.Enqueue(input.ReadBigEndian<int>());
            Remnants.Enqueue(input.ReadBigEndian<int>());
            Remnants.Enqueue(input.ReadBigEndian<int>());

            Common = new CommonMemberProperties(input);

            ushort fileSizeWhatTheF;
            byte bitmapFlags;
            byte depth;

            switch (Type)
            {
                case CastType.OLE:
                case CastType.Bitmap:
                    fileSizeWhatTheF = input.ReadBigEndian<ushort>(); //4 if depth not intact
                    Rectangle bitmpaRect = input.ReadRect();
                    byte alphaThreshold = input.ReadByte();
                    //OLE?
                    input.Position += 7;
                    short x1 = input.ReadBigEndian<short>();
                    short y1 = input.ReadBigEndian<short>();

                    bitmapFlags = input.ReadByte();

                    //Rest of this depends on fileSizeWhatTheF?
                    bool IsDataAvailable() => (Header.Offset + Header.Length > input.Position);

                    if (!IsDataAvailable()) break;

                    depth = input.ReadByte();

                    if (!IsDataAvailable()) break;

                    int palette = input.ReadBigEndian<int>(); //??
                    break;
                case CastType.Shape:
                    short shapeType = input.ReadBigEndian<short>();
                    Rectangle shapeRect = input.ReadRect();

                    short pattern = input.ReadBigEndian<short>();
                    input.Position += 2;
                    byte shapeFlags = input.ReadByte();

                    byte lineSize = input.ReadByte(); //-1
                    byte lineDir = input.ReadByte(); // -5
                    break;
                case CastType.Xtra:
                case CastType.Movie:
                case CastType.DigitalVideo:
                    uint type = input.ReadBigEndian<uint>();
                    input.Position += 10;
                    byte videoFlags = input.ReadByte();
                    videoFlags = input.ReadByte();
                    videoFlags = input.ReadByte();
                    input.Position += 3;
                    byte frameRate = input.ReadByte();
                    input.Position += 32;
                    Rectangle videoRectangle = input.ReadRect();
                    break;
                case CastType.Text:
                case CastType.Button:
                    input.Position += 4;

                    short alignment = input.ReadBigEndian<short>();
                    byte[] bgColor = input.ReadBytes(3);

                    short font = input.ReadBigEndian<short>();
                    Rectangle textRect = input.ReadRect();
                    short lineHeight = input.ReadBigEndian<short>();

                    input.Position += 4;

                    short buttonType = input.ReadBigEndian<short>();
                    break;
                case CastType.Script:
                    short scriptType = input.ReadBigEndian<short>();
                    break;

                case CastType.Transition:
                case CastType.StyledText:
                case CastType.Picture:
                case CastType.FilmLoop:
                case CastType.Sound:
                case CastType.Palette:
                default:
                    Remnants.Enqueue(input.ReadBytes(dataLength));
                    break;
            }

            long dataLeft = Header.Offset + Header.Length - input.Position;
            byte[] restOfData = input.ReadBytes((int)dataLeft);
        }

        public override int GetBodySize()
        {
            throw new NotImplementedException();
            int size = 0;
            size += sizeof(int);
            size += sizeof(int);
            size += sizeof(int);
			return size;
        }

        public override void WriteBodyTo(ShockwaveWriter output)
        {
            throw new System.NotImplementedException();
        }
    }
}
