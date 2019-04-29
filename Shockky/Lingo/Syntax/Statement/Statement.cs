namespace Shockky.Lingo.Syntax
{
    public abstract class Statement : AstNode
    {
        public override NodeKind Kind => NodeKind.Statement;
    }
}
