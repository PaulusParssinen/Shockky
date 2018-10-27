using Shockky.IO;

namespace Shockky.Shockwave.Lingo
{
    public class LingoHandlerBody : ShockwaveItem
    {
        public LingoHandler Handler { get; set; }
        
        public int StackHeight { get; set; }
        public byte[] Code { get; set; }

        public LingoHandlerBody(LingoHandler handler)
        {
            Handler = handler;
        }
        public LingoHandlerBody(LingoHandler handler, ShockwaveReader input)
            : this(handler)
        {
            StackHeight = input.ReadBigEndian<int>();
        }

        public LingoCode ParseCode() => new LingoCode(this);

        public override int GetBodySize() => Code.Length;

        public override void WriteTo(ShockwaveWriter output)
        {
            output.Write(Code);
        }
    }
}
