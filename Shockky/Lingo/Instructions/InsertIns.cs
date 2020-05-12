namespace Shockky.Lingo.Instructions
{
    public class InsertIns : Instruction
    {
        public InsertIns(LingoHandler handler) 
            : base(OPCode.Insert, handler)
        { }
        public InsertIns(LingoHandler handler, int value)
            : this(handler)
        {
            Value = value;
        }

        public override int GetPopCount() => 10;

        public override void AcceptVisitor(InstructionVisitor visitor)
        {
            visitor.VisitInsertInstruction(this);
        }
        public override void AcceptVisitor<TContext>(InstructionVisitor<TContext> visitor, TContext context)
        {
            visitor.VisitInsertInstruction(this, context);
        }
        public override T AcceptVisitor<TContext, T>(InstructionVisitor<TContext, T> visitor, TContext context)
        {
            return visitor.VisitInsertInstruction(this, context);
        }
    }
}
