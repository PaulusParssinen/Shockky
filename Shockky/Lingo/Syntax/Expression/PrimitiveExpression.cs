namespace Shockky.Lingo.Syntax
{
    public class PrimitiveExpression : Expression
    {
        public object Value { get; set; }

        public PrimitiveExpression(object value)
        {
            Value = value;
        }

        public override void AcceptVisitor(IAstVisitor visitor)
        {
            visitor.VisitPrimitiveExpression(this);
        }
    }
}
