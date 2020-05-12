namespace Shockky.Lingo.Instructions
{
    public class InsertStringIns : Instruction
    {
        public InsertStringIns(LingoHandler handler)
            : base(OPCode.InsertString, handler)
        { }
        public InsertStringIns(LingoHandler handler, int value)
            : this(handler)
        {
            Value = value;
        }

        public override int GetPopCount() => 2; //3, 0x1X. 0x16 atleast
        
        public override void AcceptVisitor(InstructionVisitor visitor)
        {
            visitor.VisitInsertStringInstruction(this);
        }
        public override void AcceptVisitor<TContext>(InstructionVisitor<TContext> visitor, TContext context)
        {
            visitor.VisitInsertStringInstruction(this, context);
        }
        public override T AcceptVisitor<TContext, T>(InstructionVisitor<TContext, T> visitor, TContext context)
        {
            return visitor.VisitInsertStringInstruction(this, context);
        }
    }
}
