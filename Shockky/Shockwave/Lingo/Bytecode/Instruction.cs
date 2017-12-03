using System;
using System.Diagnostics;
using Shockky.IO;
using Shockky.Shockwave.Lingo.Bytecode.AST;
using Shockky.Shockwave.Lingo.Bytecode.Instructions;
using Shockky.Shockwave.Lingo.Bytecode.Instructions.Arithmetic;
using Shockky.Shockwave.Lingo.Bytecode.Instructions.Control_Transfer;
using Shockky.Shockwave.Lingo.Bytecode.Instructions.Enum;

namespace Shockky.Shockwave.Lingo.Bytecode
{
    [DebuggerDisplay("OPCode: {OP}")]
    public abstract class Instruction : AstNode
    {
        public OPCode OP { get; }
        public LingoHandler Handler { get; }

        public Instruction(OPCode op)
        {
            OP = op;
        }

        public Instruction(OPCode op, LingoHandler handler)
            : this(op)
        {
            Handler = handler;
        }

        public virtual int GetPopCount()
        {
            return 0;
        }
        public virtual int GetPushCount()
        {
            return 0;
        }
        public virtual bool IsStatement { get; set; }

        public virtual void Execute(LingoMachine machine)
        { }
        public virtual void Translate()
        { }

        public static Instruction Create(LingoHandler handler, ref ShockwaveReader input)
        {
            byte opByte = input.ReadByte();

            var op = (OPCode) opByte;

            if (opByte > 0x40) //TODO: Think something better here instead of this bs
                op = (OPCode)(opByte % 0x40 + 0x40);

            Debug.WriteLine("Adding instruction: " + op);
            switch (op)
            {
                case OPCode.Return:
                    return new ReturnIns(handler);
                case OPCode.PushInt0:
                    return new PushZeroIns(handler);
                case OPCode.Multiple:
                    return new MultipleIns(handler);
                case OPCode.Add:
                    return new AddIns(handler);
                case OPCode.Substract:
                    return new SubtractIns(handler);
                case OPCode.Divide:
                    return new DivideIns(handler);
                case OPCode.Modulo:
                    return new ModuloIns(handler);
                case OPCode.Inverse:
                    return new InverseIns(handler);
                case OPCode.JoinString:
                    return new JoinStringIns(handler); 
                case OPCode.JoinPadString:
                    return new JoinPadStringIns(handler);
                case OPCode.LessThan:
                    return new LessThanIns(handler);
                case OPCode.LessThanEquals:
                    return new LessEqualsIns(handler);
                case OPCode.NotEqual:
                    return new NotEqualIns(handler);
                case OPCode.Equals:
                    return new EqualsIns(handler);
                case OPCode.GreaterThan:
                    return new GreaterThanIns(handler);
                case OPCode.GreaterEquals:
                    return new GreaterEqualsIns(handler);
                case OPCode.And:
                    return new AndIns(handler);
                case OPCode.Or:
                    return new OrIns(handler);
                case OPCode.Not:
                    return new NotIns(handler);
                case OPCode.ContainsString:
                    return new ContainsStringIns(handler);
                case OPCode.Contains0String:
                    return new StartsWithIns(handler);
                case OPCode.SplitString:
                    return new SplitStringIns(handler);
                case OPCode.LightString:
                    break;
                case OPCode.OnToSprite:
                    break;
                case OPCode.IntoSprite:
                    break;
                case OPCode.CastString:
                    return new CastStringIns(handler);
                case OPCode.StartObject:
                    break;
                case OPCode.StopObject:
                    break;
                case OPCode.WrapList:
                    return new WrapListIns(handler);
                case OPCode.NewPropList:
                    return new NewPropListIns(handler);

                //Multi

                case OPCode.PushInt:
                    return new PushIntIns(input, handler, false);
                case OPCode.NewArgList:
                    return new NewListIns(input, handler); //unparanthesized
                case OPCode.NewList:
                    return new NewListIns(input, handler); //in paranthesized call expression
                case OPCode.PushConstant:
                    return new PushConstantIns(input, handler, opByte);
                case OPCode.PushSymbol:
                    return new PushSymbolIns(input, handler, opByte);
                case OPCode.GetGlobal:
                    return new GetGlobalIns(input, handler, opByte);
                case OPCode.GetProperty:
                    return new GetPropertyIns(input, handler, opByte);
                case OPCode.GetParameter:
                    return new GetParameterIns(input, handler, opByte);
                case OPCode.GetLocal:
                    return new GetLocalIns(input, handler, opByte);
                case OPCode.SetGlobal:
                    return new SetGlobalIns(input, handler, opByte);
                case OPCode.SetProperty:
                    return new SetPropertyIns(input, handler, opByte);
                case OPCode.SetParameter:
                    return new SetParameterIns(input, handler, opByte);
                case OPCode.SetLocal:
                    return new SetLocalIns(input, handler, opByte);
                case OPCode.Jump:
                    return new JumpIns(input, handler);
                case OPCode.EndRepeat:
                    return new EndRepeatIns(input, handler);
                case OPCode.IfTrue:
                    return new IfTrueIns(input, handler);
                case OPCode.CallLocal:
                    return new CallLocalIns(input, handler);
                case OPCode.CallExternal:
                    return new CallExternalIns(input, handler, opByte);
                case OPCode.CallObjOld:
                    break;
                case OPCode.Op_59:
                    Debug.WriteLine(opByte > 0x80 ? input.ReadInt16(true) : input.ReadByte());
                    return null; //new Instruction(op);
                case OPCode.Op_5a:
                    Debug.WriteLine(opByte > 0x80 ? input.ReadInt16(true) : input.ReadByte());
                    return null; //new Instruction(op);
                case OPCode.Op_5b:
                    break;
                case OPCode.Get:
                    return new GetIns(input, handler);
                case OPCode.Set:
                    return new SetIns(input, handler);
                case OPCode.GetMovieProp:
                    return new GetMoviePropertyIns(input, handler, opByte);
                case OPCode.SetMovieProp:
                    return new SetMoviePropertryIns(input, handler, opByte);
                case OPCode.GetObjProp:
                    return new GetObjPropertyIns(input, handler, opByte);
                case OPCode.SetObjProp:
                    return new SetObjPropertyIns(input, handler, opByte);
                case OPCode.GetMovieInfo:
                    return new GetMovieInfoIns(input, handler, opByte);
                case OPCode.CallObj:
                    return new CallObjectIns(input, handler, opByte);
                case OPCode.PushInt2:
                    return new PushIntIns(input, handler, true);
            }

           throw new NotImplementedException("Unhandled instruction! " + op);
        }
    }
}
