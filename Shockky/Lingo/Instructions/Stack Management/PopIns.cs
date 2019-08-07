namespace Shockky.Lingo.Instructions
{
    public class PopIns : Instruction
    {
        public int PopAmount { get; set; }

        public PopIns(int popAmount)
            : base(OPCode.Pop)
        {
            Value = popAmount;
        }

        public override int GetPopCount() => PopAmount;

        public override void Execute(LingoMachine machine)
        {
            for (int i = 0; i < PopAmount; i++)
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
