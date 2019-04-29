using Shockky.Lingo.Bytecode.Instructions;

namespace Shockky.Lingo.Syntax
{
    public class UnaryOperatorExpression : Expression
    {
        public UnaryOperatorKind Kind { get; set; }
        public Expression Expression { get; set; }

        public UnaryOperatorExpression(UnaryOperatorKind kind, Expression expression)
        {
            Kind = kind;
            Expression = expression;
        }

        public override void AcceptVisitor(IAstVisitor visitor)
        {
            visitor.VisitUnaryOperatorExpression(this);
        }
    }
}
