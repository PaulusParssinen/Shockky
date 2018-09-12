using System.Diagnostics;

namespace Shockky.Shockwave.Lingo.Bytecode.Instructions
{
    public class GetIns : Instruction
    {
        public GetIns(LingoHandler handler, int value)
            : base(OPCode.Get, handler)
        {
            Value = value;
            Debug.WriteLine("GET: " + value);
        }
    }
}