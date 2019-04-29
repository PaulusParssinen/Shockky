using System.Collections;
using System.Collections.Generic;

namespace Shockky.Lingo.Syntax
{
    public class ListExpression : Expression, IEnumerable<Expression>
    {
        public IEnumerable<Expression> Items { get; set; }
        public bool IsWrapped { get; set; }

        public ListExpression()
        { }
        public ListExpression(IEnumerable<Expression> items)
        {
            Items = items;
        }

        public override void AcceptVisitor(IAstVisitor visitor)
        {
            visitor.VisitListExpression(this);
        }

        public IEnumerator<Expression> GetEnumerator() => Items.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
