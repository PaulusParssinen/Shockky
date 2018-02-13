using System;
using System.Collections.Generic;
using Shockky.IO;
using Shockky.Shockwave.Lingo.AST.Statements;
using Shockky.Shockwave.Lingo.Bytecode;
using Shockky.Shockwave.Lingo.Bytecode.Instructions.Enum;

namespace Shockky.Shockwave.Lingo.AST
{
	public class AstBuilder //TODO: Maybeh rename cause most of this stuff is specificially for handlers
	{
	    private Stack<Expression> _stack;

		public BlockStatement CreateHandlerBody(LingoHandler handler, ShockwaveReader input)
        {
            var handlerBody = new BlockStatement();
            _stack = new Stack<Expression>();

            input.Position = handler.CodeOffset;

			while (input.Position < handler.CodeOffset + handler.CodeLength)
			{
				var ins = new Instruction(input);
				var node = TransformInstruction(ins);

                handlerBody.AddChild(node);
			}

		    return handlerBody;
		}

		public AstNode TransformInstruction(Instruction instruction)
		{
		    AstNode CreateBinaryOperation(Instruction ins)
		    {
		        return null;
		    }

			switch (instruction.OP)
			{
				case OPCode.Return: return new ExitStatement();
				case OPCode.PushInt0:
					break;
				case OPCode.Multiple:
					break;
				case OPCode.Add:
					break;
				case OPCode.Substract:
					break;
				case OPCode.Divide:
					break;
				case OPCode.Modulo:
					break;
				case OPCode.Inverse:
					break;
				case OPCode.JoinString:
					break;
				case OPCode.JoinPadString:
					break;
				case OPCode.LessThan:
					break;
				case OPCode.LessThanEquals:
					break;
				case OPCode.NotEqual:
					break;
				case OPCode.Equals:
					break;
				case OPCode.GreaterThan:
					break;
				case OPCode.GreaterEquals:
					break;
				case OPCode.And:
					break;
				case OPCode.Or:
					break;
				case OPCode.Not:
					break;
				case OPCode.ContainsString:
					break;
				case OPCode.Contains0String:
					break;
				case OPCode.SplitString:
					break;
				case OPCode.LightString:
					break;
				case OPCode.OnToSprite:
					break;
				case OPCode.IntoSprite:
					break;
				case OPCode.CastString:
					break;
				case OPCode.StartObject:
					break;
				case OPCode.StopObject:
					break;
				case OPCode.WrapList:
					break;
				case OPCode.NewPropList:
					break;
				case OPCode.Swap:
					break;
				case OPCode.PushInt:
					break;
				case OPCode.NewArgList:
					break;
				case OPCode.NewList:
					break;
				case OPCode.PushConstant:
					break;
				case OPCode.PushSymbol:
					break;
				case OPCode.GetGlobal:
					break;
				case OPCode.GetProperty:
					break;
				case OPCode.GetParameter:
					break;
				case OPCode.GetLocal:
					break;
				case OPCode.SetGlobal:
					break;
				case OPCode.SetProperty:
					break;
				case OPCode.SetParameter:
					break;
				case OPCode.SetLocal:
					break;
				case OPCode.Jump:
					break;
				case OPCode.EndRepeat:
					break;
				case OPCode.IfTrue:
					break;
				case OPCode.CallLocal:
					break;
				case OPCode.CallExternal:
					break;
				case OPCode.CallObjOld:
					break;
				case OPCode.Op_59:
					break;
				case OPCode.Op_5a:
					break;
				case OPCode.Op_5b:
					break;
				case OPCode.Get:
					break;
				case OPCode.Set:
					break;
				case OPCode.GetMovieProp:
					break;
				case OPCode.SetMovieProp:
					break;
				case OPCode.GetObjProp:
					break;
				case OPCode.SetObjProp:
					break;
				case OPCode.GetMovieInfo:
					break;
				case OPCode.CallObj:
					break;
				case OPCode.PushInt2:
					break;
			}

            throw new NotImplementedException("Not implemented OP found: " + instruction.OP);
		}
	}
}
