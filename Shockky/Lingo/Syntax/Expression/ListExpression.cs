using System.Collections.Generic;

namespace Shockky.Lingo.Syntax
{
    public class ListExpression : Expression, IArgumentList
    {
        public IList<Expression> Items { get; set; }
        public bool IsWrapped { get; set; }

        public ListExpression() { }
        public ListExpression(IList<Expression> items)
        {
            Items = items;
        }

        public override void AcceptVisitor(IAstVisitor visitor)
        {
            visitor.VisitListExpression(this);
        }
    }
}
