using System;

using Shockky.IO;

namespace Shockky.Lingo
{
    //TODO: Still not satisfied with this implementation.
    public class LingoLiteral : ShockwaveItem, IEquatable<LingoLiteral>
    {
        protected override string DebuggerDisplay => $"[{Kind}] {Value}";

        public LiteralKind Kind { get; set; }
        public object Value { get; set; }

        public LingoLiteral(LiteralKind kind, object value)
        {
            Kind = kind;
            Value = value;
        }

        public override int GetBodySize()
        {
            int size = 0;
            if (Kind != LiteralKind.Integer)
            {
                size += sizeof(int);
                size += Kind switch
                {
                    LiteralKind.String => Value.ToString().Length + 1, //TODO: wazzup with null terminator
                    LiteralKind.FloatingPoint => sizeof(double),
                    LiteralKind.CompiledJavascript => ((byte[])Value).Length,

                    _ => throw new ArgumentException(nameof(Kind))
                };
            }
            return size;
        }

        public override void WriteTo(ShockwaveWriter output)
        {
            throw new NotImplementedException();
        }

        public bool Equals(LingoLiteral literal)
            => literal.Kind == Kind && literal.Value == Value;

        public static LingoLiteral Read(ref ShockwaveReader input, LiteralKind entryKind, int entryOffset)
        {
            if (entryKind != LiteralKind.Integer)
            {
                input.Position = entryOffset;

                int length = input.ReadInt32();
                object value = entryKind switch
                {
                    LiteralKind.String => input.ReadString(length),
                    LiteralKind.FloatingPoint => input.ReadDouble(),
                    LiteralKind.CompiledJavascript => input.ReadBytes(length).ToArray(),

                    _ => throw new ArgumentException(nameof(Kind))
                };

                return new LingoLiteral(entryKind, value);
            }
            else return new LingoLiteral(LiteralKind.Integer, entryOffset);
        }
    }
}