namespace Shockky.Lingo.Syntax
{
    public class ExitStatement : Statement
    {
        public override void AcceptVisitor(IAstVisitor visitor)
        {
            visitor.VisitExitStatement(this);
        }
    }
}
