namespace Shockky.Lingo.Syntax
{
    public class IdentifierExpression : Expression
    {
        public string Identifier { get; set; }

        public IdentifierExpression()
        { }
        public IdentifierExpression(string identifier)
        {
            Identifier = identifier;
        }

        public override void AcceptVisitor(IAstVisitor visitor)
        {
            visitor.VisitIdentifierExpression(this);
        }
    }
}
