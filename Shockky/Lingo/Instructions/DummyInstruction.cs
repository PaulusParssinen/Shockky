using System.Diagnostics;

namespace Shockky.Lingo.Instructions
{
    public class DummyInstruction : Instruction
    {
        public DummyInstruction(OPCode op, LingoHandler handler, int value) 
            : base(op, handler)
        {
            Value = value;
            Debug.WriteLine($"{op} - {value}");
        }
    }
}
