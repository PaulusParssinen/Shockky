using System;
using System.Collections.Generic;

using Shockky.Lingo.Instructions;

namespace Shockky.Lingo.Syntax
{
    public class ExpressionBuilder : InstructionVisitor<Stack<Expression>>
    {
        private readonly StatementBuilder _statementBuilder;

        public ExpressionBuilder(StatementBuilder statementBuilder)
        {
            _statementBuilder = statementBuilder;
        }

        public override void VisitNewListInstruction(NewListIns newList, Stack<Expression> expressionStack)
        {
            List<Expression> items = new List<Expression>(newList.ItemCount);
            for (int i = 0; i < items.Capacity; i++)
            {
                items.Add(expressionStack.Pop());
            }
            items.Reverse();

            if (newList.IsArgumentList)
                expressionStack.Push(new ArgumentListExpression(items));
            else expressionStack.Push(new ListExpression(items));
        }
        public override void VisitWrapListInstrution(WrapListIns wrapList, Stack<Expression> expressionStack)
        {
            if (!(expressionStack.Pop() is ListExpression listExpression))
                throw new Exception(nameof(VisitWrapListInstrution));
            
            listExpression.IsWrapped = true;
            expressionStack.Push(listExpression);
        }

        public override void VisitNewPropertyListInstruction(NewPropListIns newPropList, Stack<Expression> expressionStack)
        {
            if (!(expressionStack.Pop() is ListExpression listExpression))
                throw new Exception(nameof(VisitNewPropertyListInstruction));

            if (listExpression.Items.Count % 2 != 0)
                throw new InvalidOperationException("Preceding listExpression must be in property-value pairs");

            var properties = new Dictionary<Expression, Expression>(listExpression.Items.Count / 2);
            for (int i = 0; i < listExpression.Items.Count; i += 2)
            {
                properties.Add(listExpression.Items[i], listExpression.Items[i+1]);
            }
            expressionStack.Push(new PropertyListExpression(properties));
        }

        public override void VisitVariableReferenceInstruction(VariableReference variableReference, Stack<Expression> expressionStack)
        {
            Expression referenceExpression = new IdentifierExpression(variableReference.Name);

            if (variableReference.IsMovieReference)
            {
                expressionStack.Pop(); //TODO:
                referenceExpression = new MovieReferenceExpression(referenceExpression);
            }

            if (variableReference.IsObjectReference)
            {
                Expression objectExpression = expressionStack.Pop();
                referenceExpression = new MemberReferenceExpression(objectExpression, (IdentifierExpression)referenceExpression);
            }

            expressionStack.Push(referenceExpression);
        }
        public override void VisitPrimitiveInstruction(Primitive primitive, Stack<Expression> expressionStack)
        {
            expressionStack.Push(new PrimitiveExpression(primitive.Value));
        }
        public override void VisitSymbolInstruction(PushSymbolIns symbol, Stack<Expression> expressionStack)
        {
            expressionStack.Push(new SymbolExpression(symbol.Name));
        }

        public override void VisitUnaryInstruction(Unary unary, Stack<Expression> expressionStack)
        {
            Expression expression = expressionStack.Pop();
            expressionStack.Push(new UnaryOperatorExpression(unary.Kind, expression));
        }
        public override void VisitComputationInstruction(Computation computation, Stack<Expression> expressionStack)
        {
            Expression rhs = expressionStack.Pop();
            Expression lhs = expressionStack.Pop();

            expressionStack.Push(new BinaryOperatorExpression(lhs, computation.Kind, rhs));
        }

        public override void VisitPopInstruction(PopIns pop, Stack<Expression> expressionStack)
        {
            for (int i = 0; i < pop.GetPopCount(); i++)
            {
                expressionStack.Pop();
            }
        }
        public override void VisitSwapInstruction(SwapIns swap, Stack<Expression> expressionStack)
        {
            Expression value2 = expressionStack.Pop();
            Expression value1 = expressionStack.Pop();

            expressionStack.Push(value2);
            expressionStack.Push(value1);
        }
        public override void VisitDuplicateInstruction(DupIns dup, Stack<Expression> expressionStack)
        {
            expressionStack.Push(expressionStack.ToArray()[dup.Slot]);
        }

        protected override void Default(Instruction instruction, Stack<Expression> expressionStack)
        {
            instruction.AcceptVisitor(_statementBuilder, expressionStack);
        }
    }
}
