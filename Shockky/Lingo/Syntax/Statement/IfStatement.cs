namespace Shockky.Lingo.Syntax
{
    public class IfStatement : Statement
    {
        public Expression Condition { get; set; }

        public Statement IfBlock { get; set; }
        public Statement ElseBlock { get; set; }
        
        public IfStatement() { }
        public IfStatement(Expression condition, Statement ifBlock, Statement elseBlock = default)
        {
            Condition = condition;

            IfBlock = ifBlock;
            ElseBlock = elseBlock;
        }

        public override void AcceptVisitor(IAstVisitor visitor)
        {
            visitor.VisitIfStatement(this);
        }
    }
}
