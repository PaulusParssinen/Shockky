namespace Shockky.Lingo.Syntax
{
    public class HiliteStatement : Statement
    {
        public Expression Field { get; set; }

        public HiliteStatement()
        { }
        public HiliteStatement(Expression field)
        {
            Field = field;
        }

        public override void AcceptVisitor(IAstVisitor visitor)
        {
            visitor.VisitHiliteStatement(this);
        }
    }
}
