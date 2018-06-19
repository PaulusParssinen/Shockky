using System.Collections.Generic;
using System.Diagnostics;

namespace Shockky.Shockwave.Lingo.AST
{
    public abstract class AstNode //: ICloneable
    {
        public AstNode Parent { get; private set; }
        public AstNode NextSibling { get; private set; }
        public AstNode PrevSibling { get; private set; }
        public AstNode FirstChild { get; private set; }
        public AstNode LastChild { get; private set; }

        public IEnumerable<AstNode> Children
        {
            get
            {
                AstNode next;
                for (var cur = FirstChild; cur != null; cur = next)
                {
                    Debug.Assert(cur.Parent == this);

                    next = cur.NextSibling;
                    yield return cur;
                }
            }
        }

        public IEnumerable<AstNode> GetChildren()
        {
            yield break;
        }

        public void AddChild<T>(T child) where T : AstNode
        {
            if (child == null) return;
            AddChildUnsafe(child);
        }

        internal void AddChildUntyped(AstNode child)
        {
            if (child == null) return;
            AddChildUnsafe(child);
        }

        private void AddChildUnsafe(AstNode child)
        {
            child.Parent = this;
            if (FirstChild != null)
            {
                LastChild.NextSibling = child;
                child.PrevSibling = LastChild;
                LastChild = child;
            }
            else LastChild = FirstChild = child;
        }
		public abstract void AcceptVisitor(IAstVisitor visitor);
	}
}
