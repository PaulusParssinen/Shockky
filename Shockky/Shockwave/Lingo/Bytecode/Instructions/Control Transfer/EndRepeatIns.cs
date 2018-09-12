using System;

namespace Shockky.Shockwave.Lingo.Bytecode.Instructions
{
    public class EndRepeatIns : Jumper
    {
        public EndRepeatIns(LingoHandler handler, int offset) 
            : base(OPCode.EndRepeat, handler, offset)
        { }

        public override bool? RunCondition(LingoMachine machine) => throw new NotImplementedException();
    }
}