namespace Shockky.Shockwave.Lingo.Bytecode.AST.Statements
{
    public class BlockStatement : Statement
    {
        public override void AcceptVisitor(IAstVisitor visitor)
        {
            visitor.VisitBlockStatement(this);
        }
    }
}
