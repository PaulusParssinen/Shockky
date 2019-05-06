namespace Shockky.Lingo.Instructions
{
    public class InverseIns : Unary
    {
        public InverseIns()
            : base(OPCode.Inverse, UnaryOperatorKind.Minus)
        { }
        
        public override void Execute(LingoMachine machine)
        {
            int value = (int)machine.Values.Pop(); //TODO: Only integers?
            machine.Values.Push(value * -1);
        }
    }
}