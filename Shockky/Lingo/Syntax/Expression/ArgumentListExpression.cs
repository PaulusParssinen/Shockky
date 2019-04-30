using System.Collections.Generic;

namespace Shockky.Lingo.Syntax
{
    public class ArgumentListExpression : Expression, IArgumentList
    {
        public IList<Expression> Items { get; set; }

        public ArgumentListExpression() { }
        public ArgumentListExpression(IList<Expression> items)
        {
            Items = items;
        }

        public override void AcceptVisitor(IAstVisitor visitor)
        {
            visitor.VisitArgumentListExpression(this);
        }
    }
}
