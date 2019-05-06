namespace Shockky.Lingo.Instructions
{
    public class CallObjectIns : Call
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

        public CallObjectIns(LingoHandler handler)
            : base(OPCode.CallObj, handler)
        {
            IsObjectCall = true;
        }
        public CallObjectIns(LingoHandler handler, int handlerNameIndex)
            : this(handler)
        {
            FunctionNameIndex = handlerNameIndex;
        }
    }
}