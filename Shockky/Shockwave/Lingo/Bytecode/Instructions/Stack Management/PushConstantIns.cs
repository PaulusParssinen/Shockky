using Shockky.IO;
using Shockky.Shockwave.Lingo.Bytecode.Instructions.Stack_Management;

namespace Shockky.Shockwave.Lingo.Bytecode.Instructions
{
    public class PushConstantIns : Primitive
    {
        private object _value;
        new public object Value
        {
            get => _value;
            set
            {
                _value = value;
                base.Value = _valueIndex = Pool.AddLiteral(value);
            }
        }

        private int _valueIndex;
        public int ValueIndex
        {
            get => _valueIndex;
            set
            {
                base.Value = value;
                _valueIndex = value;
                _value = Pool.Literals[value].Value;
            }
        }

        public PushConstantIns(LingoHandler handler)
            : base(OPCode.PushConstant, handler)
        { }
        public PushConstantIns(LingoHandler handler, object value)
            : this(handler)
        {
            Value = value;
        }
        public PushConstantIns(LingoHandler handler, ShockwaveReader input, byte opByte) 
            : base(OPCode.PushConstant, handler, input, opByte)
        { }
    }
}