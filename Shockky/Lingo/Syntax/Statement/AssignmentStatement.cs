namespace Shockky.Lingo.Syntax
{
    public class AssignmentStatement : Statement
    {
        public Expression InitializerExpression { get; set; }

        public string Name { get; set; }

        public AssignmentStatement() { }
        public AssignmentStatement(string name, Expression initializerExpression)
        {
            Name = name;
            InitializerExpression = initializerExpression;
        }

        public override void AcceptVisitor(IAstVisitor visitor)
        {
            visitor.VisitAssigmentStatement(this);
        }
    }
}
