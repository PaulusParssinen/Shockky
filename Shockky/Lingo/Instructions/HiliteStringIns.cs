namespace Shockky.Lingo.Instructions
{
    public class HiliteStringIns : Instruction
    {
        public HiliteStringIns()
            : base(OPCode.Hilite)
        { }

        public override int GetPopCount() => 9;
        public override int GetPushCount() => 1;

        public override void AcceptVisitor(InstructionVisitor visitor)
        {
            visitor.VisitLightStringInstruction(this);
        }
        public override void AcceptVisitor<TContext>(InstructionVisitor<TContext> visitor, TContext context)
        {
            visitor.VisitLightStringInstruction(this, context);
        }
        public override T AcceptVisitor<TContext, T>(InstructionVisitor<TContext, T> visitor, TContext context)
        {
            return visitor.VisitLightStringInstruction(this, context);
        }
    }
}
 