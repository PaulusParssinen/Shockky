using Shockky.Lingo.Bytecode.Instructions;

namespace Shockky.Lingo.Syntax
{
    public class BinaryOperatorExpression : Expression
    {
        public BinaryOperatorKind OperatorKind { get; set; }

        public Expression Left { get; set; }
        public Expression Right { get; set; }

        public BinaryOperatorExpression()
        { }
        public BinaryOperatorExpression(Expression left, BinaryOperatorKind opKind, Expression right)
        {
            OperatorKind = opKind;

            Left = left;
            Right = right;
        }

        public override void AcceptVisitor(IAstVisitor visitor)
        {
            visitor.VisitBinaryOperatorExpression(this);
        }
    }
}
