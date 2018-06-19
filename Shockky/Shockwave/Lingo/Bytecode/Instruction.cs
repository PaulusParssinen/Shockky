using System;
using System.Diagnostics;
using Shockky.IO;
using Shockky.Shockwave.Lingo.Bytecode.Instructions;
using Shockky.Shockwave.Lingo.Bytecode.Instructions.Enum;

namespace Shockky.Shockwave.Lingo.Bytecode
{
    [DebuggerDisplay("{OP}")]
    public abstract class Instruction : ShockwaveItem
    {
        public OPCode OP { get; }
        public virtual int Value { get; set; }

        protected LingoHandler Handler { get; }
        protected LingoValuePool Pool => Handler.GetScript().Pool;

        protected Instruction(OPCode op)
        {
            OP = op;
        }
        protected Instruction(OPCode op, LingoHandler handler)
            : this(op)
        {
            Handler = handler;
        }
        protected Instruction(OPCode op, LingoHandler handler, ShockwaveReader input, byte opByte)
            : this(op, handler)
        {
            if (opByte > 0x80)
                Value = input.ReadBigEndian<short>();
            else Value = input.ReadByte();
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
            byte opByte = input.ReadByte();

            var op = (OPCode)(opByte > 0x40 ?
                (opByte % 0x40 + 0x40) : opByte); //gayyyyyy y y

            switch (op)
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
                    break;
                case OPCode.StopObject:
                    break;
                case OPCode.WrapList:
                    return new WrapListIns();
                case OPCode.NewPropList:
                    return new NewPropListIns();

                //Multi 

                case OPCode.PushInt:
                    return new PushIntIns(handler, input, opByte);
                case OPCode.NewArgList:
                    return new NewListIns(handler, input, opByte, true); //unparanthesized
                case OPCode.NewList:
                    return new NewListIns(handler, input, opByte, false); //in paranthesized call expression
                case OPCode.PushConstant:
                    return new PushConstantIns(handler, input, opByte);
                case OPCode.PushSymbol:
                    return new PushSymbolIns(handler, input, opByte);
                case OPCode.GetGlobal:
                    return new GetGlobalIns(handler, input, opByte);
                case OPCode.GetProperty:
                    return new GetPropertyIns(handler, input, opByte);
                case OPCode.GetParameter:
                    return new GetParameterIns(handler, input, opByte);
                case OPCode.GetLocal:
                    return new GetLocalIns(handler, input, opByte);
                case OPCode.SetGlobal:
                    return new SetGlobalIns(handler, input, opByte);
                case OPCode.SetProperty:
                    return new SetPropertyIns(handler, input, opByte);
                case OPCode.SetParameter:
                    return new SetParameterIns(handler, input, opByte);
                case OPCode.SetLocal:
                    return new SetLocalIns(handler, input, opByte);
                case OPCode.Jump:
                    return new JumpIns(handler, input, opByte);
                case OPCode.EndRepeat:
                    return new EndRepeatIns(handler, input, opByte);
                case OPCode.IfTrue:
                    return new IfTrueIns(handler, input, opByte);
                case OPCode.CallLocal:
                    return new CallLocalIns(handler, input, opByte);
                case OPCode.CallExternal:
                    return new CallExternalIns(handler, input, opByte);
                case OPCode.CallObjOld:
                    break;
                case OPCode.Op_59:
                case OPCode.Op_5a:
                case OPCode.Op_5b:
                    break;
                case OPCode.Get:
                    return new GetIns(handler, input, opByte);
                case OPCode.Set:
                    return new SetIns(handler, input, opByte);
                case OPCode.GetMovieProp:
                    return new GetMoviePropertyIns(handler, input, opByte);
                case OPCode.SetMovieProp:
                    return new SetMoviePropertryIns(handler, input, opByte);
                case OPCode.GetObjProp:
                    return new GetObjPropertyIns(handler, input, opByte);
                case OPCode.SetObjProp:
                    return new SetObjPropertyIns(handler, input, opByte);
                case OPCode.Op_63:
                case OPCode.DubMAYBE:
                case OPCode.PopMAYBE: //TODO: Research whether this is true
                    break;
                case OPCode.GetMovieInfo:
                    return new GetMovieInfoIns(handler, input, opByte);
                case OPCode.CallObj:
                    return new CallObjectIns(handler, input, opByte);
                case OPCode.PushInt2:
                    return new PushIntIns(handler, input, opByte);
            }

            var operand = opByte > 0x80 ? input.ReadBigEndian<short>() : input.ReadByte();
            Debug.WriteLine("Unhandled instruction: " + op);

            return null;
        }
    }
}