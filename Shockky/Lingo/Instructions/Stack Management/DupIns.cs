namespace Shockky.Lingo.Instructions
{
    public class DupIns : Instruction
    {
        public DupIns(int value)
            : base(OPCode.Dup)
        {
            Value = value; //TODO: No fucking idea what this is
        }

        public override void Execute(LingoMachine machine)
        {
            object value = machine.Values.Peek();
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
