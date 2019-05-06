namespace Shockky.Lingo.Syntax
{
    public class SymbolExpression : Expression
    {
        public string Name { get; set; }

        public SymbolExpression(){ }
        public SymbolExpression(string name)
        {
            Name = name;
        }

        public override void AcceptVisitor(IAstVisitor visitor)
        {
            visitor.VisitSymbolExpression(this);
        }
    }
}
