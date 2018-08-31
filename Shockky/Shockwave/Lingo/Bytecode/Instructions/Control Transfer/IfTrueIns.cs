using Shockky.IO;
namespace Shockky.Shockwave.Lingo.Bytecode.Instructions
{
    public class IfTrueIns : Jumper
    {
        public IfTrueIns()
            : base(OPCode.IfTrue)
        { }

        public IfTrueIns(LingoHandler handler, ShockwaveReader input, byte opByte)
            : base(OPCode.IfTrue, handler, input, opByte)
        { }

        public override bool? RunCondition(LingoMachine machine)
        {
            throw new System.NotImplementedException();
        }
    }
}