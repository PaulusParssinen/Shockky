using System;

namespace Shockky.Lingo.Instructions
{
    public class PushFloat : Primitive
    {
        public PushFloat(LingoHandler handler)
            : base(OPCode.PushFloat, handler)
        { }
        public PushFloat(LingoHandler handler, int value)
            : this(handler)
        {
            Value = BitConverter.Int32BitsToSingle(value);
        }
        public PushFloat(LingoHandler handler, float value)
            : this(handler)
        {
            Value = value;
        }

        protected override int SetValue(object value)
            => BitConverter.SingleToInt32Bits((float)Value);
    }
}
