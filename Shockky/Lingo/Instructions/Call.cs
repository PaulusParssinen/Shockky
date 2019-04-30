namespace Shockky.Lingo.Bytecode.Instructions
{
    public abstract class Call : Instruction
    {
        private string _targetFunction;
        public string TargetFunction
        {
            get => _targetFunction;
            set
            {
                _targetFunction = value;
                base.Value = SetTarget(value);
            }
        }

        public bool IsObjectCall { get; protected set; }

        protected Call(OPCode op)
            : base(op)
        { }
        protected Call(OPCode op, LingoHandler handler)
            : base(op, handler)
        { }

        protected virtual int SetTarget(string functionName) => Pool.AddName(functionName); //TODO:

        public override void AcceptVisitor(InstructionVisitor visitor)
        {
            visitor.VisitCallInstruction(this);
        }
        public override void AcceptVisitor<TContext>(InstructionVisitor<TContext> visitor, TContext context)
        {
            visitor.VisitCallInstruction(this, context);
        }
        public override T AcceptVisitor<TContext, T>(InstructionVisitor<TContext, T> visitor, TContext context)
        {
            return visitor.VisitCallInstruction(this, context);
        }
    }
}
