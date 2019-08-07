using System;
using System.Diagnostics;

using Shockky.IO;

namespace Shockky.Lingo.Instructions
{
    public abstract class Instruction : ShockwaveItem, ICloneable
    {
        protected override string DebuggerDisplay 
            => OP.ToString() + ((byte)OP > 0x40 ? " " + Value : string.Empty); //Operand for debugging purposes, gonna be removed later

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

            if (op > 0xC0) output.WriteBigEndian(Value);
            else if (op > 0x80) output.WriteBigEndian((short)Value);
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

        public static Instruction Create(LingoHandler handler, ShockwaveReader input)
        {
            int operandValue = 0;

            byte op = input.ReadByte();
            byte ogOp = op;

            if (op > 0x40)
            {
                op %= 0x40;
                op += 0x40;

                if (ogOp > 0xC0) operandValue = input.ReadBigEndian<int>();
                else if (ogOp > 0x80) operandValue = input.ReadBigEndian<short>();
                else operandValue = input.ReadByte();
            }

            switch ((OPCode)op)
            {
                case OPCode.Return:
                    return new ReturnIns();
                case OPCode.PushInt0:
                    return new PushZeroIns();

                #region Arithmetic
                case OPCode.Multiple:
                    return new MultipleIns();
                case OPCode.Add:
                    return new AddIns();
                case OPCode.Substract:
                    return new SubtractIns();
                case OPCode.Divide:
                    return new DivideIns();
                case OPCode.Modulo:
                    return new ModuloIns();
                case OPCode.Inverse:
                    return new InverseIns();
                #endregion

                case OPCode.JoinString:
                    return new JoinStringIns(); 
                case OPCode.JoinPadString:
                    return new JoinPadStringIns();
                case OPCode.LessThan:
                    return new LessThanIns();
                case OPCode.LessThanEquals:
                    return new LessEqualsIns();
                case OPCode.NotEqual:
                    return new NotEqualIns();
                case OPCode.Equals:
                    return new EqualsIns();
                case OPCode.GreaterThan:
                    return new GreaterThanIns();
                case OPCode.GreaterEquals:
                    return new GreaterEqualsIns();
                case OPCode.And:
                    return new AndIns();
                case OPCode.Or:
                    return new OrIns();
                case OPCode.Not:
                    return new NotIns();
                case OPCode.ContainsString:
                    return new ContainsStringIns();
                case OPCode.StartsWith:
                    return new StartsWithIns();
                case OPCode.SplitString:
                    return new SplitStringIns();
                case OPCode.LightString:
                    break;
                case OPCode.OnToSprite:
                    break;
                case OPCode.IntoSprite:
                    break;
                case OPCode.CastString:
                    return new CastStringIns();
                case OPCode.StartObject: //PushScopeIns
                    return new StartObjectIns();
                case OPCode.StopObject: //PopScopeIns
                    return new StopObjectIns();
                case OPCode.WrapList:
                    return new WrapListIns();
                case OPCode.NewPropList:
                    return new NewPropListIns();
                case OPCode.Swap:
                    return new SwapIns();

                //Multi 

                case OPCode.PushInt:
                    return new PushIntIns(handler, operandValue);
                case OPCode.NewArgList:
                    return new NewListIns(handler, operandValue, true); //unparanthesized
                case OPCode.NewList:
                    return new NewListIns(handler, operandValue, false); //in paranthesized call expression
                case OPCode.PushConstant:
                    return new PushConstantIns(handler, operandValue);
                case OPCode.PushSymbol:
                    return new PushSymbolIns(handler, operandValue);
                case OPCode.PushObject:
                    return new PushObjectIns(handler, operandValue);
                case OPCode.Op_47:
                case OPCode.Op_48:
                    break;
                case OPCode.GetGlobal:
                    return new GetGlobalIns(handler, operandValue);
                case OPCode.GetProperty:
                    return new GetPropertyIns(handler, operandValue);
                case OPCode.GetParameter:
                    return new GetParameterIns(handler, operandValue);
                case OPCode.GetLocal:
                    return new GetLocalIns(handler, operandValue);
                case OPCode.SetGlobal:
                    return new SetGlobalIns(handler, operandValue);
                case OPCode.SetProperty:
                    return new SetPropertyIns(handler, operandValue);
                case OPCode.SetParameter:
                    return new SetParameterIns(handler, operandValue);
                case OPCode.SetLocal:
                    return new SetLocalIns(handler, operandValue);
                case OPCode.Jump:
                    return new JumpIns(handler, operandValue);
                case OPCode.EndRepeat:
                    return new EndRepeatIns(handler, operandValue);
                case OPCode.IfTrue:
                    return new IfTrueIns(handler, operandValue);
                case OPCode.CallLocal:
                    return new CallLocalIns(handler, operandValue);
                case OPCode.CallExternal:
                    return new CallExternalIns(handler, operandValue);
                case OPCode.CallObjOld:
                    break;
                case OPCode.Op_59:
                case OPCode.Op_5a:
                case OPCode.Op_5b:
                    break;
                case OPCode.Get:
                    return new GetIns(handler, operandValue);
                case OPCode.Set:
                    return new SetIns(handler, operandValue);
                case OPCode.GetMovieProp:
                    return new GetMoviePropertyIns(handler, operandValue);
                case OPCode.SetMovieProp:
                    return new SetMoviePropertryIns(handler, operandValue);
                case OPCode.GetObjProp:
                    return new GetObjPropertyIns(handler, operandValue);
                case OPCode.SetObjProp:
                    return new SetObjPropertyIns(handler, operandValue);
                case OPCode.Op_63:
                    break;
                case OPCode.Dup:
                    return new DupIns(operandValue);
                case OPCode.Pop:
                    return new PopIns(operandValue);
                case OPCode.GetMovieInfo:
                    return new GetMovieInfoIns(handler, operandValue);
                case OPCode.CallObj:
                    return new CallObjectIns(handler, operandValue);
                case OPCode.PushInt2:
                case OPCode.PushInt3:
                    return new PushIntIns(handler, operandValue);
                case OPCode.GetSpecial:
                    string specialField = handler.Script.Pool.GetName(operandValue);
                    break;
                case OPCode.PushFloat:
                    return new PushFloat(handler, operandValue);
                case OPCode.Op_72:
                    //Operand points to names prefixed by "_", is this again some special movie property get? 
                    //TODO: inspect stack at this OP. 
                    string _prefixed = handler.Script.Pool.GetName(operandValue); //Occurred values: _movie, _global, _system
                    break;
            }

            //Debug.WriteLine($"{ogOp:X2}|{Enum.GetName(typeof(OPCode), (OPCode)op)} {(op > 0x40 ? operandValue.ToString() : string.Empty)}");
            return null;
        }

        object ICloneable.Clone() => Clone();
        public Instruction Clone() => (Instruction)MemberwiseClone();
    }
}