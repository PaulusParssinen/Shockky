namespace Shockky.Lingo.Instructions
{
    public class IfFalseIns : Jumper
    {
        public IfFalseIns()
            : base(OPCode.IfFalse)
        { }
        public IfFalseIns(LingoHandler handler, int offset)
            : base(OPCode.IfFalse, handler, offset)
        { }

        public override void AcceptVisitor(InstructionVisitor visitor)
        {
            visitor.VisitIfTrueInstruction(this);
        }
        public override T AcceptVisitor<TContext, T>(InstructionVisitor<TContext, T> visitor, TContext context)
        {
            return visitor.VisitIfFalseInstruction(this, context);
        }

        public override bool? RunCondition(LingoMachine machine)
        {
            return (machine.Values.Pop() as bool?);
        }
    }
}