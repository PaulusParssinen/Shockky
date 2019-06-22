namespace Shockky.Lingo.Instructions
{
    public abstract class VariableAssignment : VariableReference
    {
        protected VariableAssignment(OPCode op, LingoHandler handler)
            : base(op, handler)
        { }
        protected VariableAssignment(OPCode op)
            : base(op)
        { }

        public override void AcceptVisitor(InstructionVisitor visitor)
        {
            visitor.VisitVariableAssignmentInstruction(this);
        }
        public override void AcceptVisitor<TContext>(InstructionVisitor<TContext> visitor, TContext context)
        {
            visitor.VisitVariableAssignmentInstruction(this, context);
        }
        public override T AcceptVisitor<TContext, T>(InstructionVisitor<TContext, T> visitor, TContext context)
        {
            return visitor.VisitVariableAssignmentInstruction(this, context);
        }

        public override int GetPopCount() => 1;
    }
}
