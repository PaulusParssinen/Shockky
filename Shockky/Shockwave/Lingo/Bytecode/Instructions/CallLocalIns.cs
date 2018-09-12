namespace Shockky.Shockwave.Lingo.Bytecode.Instructions
{
    public class CallLocalIns : Instruction
    {
        public LingoHandler LocalHandler => Pool.Handlers[Value];

        public CallLocalIns(LingoHandler handler)
            : base(OPCode.CallLocal, handler)
        { }
        public CallLocalIns(LingoHandler handler, int handlerIndex)
            : this(handler)
        {
            Value = handlerIndex;
        }
    }
}