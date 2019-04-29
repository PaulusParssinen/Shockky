namespace Shockky.Lingo.Syntax
{
    public abstract class Expression : AstNode
    {
        public override NodeKind Kind => NodeKind.Expression;
    }
}
