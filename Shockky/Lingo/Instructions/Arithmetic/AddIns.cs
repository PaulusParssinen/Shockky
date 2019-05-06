namespace Shockky.Lingo.Instructions
{
    public class AddIns : Computation
    {
        public AddIns()
            : base(OPCode.Add, BinaryOperatorKind.Add)
        { }

        protected override object Execute(dynamic left, dynamic right)
        {
            return (left + right);
        }
    }
}