namespace Shockky.Lingo.Syntax
{
    public class ExpressionStatement : Statement
    {
        public Expression Expression { get; set; }

        public ExpressionStatement(Expression expression)
        {
            Expression = expression;
        }

        public override void AcceptVisitor(IAstVisitor visitor)
        {
            visitor.VisitExpressionStatement(this);
        }
    }
}
