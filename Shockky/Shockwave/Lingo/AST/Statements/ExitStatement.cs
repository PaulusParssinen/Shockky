namespace Shockky.Shockwave.Lingo.AST.Statements
{
    public class ExitStatement : Statement
    {
	    public override void AcceptVisitor(IAstVisitor visitor)
	    {
		    visitor.VisitExitStatement(this);
	    }
    }
}
