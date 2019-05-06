namespace Shockky.Lingo.Instructions
{
    public class WrapListIns : Instruction
    {
        public WrapListIns() 
            : base(OPCode.WrapList)
        { }

        public override int GetPopCount() => 1;
        public override int GetPushCount() => 1;

        public override void AcceptVisitor(InstructionVisitor visitor)
        {
            visitor.VisitWrapListInstrution(this);
        }
        public override void AcceptVisitor<TContext>(InstructionVisitor<TContext> visitor, TContext context)
        {
            visitor.VisitWrapListInstrution(this, context);
        }
        public override T AcceptVisitor<TContext, T>(InstructionVisitor<TContext, T> visitor, TContext context)
        {
            return visitor.VisitWrapListInstrution(this, context);
        }
    }
}