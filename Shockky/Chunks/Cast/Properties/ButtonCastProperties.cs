using Shockky.IO;

namespace Shockky.Chunks.Cast
{
    public class ButtonCastProperties : TextCastProperties, ICastProperties
    {
        public ButtonType ButtonType { get; set; }

        public ButtonCastProperties()
        { }
        public ButtonCastProperties(ref ShockwaveReader input)
            : base(ref input)
        {
            ButtonType = (ButtonType)input.ReadInt16();
        }

        public override int GetBodySize()
        {
            int size = 0;
            size += base.GetBodySize();
            size += sizeof(short);
            return size;
        }

        public override void WriteTo(ShockwaveWriter output)
        {
            base.WriteTo(output);
            output.Write((short)ButtonType);
        }
    }
}
