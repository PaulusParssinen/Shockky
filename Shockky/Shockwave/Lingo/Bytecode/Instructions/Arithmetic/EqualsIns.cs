namespace Shockky.Shockwave.Lingo.Bytecode.Instructions
{
    public class EqualsIns : Computation
    {
        public EqualsIns()
            : base(OPCode.Equals)
        { }

        protected override object Execute(dynamic left, dynamic right)
        {
            return (left == right);
        }
    }
}