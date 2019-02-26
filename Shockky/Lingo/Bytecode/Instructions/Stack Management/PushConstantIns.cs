using System.Diagnostics;

namespace Shockky.Lingo.Bytecode.Instructions
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
                _value = /*Pool.Literals[value].Value*/ Pool.NameList[value];
            }
        }

        public PushConstantIns(LingoHandler handler)
            : base(OPCode.PushConstant, handler)
        { }
        public PushConstantIns(LingoHandler handler, int valueIndex)
            : this(handler)
        {
            ValueIndex = valueIndex;
        }
        public PushConstantIns(LingoHandler handler, object value)
            : this(handler)
        {
            Value = value;
        }
    }
}