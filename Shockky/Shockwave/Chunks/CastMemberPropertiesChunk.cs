using System;

using Shockky.IO;
using Shockky.Shockwave.Chunks.Cast;

namespace Shockky.Shockwave.Chunks
{
    public class CastMemberPropertiesChunk : ChunkItem
    {
        public CastType Type { get; set; }
        public ICastTypeProperties Properties { get; set; }

        public CommonMemberProperties Common { get; set; }

        public CastMemberPropertiesChunk(ShockwaveReader input, ChunkHeader header)
            : base(header)
        {
            Type = (CastType)input.ReadBigEndian<int>();
            input.ReadBigEndian<int>();
            int dataLength = input.ReadBigEndian<int>();

            Remnants.Enqueue(input.ReadBigEndian<int>());
            Remnants.Enqueue(input.ReadBigEndian<int>());
            Remnants.Enqueue(input.ReadBigEndian<int>());
            Remnants.Enqueue(input.ReadBigEndian<int>());
            Remnants.Enqueue(input.ReadBigEndian<int>());

            Common = new CommonMemberProperties(input);
            Properties = ReadTypeProperties(input, dataLength);
        }

        private ICastTypeProperties ReadTypeProperties(ShockwaveReader input, int dataLength)
        {
            switch (Type)
            {
                case CastType.Bitmap:
                case CastType.OLE:
                    return new BitmapCastProperties(this, input);
                case CastType.Shape:
                    return new ShapeCastProperties(input);
                case CastType.DigitalVideo:
                case CastType.Movie:
                case CastType.Xtra:
                    return new VideoCastProperties(input);
                case CastType.Button:
                case CastType.Text:
                    return new TextCastProperties(input);
                case CastType.Script:
                    return new ScriptCastProperties(input);

                default:
                    return new UnknownCastProperties(input, dataLength);
            }
        }

        public override int GetBodySize()
        {
            int size = 0;
            size += sizeof(int);
            size += sizeof(int);
            size += sizeof(int);

            size += sizeof(int);
            size += sizeof(int);
            size += sizeof(int);
            size += sizeof(int);
            size += sizeof(int);

            size += Common.GetBodySize(); //TODO:
            size += Properties.GetBodySize();
            return size;
        }

        public override void WriteBodyTo(ShockwaveWriter output)
        {
            output.WriteBigEndian((int)Type);
            output.WriteBigEndian(Common.GetBodySize()); //TODO:
            output.WriteBigEndian(Properties.GetBodySize());

            output.WriteBigEndian((int)Remnants.Dequeue());
            output.WriteBigEndian((int)Remnants.Dequeue());
            output.WriteBigEndian((int)Remnants.Dequeue());
            output.WriteBigEndian((int)Remnants.Dequeue());
            output.WriteBigEndian((int)Remnants.Dequeue());

            Common.WriteTo(output); //TODO:
            Properties.WriteTo(output);
        }
    }
}
