namespace Shockky.Lingo.Syntax
{
    public class RepeatStatement : Statement
    {
        public Expression Condition { get; set; }
        public BlockStatement Body { get; set; }

        public RepeatStatement()
        { }
        public RepeatStatement(Expression condition, BlockStatement body)
        {
            Condition = condition;
            Body = body;
        }

        public override void AcceptVisitor(IAstVisitor visitor)
        {
            visitor.VisitRepeatStatement(this);
        }
    }
}
