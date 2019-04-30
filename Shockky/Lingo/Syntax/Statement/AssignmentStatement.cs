namespace Shockky.Lingo.Syntax
{
    public class AssignmentStatement : Statement
    {
        public Expression Target { get; set; }
        public Expression Initializer { get; set; }

        public AssignmentStatement() { }
        public AssignmentStatement(Expression targetExpression, Expression initializerExpression)
        {
            Target = targetExpression;
            Initializer = initializerExpression;
        }

        public override void AcceptVisitor(IAstVisitor visitor)
        {
            visitor.VisitAssigmentStatement(this);
        }
    }
}
