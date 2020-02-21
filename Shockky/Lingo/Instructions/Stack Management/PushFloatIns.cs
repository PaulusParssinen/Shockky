using System;

namespace Shockky.Lingo.Instructions
{
    public class PushFloatIns : Primitive
    {
        public PushFloatIns(LingoHandler handler)
            : base(OPCode.PushFloat, handler)
        { }
        public PushFloatIns(LingoHandler handler, int value)
            : this(handler)
        {
            Value = BitConverter.Int32BitsToSingle(value);
        }
        public PushFloatIns(LingoHandler handler, float value)
            : this(handler)
        {
            Value = value;
        }

        protected override int SetValue(object value)
            => BitConverter.SingleToInt32Bits((float)Value);
    }
}
