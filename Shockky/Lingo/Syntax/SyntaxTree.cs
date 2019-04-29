using System;
using System.Collections.Generic;
using System.Text;

namespace Shockky.Lingo.Syntax
{
    public class SyntaxTree : AstNode
    {
        public List<AstNode> Members { get; set; }

        public override NodeKind Kind => NodeKind.Unknown;

        public override void AcceptVisitor(IAstVisitor visitor)
        {
            throw new NotImplementedException();
        }
    }
}
