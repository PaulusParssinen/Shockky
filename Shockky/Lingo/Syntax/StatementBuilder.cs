using System;
using System.Text;
using System.Collections.Generic;

using Shockky.Lingo.Bytecode;
using Shockky.Lingo.Bytecode.Instructions;

namespace Shockky.Lingo.Syntax
{
    public class StatementBuilder : InstructionVisitor<Stack<Expression>, Statement>
    {
        private readonly ExpressionBuilder _expressionBuilder;
        private readonly Dictionary<Jumper, Instruction[]> _jumpBlocks;

        public StatementBuilder(LingoCode code)
        {
            _expressionBuilder = new ExpressionBuilder(this);

            _jumpBlocks = new Dictionary<Jumper, Instruction[]>(code.JumpExits.Count);
            foreach (Jumper jumper in code.JumpExits.Keys)
            {
                _jumpBlocks.Add(jumper, code.GetJumpBlock(jumper));
            }
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
                !(expression is ListExpression)) throw new InvalidOperationException("illegal");

            return new CallExpression
            {
                Target = call.TargetFunction,
                Arguments = (IEnumerable<Expression>)expression
            };
        }

        public override Statement VisitCallInstruction(Call call, Stack<Expression> expressionStack)
        {
            return new ExpressionStatement(CreateCall(call, expressionStack));
        }

        public override Statement VisitVariableAssignmentInstruction(VariableAssignment variableAssignment, Stack<Expression> expressionStack)
        {
            Expression initializer = expressionStack.Pop();

            return new AssignmentStatement
            {
                Name = variableAssignment.Name,
                InitializerExpression = initializer
            };
        }
        
        public override Statement VisitIfTrueInstruction(IfTrueIns ifTrue, Stack<Expression> context)
        {
            throw new NotImplementedException(nameof(VisitIfTrueInstruction));
            return default;
        }

        protected override Statement Default(Instruction instruction, Stack<Expression> expressionStack)
        {
            instruction.AcceptVisitor(_expressionBuilder, expressionStack);
            return default;
        }
    }
}
