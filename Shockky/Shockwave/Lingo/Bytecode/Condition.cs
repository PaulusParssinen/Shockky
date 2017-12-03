using Shockky.Shockwave.Lingo.Bytecode.AST;

namespace Shockky.Shockwave.Lingo.Bytecode
{
    public class Condition : AstNode
    {
        public Instruction EntryCondition { get; set; }

        public Block TrueBlock { get; set; }
        public Block FalseBlock { get; set; }

    }
}
