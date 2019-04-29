using System.Collections;
using System.Collections.Generic;

namespace Shockky.Lingo.Syntax
{
    public class ArgumentListExpression : Expression, IEnumerable<Expression>
    {
        public IEnumerable<Expression> Arguments { get; set; }

        public ArgumentListExpression()
        { }
        public ArgumentListExpression(IEnumerable<Expression> arguments)
        {
            Arguments = arguments;
        }

        public override void AcceptVisitor(IAstVisitor visitor)
        {
            visitor.VisitArgumentListExpression(this);
        }

        public IEnumerator<Expression> GetEnumerator() => Arguments.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
