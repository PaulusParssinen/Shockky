using System;
using System.Collections.Generic;
using System.Text;

namespace Shockky.Lingo.Syntax
{
    public abstract class AstNode : ICloneable
    {
        public abstract NodeKind Kind { get; }

        public int Start { get; set; }
        public int End { get; set; }

        //public List<AstNode> Children { get; set; } = new List<AstNode>();

        public abstract void AcceptVisitor(IAstVisitor visitor);

        object ICloneable.Clone() => Clone();
        public AstNode Clone() => (AstNode)MemberwiseClone();
    }
}
