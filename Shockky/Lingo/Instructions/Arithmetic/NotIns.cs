namespace Shockky.Lingo.Instructions
{
    public class NotIns : Unary
    {
        public NotIns() 
            : base(OPCode.Not, UnaryOperatorKind.Not)
        { }

        public override void Execute(LingoMachine machine)
        {
            bool value = (bool)machine.Values.Pop();
            machine.Values.Push(!value);
        }
    }
}