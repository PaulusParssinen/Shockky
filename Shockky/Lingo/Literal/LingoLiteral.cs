using System;
using Shockky.IO;

namespace Shockky.Lingo
{
    public class LingoLiteral : ShockwaveItem, IEquatable<LingoLiteral>
    {
        protected override string DebuggerDisplay => $"[{Kind}] Value: {Value}, Offset: {Offset}";

        public LiteralKind Kind { get; set; }
        public int Offset { get; set; }
        public object Value { get; set; }

        public LingoLiteral(LiteralKind kind, object value)
        {
            Kind = kind;
            Value = value;
        }

        public LingoLiteral(ref ShockwaveReader input)
        {
            Kind = (LiteralKind)input.ReadInt32();
            Offset = input.ReadInt32();
        }

        public void ReadValue(ref ShockwaveReader input, int dataOffset)
        {
            if (Kind != LiteralKind.Integer) 
            {
                input.Position = dataOffset + Offset;

                int length = input.ReadInt32();
                Value = Kind switch
                {
                    LiteralKind.String => input.ReadString(length),
                    LiteralKind.FloatingPoint => input.ReadDouble(),
                    LiteralKind.CompiledJavascript => input.ReadBytes(length).ToArray()
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
            output.Write((int)Kind);
            output.Write(Offset);
        }

        public bool Equals(LingoLiteral literal)
            => (literal.Kind == Kind && literal.Value == Value); 
    }
}