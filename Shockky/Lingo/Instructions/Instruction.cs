using System;
using System.Diagnostics;

using Shockky.IO;

namespace Shockky.Lingo.Instructions
{
    public abstract class Instruction : ShockwaveItem, ICloneable
    {
        protected override string DebuggerDisplay 
            => OP.ToString() + ((byte)OP > 0x40 ? " " + Value : string.Empty); //Operand for debugging purposes, to be removed

        public OPCode OP { get; }
        public virtual int Value { protected get; set; }

        protected LingoHandler Handler { get; }
        protected LingoValuePool Pool => Handler.Script.Pool;

        protected Instruction(OPCode op)
        {
            OP = op;
        }
        protected Instruction(OPCode op, LingoHandler handler)
            : this(op)
        {
            Handler = handler;
        }

        public override void WriteTo(ShockwaveWriter output)
        {
            //TODO:

            byte op = (byte)OP;

            if (Value > byte.MaxValue)
                op += 0x40;

            if (Value > short.MaxValue)
                op += 0x40;

            output.Write(op);

            if (op > 0xC0) output.Write(Value);
            else if (op > 0x80) output.Write((short)Value);
            else if (op > 0x40) output.Write((byte)Value);
        }

        public virtual void Execute(LingoMachine machine)
        { }

        public virtual void AcceptVisitor(InstructionVisitor visitor)
        {
            Console.WriteLine($"{OP} has not implemented InstructionVisitor");
        }
        public virtual void AcceptVisitor<TContext>(InstructionVisitor<TContext> visitor, TContext context)
        {
            Console.WriteLine($"{OP} has not implemented InstructionVisitor");
        }
        public virtual T AcceptVisitor<TContext, T>(InstructionVisitor<TContext, T> visitor, TContext context)
        {
            Console.WriteLine($"{OP} has not implemented InstructionVisitor");
            return default;
        }

        public virtual int GetPopCount() => 0;
        public virtual int GetPushCount() => 0;

        public override int GetBodySize() => throw new NotImplementedException();

        public static Instruction Create(LingoHandler handler, ref ShockwaveReader input)
        {
            int operandValue = 0;

            byte op = input.ReadByte();
            byte ogOp = op;

            if (op > 0x40)
            {
                op %= 0x40;
                op += 0x40;

                if (ogOp > 0xC0) operandValue = input.ReadInt32();
                else if (ogOp > 0x80) operandValue = input.ReadInt16();
                else operandValue = input.ReadByte();
            }

            //Debug.WriteLine($"{ogOp:X2}|{Enum.GetName(typeof(OPCode), (OPCode)op)} {(op > 0x40 ? operandValue.ToString() : string.Empty)}");

            return (OPCode)op switch
            {
                OPCode.Return => new ReturnIns(),
                OPCode.PushInt0 => new PushZeroIns(),

                #region Arithmetic
                OPCode.Multiple => new MultipleIns(),
                OPCode.Add => new AddIns(),
                OPCode.Substract => new SubtractIns(),
                OPCode.Divide => new DivideIns(),
                OPCode.Modulo => new ModuloIns(),
                OPCode.Inverse => new InverseIns(),
                #endregion

                OPCode.JoinString => new JoinStringIns(),
                OPCode.JoinPadString => new JoinPadStringIns(),
                OPCode.LessThan => new LessThanIns(),
                OPCode.LessThanEquals => new LessEqualsIns(),
                OPCode.NotEqual => new NotEqualIns(),
                OPCode.Equals => new EqualsIns(),
                OPCode.GreaterThan => new GreaterThanIns(),
                OPCode.GreaterEquals => new GreaterEqualsIns(),
                OPCode.And => new AndIns(),
                OPCode.Or => new OrIns(),
                OPCode.Not => new NotIns(),
                OPCode.ContainsString => new ContainsStringIns(),
                OPCode.StartsWith => new StartsWithIns(),
                OPCode.SplitString => new SplitStringIns(),
                OPCode.Hilite => new HiliteStringIns(),
                OPCode.OntoSprite => new OntoSpriteIns(),
                OPCode.IntoSprite => new IntoSpriteIns(),
                OPCode.CastString => new CastStringIns(),
                OPCode.StartObject => new StartObjectIns(), //PushScopeIns
                OPCode.StopObject => new StopObjectIns(), //PopScopeIns
                OPCode.WrapList => new WrapListIns(),
                OPCode.NewPropList => new NewPropListIns(),
                OPCode.Swap => new SwapIns(),

                //Multi 
                OPCode.PushInt => new PushIntIns(handler, operandValue),
                OPCode.NewArgList => new NewListIns(handler, operandValue, true), //unparanthesized
                OPCode.NewList => new NewListIns(handler, operandValue, false), //in paranthesized call expression
                OPCode.PushConstant => new PushConstantIns(handler, operandValue),
                OPCode.PushSymbol => new PushSymbolIns(handler, operandValue),
                OPCode.PushObject => new PushObjectIns(handler, operandValue),
                //OPCode.Op_47:
                //OPCode.Op_48:
                OPCode.GetGlobal => new GetGlobalIns(handler, operandValue),
                OPCode.GetProperty => new GetPropertyIns(handler, operandValue),
                OPCode.GetParameter => new GetParameterIns(handler, operandValue),
                OPCode.GetLocal => new GetLocalIns(handler, operandValue),
                OPCode.SetGlobal => new SetGlobalIns(handler, operandValue),
                OPCode.SetProperty => new SetPropertyIns(handler, operandValue),
                OPCode.SetParameter => new SetParameterIns(handler, operandValue),
                OPCode.SetLocal => new SetLocalIns(handler, operandValue),
                OPCode.Jump => new JumpIns(handler, operandValue),
                OPCode.EndRepeat => new EndRepeatIns(handler, operandValue),
                OPCode.IfFalse => new IfFalseIns(handler, operandValue),
                OPCode.CallLocal => new CallLocalIns(handler, operandValue),
                OPCode.CallExternal => new CallExternalIns(handler, operandValue),
                //OPCode.CallObjOld:
                OPCode.InsertString => new InsertStringIns(handler, operandValue),
                OPCode.Insert => new InsertIns(handler, operandValue),
                //OPCode.Op_5b:
                OPCode.Get => new GetIns(handler, operandValue),
                OPCode.Set => new SetIns(handler, operandValue),
                OPCode.GetMovieProp => new GetMoviePropertyIns(handler, operandValue),
                OPCode.SetMovieProp => new SetMoviePropertryIns(handler, operandValue),
                OPCode.GetObjProp => new GetObjPropertyIns(handler, operandValue),
                OPCode.SetObjProp => new SetObjPropertyIns(handler, operandValue),
                //OPCode.Op_63:
                OPCode.Dup => new DupIns(operandValue),
                OPCode.Pop => new PopIns(operandValue),
                OPCode.GetMovieInfo => new GetMovieInfoIns(handler, operandValue),
                OPCode.CallObj => new CallObjectIns(handler, operandValue),
                //OPCode.Op_6d
                OPCode.PushInt2 => new PushIntIns(handler, operandValue),
                OPCode.PushInt3 => new PushIntIns(handler, operandValue),

                //OPCode.GetSpecial => null,
                //    string specialField = handler.Script.Pool.GetName(operandValue),

                OPCode.PushFloat => new PushFloatIns(handler, operandValue),
                //OPCode.Op_72:
                //Operand points to names prefixed by "_", is this another special movie property get? 
                //TODO: inspect stack at this OP. 
                //string _prefixed = handler.Script.Pool.GetName(operandValue), //Occurred values: _movie, _global, _system
                _ => null//new DummyInstruction((OPCode)op, handler, operandValue),
            };
        }

        object ICloneable.Clone() => Clone();
        public Instruction Clone() => (Instruction)MemberwiseClone();
    }
}