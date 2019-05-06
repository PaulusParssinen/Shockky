namespace Shockky.Lingo.Instructions
{
    public class CallLocalIns : Call
    {
        private int _handlerIndex;
        public int HandlerIndex
        {
            get => _handlerIndex;
            set
            {
                _handlerIndex = value;
                TargetFunction = Pool.Handlers[value].Name;
            }
        }

        public LingoHandler TargetHandler => Pool.Handlers[Value];

        public CallLocalIns(LingoHandler handler)
            : base(OPCode.CallLocal, handler)
        { }
        public CallLocalIns(LingoHandler handler, int handlerIndex)
            : this(handler)
        {
            HandlerIndex = handlerIndex;
        }

        protected override int SetTarget(string functionName)
            => Pool.Handlers.FindIndex(handler => handler.Name == functionName);
    }
}