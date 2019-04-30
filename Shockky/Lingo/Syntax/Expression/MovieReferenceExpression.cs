namespace Shockky.Lingo.Syntax
{
    public class MovieReferenceExpression : Expression
    {
        public Expression Expression { get; set; }

        public MovieReferenceExpression() { }
        public MovieReferenceExpression(Expression expression)
        {
            Expression = expression;
        }

        public override void AcceptVisitor(IAstVisitor visitor)
        {
            visitor.VisitMovieReferenceExpression(this);
        }
    }
}
