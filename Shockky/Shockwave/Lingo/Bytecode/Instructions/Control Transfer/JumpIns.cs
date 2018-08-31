using Shockky.IO;
namespace Shockky.Shockwave.Lingo.Bytecode.Instructions
{
    public class JumpIns : Jumper
    {
        public JumpIns(LingoHandler handler)
            : base(OPCode.Jump, handler)
        { }
        public JumpIns(LingoHandler handler, int offset)
            : this(handler)
        {
            Value = offset;
        }
        public JumpIns(LingoHandler handler, ShockwaveReader input, byte opByte)
            : base(OPCode.Jump, handler, input, opByte)
        { }

        public override bool? RunCondition(LingoMachine machine) => true;
    }
}