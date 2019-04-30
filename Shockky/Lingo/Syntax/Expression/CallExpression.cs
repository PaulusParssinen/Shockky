using System.Collections.Generic;

namespace Shockky.Lingo.Syntax
{
    public class CallExpression : Expression
    {
        public Expression Target { get; set; }
        public IList<Expression> Arguments { get; set; }

        public CallExpression() { }
        public CallExpression(Expression target, IList<Expression> arguments)
        {
            Target = target;
            Arguments = arguments;
        }

        public override void AcceptVisitor(IAstVisitor visitor)
        {
            visitor.VisitCallExpression(this);
        }
    }
}
