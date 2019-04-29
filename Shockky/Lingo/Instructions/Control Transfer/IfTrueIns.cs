namespace Shockky.Lingo.Bytecode.Instructions
{
    public class IfTrueIns : Jumper
    {
        public IfTrueIns()
            : base(OPCode.IfTrue)
        { }
        public IfTrueIns(LingoHandler handler, int offset)
            : base(OPCode.IfTrue, handler, offset)
        { }

        public override void AcceptVisitor(InstructionVisitor visitor)
        {
            visitor.VisitIfTrueInstruction(this);
        }
        public override T AcceptVisitor<TContext, T>(InstructionVisitor<TContext, T> visitor, TContext context)
        {
            return visitor.VisitIfTrueInstruction(this, context);
        }

        public override bool? RunCondition(LingoMachine machine)
        {
            return (machine.Values.Pop() as bool?);
        }
    }
}