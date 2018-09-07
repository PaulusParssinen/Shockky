using System;

using Shockky.IO;

namespace Shockky.Shockwave.Lingo
{
    public class LingoHandlerBody : ShockwaveItem
    {
        public LingoHandler Handler { get; set; }
        
        public int CodeLength { get; set; }
        public int CodeOffset { get; set; }

        public int StackHeight { get; set; }
        public byte[] Code { get; set; }

        public LingoHandlerBody(LingoHandler handler)
        {
            Handler = handler;
        }

        public LingoHandlerBody(LingoHandler handler, ShockwaveReader input)
            : this(handler)
        {
            CodeLength = input.ReadBigEndian<int>();
            CodeOffset = input.ReadBigEndian<int>();

            long pos = input.Position;

            input.Position = handler.GetScript().Header.Offset + CodeOffset;
            Code = input.ReadBytes(CodeLength);

            input.Position = pos;
        }

        public LingoCode ParseCode() => new LingoCode(this);

        public override int GetBodySize() => Code.Length;

        public override void WriteTo(ShockwaveWriter output)
        {
            throw new NotImplementedException();
            output.WriteBigEndian(CodeLength);
            output.WriteBigEndian(CodeOffset);
        }
    }
}
