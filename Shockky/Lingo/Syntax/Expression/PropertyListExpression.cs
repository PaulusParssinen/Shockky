using System.Collections.Generic;

namespace Shockky.Lingo.Syntax
{
    public class PropertyListExpression : Expression
    {
        public Dictionary<Expression, Expression> Properties { get; set; } 

        public PropertyListExpression() { }
        public PropertyListExpression(Dictionary<Expression, Expression> properties)
        {
            Properties = properties;
        }

        public override void AcceptVisitor(IAstVisitor visitor)
        {
            visitor.VisitPropertyListExpression(this);
        }
    }
}
