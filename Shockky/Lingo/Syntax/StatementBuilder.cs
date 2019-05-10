using System;
using System.Linq;
using System.Collections.Generic;

using Shockky.Lingo.Instructions;

namespace Shockky.Lingo.Syntax
{
    public class StatementBuilder : InstructionVisitor<Stack<Expression>, Statement>
    {
        private readonly ExpressionBuilder _expressionBuilder;

        private readonly Dictionary<IfTrueIns, List<Instruction>> _ifTrueBlocks;
        private readonly Dictionary<IfTrueIns, Instruction[]> _elseBlocks;

        public StatementBuilder(LingoCode code)
        {
            _expressionBuilder = new ExpressionBuilder(this);

            _ifTrueBlocks = new Dictionary<IfTrueIns, List<Instruction>>(code.JumpExits.Count);
            _elseBlocks = new Dictionary<IfTrueIns, Instruction[]>(code.JumpExits.Count);

            foreach (Jumper jumper in code.JumpExits.Keys)
            {
                if (jumper.OP != OPCode.IfTrue) continue;

                var block = new List<Instruction>(code.GetJumpBlock(jumper));
                if (block.Last() is Jumper jump)
                {
                    block.Remove(jump);
                    _elseBlocks.Add((IfTrueIns)jumper, code.GetJumpBlock(jump));
                }

                _ifTrueBlocks.Add((IfTrueIns)jumper, block);
            }
        }

        public BlockStatement ConvertBlock(IEnumerable<Instruction> instructions)
        {
            var statements = new List<Statement>();
            var stack = new Stack<Expression>();

            foreach (var instruction in instructions)
            {
                if (instruction == null)
                {
                    Console.WriteLine($"Unimplemented instruction! Stack: {stack.Count}");
                    continue;
                }

                Statement stmt = instruction.AcceptVisitor(this, stack);

                if (stmt != null)
                    statements.Add(stmt);
            }

            return new BlockStatement(statements);
        }

        public CallExpression CreateCall(Call call, Stack<Expression> expressionStack)
        {
            Expression expression = expressionStack.Pop();
            
            if (!(expression is IArgumentList argumentList))
                throw new Exception();

            IList<Expression> arguments = argumentList.Items;
            Expression targetExpression = new IdentifierExpression(call.TargetFunction);

            if (call.IsObjectCall)
            {
                Expression targetObj = arguments[0];
                arguments.RemoveAt(0);

                targetExpression = new MemberReferenceExpression(targetObj, (IdentifierExpression)targetExpression);
            }

            return new CallExpression(targetExpression, arguments);
        }

        public override Statement VisitCallInstruction(Call call, Stack<Expression> expressionStack)
        {
            Expression expression = expressionStack.Peek();

            CallExpression callExpression = CreateCall(call, expressionStack);
            if (expression is ListExpression)
            {
                expressionStack.Push(callExpression);
                return default;
            }
            return new ExpressionStatement(callExpression);
        }

        public override Statement VisitVariableAssignmentInstruction(VariableAssignment variableAssignment, Stack<Expression> expressionStack)
        {
            Expression initializer = expressionStack.Pop();
            Expression targetExpression = new IdentifierExpression(variableAssignment.Name);

            if (variableAssignment.IsObjectReference)
            {
                Expression objectExpression = expressionStack.Pop();
                targetExpression = new MemberReferenceExpression(objectExpression, (IdentifierExpression)targetExpression);
            }

            return new AssignmentStatement
            {
                Target = targetExpression,
                Initializer = initializer
            };
        }
        
        public override Statement VisitIfTrueInstruction(IfTrueIns ifTrue, Stack<Expression> expressionStack)
        {
            if (!_ifTrueBlocks.TryGetValue(ifTrue, out var trueBlock))
                throw new Exception();

            var ifStatement = new IfStatement
            {
                Condition = expressionStack.Pop(), //TODO: Validate more <^v
                IfBlock = ConvertBlock(trueBlock)
            };

            if (_elseBlocks.TryGetValue(ifTrue, out var elseBlock))
                ifStatement.ElseBlock = ConvertBlock(elseBlock);

            return ifStatement;
        }

        public override Statement VisitReturnInstruction(ReturnIns @return, Stack<Expression> expressionStack)
        {
            return new ExitStatement();
        }

        protected override Statement Default(Instruction instruction, Stack<Expression> expressionStack)
        {
            instruction.AcceptVisitor(_expressionBuilder, expressionStack);
            return default;
        }
    }
}
