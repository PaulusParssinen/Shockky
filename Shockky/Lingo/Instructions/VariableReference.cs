namespace Shockky.Lingo.Bytecode.Instructions
{
    public abstract class VariableReference : Instruction
    {
        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                base.Value = SetName(value);
            }
        }

        protected VariableReference(OPCode op, LingoHandler handler)
            : base(op, handler)
        { }
        protected VariableReference(OPCode op)
            : base(op)
        { }

        protected virtual int SetName(string name) => Pool.AddName(name);

        public override void AcceptVisitor(InstructionVisitor visitor)
        {
            visitor.VisitVariableReferenceInstruction(this);
        }
        public override void AcceptVisitor<TContext>(InstructionVisitor<TContext> visitor, TContext context)
        {
            visitor.VisitVariableReferenceInstruction(this, context);
        }
        public override T AcceptVisitor<TContext, T>(InstructionVisitor<TContext, T> visitor, TContext context)
        {
            return visitor.VisitVariableReferenceInstruction(this, context);
        }

        public override int GetPushCount() => 1;
    }
}
