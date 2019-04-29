using System.Collections.Generic;

namespace Shockky.Lingo.Syntax
{
    public class CallExpression : Expression
    {
        public string Target { get; set; }
        public IEnumerable<Expression> Arguments { get; set; }

        public CallExpression()
        { }
        public CallExpression(string target, IEnumerable<Expression> arguments)
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
