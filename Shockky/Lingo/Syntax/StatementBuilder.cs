using System;
using System.Collections.Generic;

using Shockky.Lingo.Instructions;

namespace Shockky.Lingo.Syntax
{
    public class StatementBuilder : InstructionVisitor<Stack<Expression>, Statement>
    {
        private readonly ExpressionBuilder _expressionBuilder;

        private readonly Dictionary<Jumper, Instruction[]> _jumpBlocks;
        private readonly Dictionary<Jumper, BlockStatement> _jumpBlockStatements;

        private readonly Dictionary<IfTrueIns, Instruction[]> _elseBlocks;

        public StatementBuilder(LingoCode code)
        {
            _expressionBuilder = new ExpressionBuilder(this);

            _jumpBlocks = new Dictionary<Jumper, Instruction[]>(code.JumpExits.Count);
            foreach (Jumper jumper in code.JumpExits.Keys)
            {
                _jumpBlocks.Add(jumper, code.GetJumpBlock(jumper));
            }
            /*
            foreach (Jumper jumper in code.JumpExits.Keys)
            {
                _jumpBlockStatements.Add(jumper, ConvertBlock(code.GetJumpBlock(jumper)));
            }
            */
        }

        public BlockStatement ConvertBlock(IEnumerable<Instruction> instructions)
        {
            var statements = new List<Statement>();
            var stack = new Stack<Expression>();
            
            foreach (var instruction in instructions)
            { 
                Statement stmt = instruction?.AcceptVisitor(this, stack);
                if(stmt != null)
                    statements.Add(stmt);
            }

            return new BlockStatement(statements);
        }

        public CallExpression CreateCall(Call call, Stack<Expression> expressionStack)
        {
            Expression expression = expressionStack.Pop();
            
            if (!(expression is ArgumentListExpression) && 
                !(expression is ListExpression list))
                throw new Exception();

            IList<Expression> arguments = ((IArgumentList)expression).Items;
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
            return new ExpressionStatement(CreateCall(call, expressionStack));
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
            throw new NotImplementedException(nameof(VisitIfTrueInstruction));
            return default;
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
