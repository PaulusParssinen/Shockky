namespace Shockky.Lingo.Instructions
{
    public class CallExternalIns : Call
    {
        private int _functionNameIndex;
        public int FunctionNameIndex
        {
            get => _functionNameIndex;
            set
            {
                _functionNameIndex = value;
                TargetFunction = Pool.NameList[value];
            }
        }

        public CallExternalIns(LingoHandler handler)
            : base(OPCode.CallExternal, handler)
        { }
        public CallExternalIns(LingoHandler handler, int externalFunctionNameIndex)
            : this(handler)
        {
            FunctionNameIndex = externalFunctionNameIndex;
        }
    }
}