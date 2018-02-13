using Shockky.Shockwave.Lingo.AST;

namespace Shockky.Shockwave.Lingo.Bytecode
{
    public class Condition : AstNode
    {
        public Instruction EntryCondition { get; set; }

        public Block Body { get; set; }
        public Block ElseBlock { get; set; }

	    public override void AcceptVisitor(IAstVisitor visitor)
	    {
		    throw new System.NotImplementedException();
	    }
    }
}
