using System;
using Shockky.IO;

namespace Shockky.Lingo
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
        public LingoHandlerBody(LingoHandler handler, ref ShockwaveReader input)
            : this(handler)
        {
            Code = new byte[input.ReadInt32()];
        }

        public LingoCode ParseCode() => new LingoCode(this);

        public override int GetBodySize() => Code.Length;

        public override void WriteTo(ShockwaveWriter output)
            => throw new NotImplementedException();
    }
}
