using System.Collections.Generic;

namespace Shockky.Lingo.Syntax
{
    public class BlockStatement : Statement
    {
        public IList<Statement> Statements { get; set; }

        public BlockStatement()
            : this(new List<Statement>())
        { }
        public BlockStatement(IList<Statement> statements)
        {
            Statements = statements;
        }

        public override void AcceptVisitor(IAstVisitor visitor)
        {
            visitor.VisitBlockStatement(this);
        }
    }
}
