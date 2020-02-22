namespace Shockky.Lingo.Instructions
{
    public class SwapIns : Instruction
    {
        public SwapIns()
            : base(OPCode.Swap)
        { }

        public override int GetPopCount() => 2;
        public override int GetPushCount() => 2;

        public override void Execute(LingoMachine machine)
        {
            object value2 = machine.Values.Pop();
            object value1 = machine.Values.Pop();

            machine.Values.Push(value2);
            machine.Values.Push(value1);
        }

        public override void AcceptVisitor(InstructionVisitor visitor)
        {
            visitor.VisitSwapInstruction(this);
        }
        public override void AcceptVisitor<TContext>(InstructionVisitor<TContext> visitor, TContext context)
        {
            visitor.VisitSwapInstruction(this, context);
        }
        public override T AcceptVisitor<TContext, T>(InstructionVisitor<TContext, T> visitor, TContext context)
        {
            return visitor.VisitSwapInstruction(this, context);
        }
    }
}
