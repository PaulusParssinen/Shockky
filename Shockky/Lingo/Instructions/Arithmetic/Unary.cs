namespace Shockky.Lingo.Bytecode.Instructions
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
