namespace Shockky.Lingo.Instructions
{
    public class NewPropListIns : Instruction
    {
        public NewPropListIns()
            : base(OPCode.NewPropList)
        { }

        public override void AcceptVisitor(InstructionVisitor visitor)
        {
            visitor.VisitNewPropertyListInstruction(this);
        }
        public override void AcceptVisitor<TContext>(InstructionVisitor<TContext> visitor, TContext context)
        {
            visitor.VisitNewPropertyListInstruction(this, context);
        }
        public override T AcceptVisitor<TContext, T>(InstructionVisitor<TContext, T> visitor, TContext context)
        {
            return visitor.VisitNewPropertyListInstruction(this, context);
        }

        public override int GetPopCount() => 1;
        public override int GetPushCount() => 1;
    }
}