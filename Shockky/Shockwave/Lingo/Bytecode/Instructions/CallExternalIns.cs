namespace Shockky.Shockwave.Lingo.Bytecode.Instructions
{
    public class CallExternalIns : Instruction
    {
        public int ExternalFunctionNameIndex { get; set; }
        public string ExternalFunctionName => Pool.NameList[ExternalFunctionNameIndex];

        public CallExternalIns(LingoHandler handler)
            : base(OPCode.CallExternal, handler)
        { }
        public CallExternalIns(LingoHandler handler, int externalFunctionNameIndex)
            : this(handler)
        {
            ExternalFunctionNameIndex = externalFunctionNameIndex;
        }
    }
}