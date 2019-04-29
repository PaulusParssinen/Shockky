using System;
namespace Shockky.Lingo.Bytecode.Instructions
{
    public class StartObjectIns : Instruction
    {
        public StartObjectIns()
            : base(OPCode.StartObject)
        { }

        public override int GetPopCount() => 1;
    }
}