using System;
using System.Text;
using System.Collections.Generic;

namespace Shockky.Lingo.Syntax
{
    public class StringSplitExpression : Expression
    {
        public StringSplitExpression()
        { }
        public StringSplitExpression(Expression str)
        { }

        public override void AcceptVisitor(IAstVisitor visitor)
        {
            visitor.VisitStringSplitExpression(this);
        }
    }
}
