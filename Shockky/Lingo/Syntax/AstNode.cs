using System;

namespace Shockky.Lingo.Syntax
{
    public abstract class AstNode : ICloneable
    {
        public abstract NodeKind Kind { get; }

        //public List<AstNode> Children { get; set; } = new List<AstNode>();

        public abstract void AcceptVisitor(IAstVisitor visitor);

        object ICloneable.Clone() => Clone();
        public AstNode Clone() => (AstNode)MemberwiseClone();
    }
}
