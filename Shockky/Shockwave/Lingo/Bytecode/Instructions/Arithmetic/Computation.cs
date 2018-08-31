namespace Shockky.Shockwave.Lingo.Bytecode.Instructions
{
    public abstract class Computation : Instruction
    {
        protected Computation(OPCode op)
            : base(op)
        { }

        public override int GetPopCount() => 2;
        public override int GetPushCount() => 1;

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
                case OPCode.Inverse:
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
                case OPCode.Not:
                case OPCode.ContainsString:
                case OPCode.Contains0String:
                    return true;
                default:
                    return false;
            }
        }
    }
}