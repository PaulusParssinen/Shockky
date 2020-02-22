namespace Shockky.Lingo.Instructions
{
    public abstract class Unary : Instruction
    {
        public UnaryOperatorKind Kind { get; }

        protected Unary(OPCode op, UnaryOperatorKind kind)
            : base(op)
        {
            Kind = kind;
        }
        
        public override int GetPopCount() => 1;
        public override int GetPushCount() => 1;

        public override void AcceptVisitor(InstructionVisitor visitor)
        {
            visitor.VisitUnaryInstruction(this);
        }
        public override void AcceptVisitor<TContext>(InstructionVisitor<TContext> visitor, TContext context)
        {
            visitor.VisitUnaryInstruction(this, context);
        }
        public override T AcceptVisitor<TContext, T>(InstructionVisitor<TContext, T> visitor, TContext context)
        {
            return visitor.VisitUnaryInstruction(this, context);
        }

        public static bool IsValid(OPCode op)
        {
            switch (op)
            {
                case OPCode.Inverse:
                case OPCode.Not:
                    return true;
                default:
                    return false;
            }
        }
    }
}
