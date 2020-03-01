using System;
using Shockky.IO;

namespace Shockky.Lingo
{
    public class LingoLiteral : ShockwaveItem, IEquatable<LingoLiteral>
    {
        protected override string DebuggerDisplay => $"[{Kind}] Value: {Value}, Offset: {Offset}";

        public LiteralKind Kind { get; set; }
        public long Offset { get; set; }
        public object Value { get; set; }

        public LingoLiteral(LiteralKind kind, object value)
        {
            Kind = kind;
            Value = value;
        }

        public LingoLiteral(ShockwaveReader input)
        {
            Kind = (LiteralKind)input.ReadBigEndian<int>();
            Offset = input.ReadBigEndian<int>();
        }

        public void ReadValue(ShockwaveReader input, long dataOffset)
        {
            if (Kind != LiteralKind.Integer) 
            {
                input.Position = dataOffset + Offset;

                int length = input.ReadBigEndian<int>();
                Value = Kind switch
                {
                    LiteralKind.String => input.ReadString(length),
                    LiteralKind.Float => input.ReadBigEndian<long>(),
                    LiteralKind.CompiledJavascript => input.ReadBytes(length)
                };
            }
            else Value = Offset;
        }

        public override int GetBodySize()
        {
            int size = 0;
            size += sizeof(int);
            size += sizeof(int);
            return size;
        }

        public override void WriteTo(ShockwaveWriter output)
        {
            output.WriteBigEndian((int)Kind);
            output.WriteBigEndian(Offset);
        }

        public bool Equals(LingoLiteral literal)
            => (literal.Kind == Kind && literal.Value == Value); 
    }
}