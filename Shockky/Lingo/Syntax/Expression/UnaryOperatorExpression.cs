using Shockky.Lingo.Instructions;

namespace Shockky.Lingo.Syntax
{
    public class UnaryOperatorExpression : Expression
    {
        public UnaryOperatorKind OperatorKind { get; set; }
        public Expression Expression { get; set; }

        public UnaryOperatorExpression() { }
        public UnaryOperatorExpression(UnaryOperatorKind opKind, Expression expression)
        {
            OperatorKind = opKind;
            Expression = expression;
        }

        public override void AcceptVisitor(IAstVisitor visitor)
        {
            visitor.VisitUnaryOperatorExpression(this);
        }
    }
}
