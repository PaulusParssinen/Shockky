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

        public StatementBuilder(LingoCode code)
        {
            _code = code;
            _expressionBuilder = new ExpressionBuilder(this);
        }

        public BlockStatement ConvertBlock(Instruction[] instructions, Stack<Expression> stack, out int consumed)
        {
            var statements = new List<Statement>();
            for (consumed = 0; consumed < instructions.Length; consumed++)
            {
                Instruction instruction = instructions[consumed];

                if (instruction == null)
                {
                    Console.WriteLine($"Unimplemented instruction! Stack: {stack.Count}");
                    continue;
                }

                if (Jumper.IsValid(instruction.OP))
                    throw new NotImplementedException("Control Flow handling is still under construction.");

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
                throw new InvalidOperationException(nameof(CreateCall));

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
            Expression target = new IdentifierExpression(variableAssignment.Name);

            if (variableAssignment.IsObjectReference)
            {
                Expression objectExpression = expressionStack.Pop();
                target = new MemberReferenceExpression(objectExpression, (IdentifierExpression)target);
            }

            return new AssignmentStatement
            {
                Target = target,
                Initializer = initializer
            };
        }

        public override Statement VisitInsertStringInstruction(InsertStringIns insertString, Stack<Expression> expressionStack)
        {
            Expression aa = expressionStack.Pop();
            Expression b = expressionStack.Pop();

            return null;
        }
        public override Statement VisitInsertInstruction(InsertIns insert, Stack<Expression> expressionStack)
        {
            Expression b = expressionStack.Pop();

            Expression a1 = expressionStack.Pop();
            Expression a2 = expressionStack.Pop();
            Expression a3 = expressionStack.Pop();
            Expression a4 = expressionStack.Pop(); //the itemDelimiter
            Expression a5 = expressionStack.Pop();
            Expression a6 = expressionStack.Pop();
            Expression a8 = expressionStack.Pop();
            Expression a9 = expressionStack.Pop();

            return null;
        }

        public override Statement VisitLightStringInstruction(HiliteStringIns lightString, Stack<Expression> expressionStack)
        {
            Expression field = expressionStack.Pop();

            Expression lastLine = expressionStack.Pop();
            Expression firstLine = expressionStack.Pop();
            Expression lastItem = expressionStack.Pop();
            Expression firstItem = expressionStack.Pop(); //the itemDelimiter
            Expression lastWord = expressionStack.Pop();
            Expression firstWord = expressionStack.Pop();
            Expression lastChar = expressionStack.Pop();
            Expression firstChar = expressionStack.Pop();

            return null;
        }

        public override Statement VisitIfTrueInstruction(IfTrueIns ifTrue, Stack<Expression> expressionStack)
        {
            throw new NotImplementedException("Control Flow handling is still under construction.");

            /*if (!_ifTrueBlocks.TryGetValue(ifTrue, out var trueBlock))
                throw new Exception();

            var ifStatement = new IfStatement
            {
                Condition = expressionStack.Pop(), //TODO: Validate more <^v
                IfBlock = trueBlock
            };

            if (_elseBlocks.TryGetValue(ifTrue, out var elseBlock))
                ifStatement.ElseBlock = elseBlock;
                */
            return default;//ifStatement;
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
