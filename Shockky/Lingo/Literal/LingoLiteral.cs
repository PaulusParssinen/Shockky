using Shockky.IO;
using System;

namespace Shockky.Lingo
{
    public class LingoLiteral : ShockwaveItem, IEquatable<LingoLiteral>
    {
        protected override string DebuggerDisplay => $"Offset: {Offset} | [{Enum.GetName(typeof(LiteralKind), Kind)}] {Value ?? "NULL"}";

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

                switch (Kind)
                {
                    case LiteralKind.String:
                        Value = input.ReadString(length - 1);
                        input.ReadByte();
                        break;
                    case LiteralKind.Float:
                        Value = input.ReadBigEndian<long>();
                        break;
                    case LiteralKind.CompiledJavascript:
                        Value = input.ReadBytes(length);
                        break;
                }
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