namespace Shockky.Lingo.Instructions
{
    public class PopIns : Instruction
    {
        public PopIns(int popCount)
            : base(OPCode.Pop)
        {
            Value = popCount;
        }

        public override int GetPopCount() => Value;

        public override void Execute(LingoMachine machine)
        {
            for (int i = 0; i < GetPopCount(); i++)
            {
                machine.Values.Pop();
            }
        }

        public override void AcceptVisitor(InstructionVisitor visitor)
        {
            visitor.VisitPopInstruction(this);
        }
        public override void AcceptVisitor<TContext>(InstructionVisitor<TContext> visitor, TContext context)
        {
            visitor.VisitPopInstruction(this, context);
        }
        public override T AcceptVisitor<TContext, T>(InstructionVisitor<TContext, T> visitor, TContext context)
        {
            return visitor.VisitPopInstruction(this, context);
        }
    }
}
