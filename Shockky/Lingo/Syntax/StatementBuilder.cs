using System;
using System.Linq;
using System.Collections.Generic;

using Shockky.Lingo.Instructions;

namespace Shockky.Lingo.Syntax
{
    public class StatementBuilder : InstructionVisitor<Stack<Expression>, Statement>
    {
        private readonly LingoCode _code;
        private readonly ExpressionBuilder _expressionBuilder;

        private readonly Dictionary<IfTrueIns, BlockStatement> _ifTrueBlocks;
        private readonly Dictionary<IfTrueIns, BlockStatement> _elseBlocks;

        private int _intendation = 0;

        public StatementBuilder(LingoCode code)
        {
            _code = code;
            _expressionBuilder = new ExpressionBuilder(this);

            _ifTrueBlocks = new Dictionary<IfTrueIns, BlockStatement>(code.JumpExits.Count);
            _elseBlocks = new Dictionary<IfTrueIns, BlockStatement>(code.JumpExits.Count);
        }

        public BlockStatement ConvertBlock(Instruction[] instructions, Stack<Expression> stack, out int consumed)
        {
            var statements = new List<Statement>();
            for (consumed = 0; consumed < instructions.Length; consumed++)
            {
                Instruction instruction = instructions[consumed];

                Console.WriteLine(new string('\t', _intendation) + $"[{consumed}/{instructions.Length}] {instruction.OP}");

                if (instruction == null)
                {
                    Console.WriteLine($"Unimplemented instruction! Stack: {stack.Count}");
                    continue;
                }

                if (instruction is IfTrueIns ifTrue)
                {
                    var block = new List<Instruction>(_code.GetJumpBlock(ifTrue));
                    Console.WriteLine(new string('\t', ++_intendation) + $"# Entering conditional block. Stack: {stack.Count}");

                    _ifTrueBlocks.Add(ifTrue, ConvertBlock(block.ToArray(), stack, out int ifConsumed));
                    consumed += ifConsumed;
                    Console.WriteLine(new string('\t', --_intendation) + $"# Block exit. Stack: {stack.Count}");

                    if (block.Last() is Jumper jump)
                    {
                        block.Remove(jump);
                        var elseBlock = _code.GetJumpBlock(jump);

                        Console.WriteLine(new string('\t', _intendation++) + $"# Has else block. Entering.. Stack: {stack.Count}");

                        _elseBlocks.Add(ifTrue, ConvertBlock(elseBlock, stack, out int elseConsumed));
                        consumed += elseConsumed;

                        Console.WriteLine(new string('\t', --_intendation) + $"# Block exit. Stack: {stack.Count}");
                    }
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
                IfBlock = trueBlock
            };

            if (_elseBlocks.TryGetValue(ifTrue, out var elseBlock))
                ifStatement.ElseBlock = elseBlock;

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
