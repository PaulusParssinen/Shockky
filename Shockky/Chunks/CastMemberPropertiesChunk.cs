using Shockky.IO;
using Shockky.Chunks.Cast;

namespace Shockky.Chunks
{
    public class CastMemberPropertiesChunk : ChunkItem
    {
        public CastType Type { get; set; }
        public ICastProperties Properties { get; set; }

        public CommonMemberProperties Common { get; set; }

        public CastMemberPropertiesChunk()
            : base(ChunkKind.CASt)
        { }
        public CastMemberPropertiesChunk(ref ShockwaveReader input, ChunkHeader header)
            : base(header)
        {
            input.IsBigEndian = true;

            Type = (CastType)input.ReadInt32();
            input.ReadInt32();
            int dataLength = input.ReadInt32();

            Remnants.Enqueue(input.ReadInt32()); //TOOD: Why is DIRAPI checking this 24/7 if its a constant(?) 0x14
            Remnants.Enqueue(input.ReadInt32());
            Remnants.Enqueue(input.ReadInt32());
            Remnants.Enqueue(input.ReadInt32());
            Remnants.Enqueue(input.ReadInt32());

            Common = new CommonMemberProperties(ref input);
            Properties = ReadTypeProperties(ref input, dataLength);
        }

        private ICastProperties ReadTypeProperties(ref ShockwaveReader input, int dataLength)
        {
            switch (Type)
            {
                case CastType.Bitmap:
                case CastType.OLE:
                    return new BitmapCastProperties(ref input);
                case CastType.Shape:
                    return new ShapeCastProperties(ref input);
                case CastType.Movie:
                case CastType.DigitalVideo:
                    return new VideoCastProperties(ref input);
                case CastType.Button:
                case CastType.Text:
                    return new TextCastProperties(ref input);
                case CastType.Script:
                    return new ScriptCastProperties(ref input);
                default:
                    return new UnknownCastProperties(ref input, dataLength);
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
            output.Write((int)Type);
            output.Write(Common.GetBodySize()); //TODO:
            output.Write(Properties.GetBodySize());

            output.Write((int)Remnants.Dequeue());
            output.Write((int)Remnants.Dequeue());
            output.Write((int)Remnants.Dequeue());
            output.Write((int)Remnants.Dequeue());
            output.Write((int)Remnants.Dequeue());

            Common.WriteTo(output); //TODO:
            Properties.WriteTo(output);
        }
    }
}
