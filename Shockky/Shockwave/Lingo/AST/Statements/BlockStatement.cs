namespace Shockky.Shockwave.Lingo.AST.Statements
{
    public class BlockStatement : Statement
    {
        public override void AcceptVisitor(IAstVisitor visitor)
        {
            visitor.VisitBlockStatement(this);
        }
    }
}
