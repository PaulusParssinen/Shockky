namespace Shockky.Shockwave.Lingo.Bytecode.Instructions
{
    public class MultipleIns : Computation
    {
        public MultipleIns()
            : base(OPCode.Multiple)
        { }

        protected override object Execute(dynamic left, dynamic right)
        {
            return (left * right);
        }
    }
}