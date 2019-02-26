using System.Diagnostics;

namespace Shockky.Lingo.Bytecode.Instructions
{
    public class SetIns : Instruction
    {
        public SetIns(LingoHandler handler, int value) 
            : base(OPCode.Set, handler)
        {
            Value = value;
            Debug.WriteLine("SET: " + value);
        }
    }
}