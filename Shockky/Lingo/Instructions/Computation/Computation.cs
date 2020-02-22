namespace Shockky.Lingo.Instructions
{
    public abstract class Computation : Instruction
    {
        public BinaryOperatorKind Kind { get; }

        protected Computation(OPCode op)
            : this(op, BinaryOperatorKind.Unknown)
        { }
        protected Computation(OPCode op, BinaryOperatorKind kind)
            : base(op)
        {
            Kind = kind;
        }

        public override int GetPopCount() => 2;
        public override int GetPushCount() => 1;

        public override void AcceptVisitor(InstructionVisitor visitor)
        {
            visitor.VisitComputationInstruction(this);
        }
        public override void AcceptVisitor<TContext>(InstructionVisitor<TContext> visitor, TContext context)
        {
            visitor.VisitComputationInstruction(this, context);
        }
        public override T AcceptVisitor<TContext, T>(InstructionVisitor<TContext, T> visitor, TContext context)
        {
            return visitor.VisitComputationInstruction(this, context);
        }

        public override void Execute(LingoMachine machine)
        {
            object right = machine.Values.Pop();
            object left = machine.Values.Pop();
                            
            object result = null;
            if (left != null && right != null)
            {
                result = Execute(left, right);
            }
            machine.Values.Push(result);
        }
        protected abstract object Execute(object left, object right);

        public static bool IsValid(OPCode op)
        {
            switch (op)
            {
                case OPCode.Multiple:
                case OPCode.Add:
                case OPCode.Substract:
                case OPCode.Divide:
                case OPCode.Modulo:
                case OPCode.JoinString:
                case OPCode.JoinPadString:
                case OPCode.LessThan:
                case OPCode.LessThanEquals:
                case OPCode.NotEqual:
                case OPCode.Equals:
                case OPCode.GreaterThan:
                case OPCode.GreaterEquals:
                case OPCode.And:
                case OPCode.Or:
                case OPCode.ContainsString:
                case OPCode.StartsWith:
                case OPCode.OntoSprite:
                case OPCode.IntoSprite:
                    return true;
                default:
                    return false;
            }
        }

    }
}