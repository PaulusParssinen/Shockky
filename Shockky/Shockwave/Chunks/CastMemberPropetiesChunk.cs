using System;
using System.Drawing;

using Shockky.IO;

namespace Shockky.Shockwave.Chunks
{
    public class CastMemberPropetiesChunk : ChunkItem
    {
        public CastType Type { get; set; }

        public CastMemberPropetiesChunk(ShockwaveReader input, ChunkHeader header)
            : base(header)
        {
            Type = (CastType)input.ReadBigEndian<int>();
            int commonCastDataLength = input.ReadBigEndian<int>();
            int dataLength = input.ReadBigEndian<int>();
            
            byte[] commonCastData = input.ReadBytes(commonCastDataLength); //TODO:

            switch (Type)
            {
                case CastType.OLE:
                case CastType.Bitmap:
                    ushort fileSizeWhatTheF = input.ReadBigEndian<ushort>();
                    Rectangle bitmpaRect = input.ReadRect();
                    byte alphaThreshold = input.ReadByte();
                    //OLE?
                    input.Position += 7;
                    short x1 = input.ReadBigEndian<short>();
                    short y1 = input.ReadBigEndian<short>();

                    byte bitmapFlags = input.ReadByte();
                    byte depth = input.ReadByte();
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
