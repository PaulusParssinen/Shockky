namespace Shockky.Lingo.Instructions
{
    public class DupIns : Instruction
    {
        public int Slot => Value;

        public DupIns(int slot)
            : base(OPCode.Dup)
        {
            Value = slot;
        }

        public override void Execute(LingoMachine machine)
        {
            object value = machine.Values.ToArray()[Slot];
            machine.Values.Push(value);
        }

        public override int GetPushCount() => 1;

        public override void AcceptVisitor(InstructionVisitor visitor)
        {
            visitor.VisitDuplicateInstruction(this);
        }
        public override void AcceptVisitor<TContext>(InstructionVisitor<TContext> visitor, TContext context)
        {
            visitor.VisitDuplicateInstruction(this, context);
        }
        public override T AcceptVisitor<TContext, T>(InstructionVisitor<TContext, T> visitor, TContext context)
        {
            return visitor.VisitDuplicateInstruction(this, context);
        }
    }
}
