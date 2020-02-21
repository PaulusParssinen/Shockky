using System;

using Shockky.IO;

namespace Shockky.Lingo.Instructions
{
    public abstract class Primitive : Instruction
    {
        private object _value;
        new public object Value
        {
            get => _value;
            set
            {
                _value = value;
                base.Value = SetValue(value);
            }
        }

        protected Primitive(OPCode op)
            : base(op)
        { }

        protected Primitive(OPCode op, LingoHandler handler)
            : base(op, handler)
        { }

        public override int GetPushCount() => 1;

        public override void WriteTo(ShockwaveWriter output)
        {
            base.WriteTo(output);
        }

        public override void Execute(LingoMachine machine)
        {
            machine.Values.Push(Value);
        }

        protected abstract int SetValue(object value);

        public static bool IsValid(OPCode op)
        {
            switch (op)
            {
                case OPCode.PushInt0:
                case OPCode.PushInt:
                case OPCode.PushInt2:
                case OPCode.PushFloat:
                case OPCode.PushConstant:
                //case OPCode.PushSymbol: //TODO:
                    return true;
                default:
                    return false;
            }
        }
        public static Primitive Create(LingoHandler handler, object value)
        {
            return Type.GetTypeCode(value.GetType()) switch
            {
                TypeCode.Byte => new PushIntIns(handler, (byte)value),
                TypeCode.Int16 => new PushIntIns(handler, (short)value),
                TypeCode.Int32 => new PushConstantIns(handler, (int)value),
                TypeCode.Int64 => new PushFloatIns(handler, (float)value),

                TypeCode.String => new PushConstantIns(handler, (string)value),
                //case TypeCode.Double => return new PushConstantIns(abc, (double)value),
               
                // case TypeCode.Empty => new PushNullIns(),
                _ => null
            };
        }

        public override void AcceptVisitor(InstructionVisitor visitor)
        {
            visitor.VisitPrimitiveInstruction(this);
        }
        public override void AcceptVisitor<TContext>(InstructionVisitor<TContext> visitor, TContext context)
        {
            visitor.VisitPrimitiveInstruction(this, context);
        }
        public override T AcceptVisitor<TContext, T>(InstructionVisitor<TContext, T> visitor, TContext context)
        {
            return visitor.VisitPrimitiveInstruction(this, context);
        }
    }
}