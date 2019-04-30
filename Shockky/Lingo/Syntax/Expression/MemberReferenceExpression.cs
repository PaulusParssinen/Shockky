namespace Shockky.Lingo.Syntax
{
    public class MemberReferenceExpression : Expression
    {
        public Expression Target { get; set; }
        public IdentifierExpression Member { get; set; }

        public MemberReferenceExpression(){ }
        public MemberReferenceExpression(Expression target, IdentifierExpression member)
        {
            Target = target;
            Member = member;
        }

        public override void AcceptVisitor(IAstVisitor visitor)
        {
            visitor.VisitMemberReferenceExpression(this);
        }
    }
}
