using System;
using System.Diagnostics;
using Shockky.IO;
using Shockky.Lingo.Bytecode.Instructions;

namespace Shockky.Lingo.Bytecode
{
    public abstract class Instruction : ShockwaveItem, ICloneable
    {
        protected override string DebuggerDisplay => OP.ToString() + ((byte)OP > 0x40 ? $" {Value}" : string.Empty); //Operand for debugging purposes, gonna be removed later

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
            //eww not happy with this at all

            byte op = (byte)OP;

            if (Value > byte.MaxValue)
                op += 0x40;

            output.Write(op);

            if (op > 0x80) output.WriteBigEndian((short)Value);
            else if(op > 0x40) output.Write((byte)Value);
        }

        public virtual void Execute(LingoMachine machine)
        { }

        public virtual int GetPopCount() => 0;
        public virtual int GetPushCount() => 0;

        public override int GetBodySize() => throw new NotImplementedException();

        public static Instruction Create(LingoHandler handler, ShockwaveReader input)
        {
            byte op = input.ReadByte();
            int operandValue = 0;

            if (op > 0x40)
            {
                op = (byte)(op % 0x40 + 0x40);
                if (op > 0xC0) throw new Exception("wowowo");

                operandValue = op > 0x80 ? input.ReadBigEndian<short>() : input.ReadByte();
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
                case OPCode.Contains0String:
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
                case OPCode.StartObject:
                    return new StartObjectIns();
                case OPCode.StopObject:
                    return new StopObjectIns();
                case OPCode.WrapList:
                    return new WrapListIns();
                case OPCode.NewPropList:
                    return new NewPropListIns();

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
                    return new PushIntIns(handler, operandValue);
            }

            Debug.WriteLine($"UNK {op:X}");
            return null;
        }

        object ICloneable.Clone()
        {
            return Clone();
        }
        public Instruction Clone()
        {
            return (Instruction)MemberwiseClone();
        }
    }
}