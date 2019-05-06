namespace Shockky.Lingo.Instructions
{
    public class ReturnIns : Instruction
    {
        public ReturnIns()
            : base(OPCode.Return)
        { }

        public override void AcceptVisitor(InstructionVisitor visitor)
        {
            visitor.VisitReturnInstruction(this);
        }
        public override void AcceptVisitor<TContext>(InstructionVisitor<TContext> visitor, TContext context)
        {
            visitor.VisitReturnInstruction(this, context);
        }
        public override T AcceptVisitor<TContext, T>(InstructionVisitor<TContext, T> visitor, TContext context)
        {
            return visitor.VisitReturnInstruction(this, context);
        }
    }
}