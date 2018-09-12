namespace Shockky.Shockwave.Lingo.Bytecode.Instructions
{
    public class IfTrueIns : Jumper
    {
        public IfTrueIns()
            : base(OPCode.IfTrue)
        { }
        public IfTrueIns(LingoHandler handler, int offset)
            : base(OPCode.IfTrue, handler, offset)
        { }

        public override bool? RunCondition(LingoMachine machine)
        {
            return (machine.Values.Pop() as bool?);
        }
    }
}