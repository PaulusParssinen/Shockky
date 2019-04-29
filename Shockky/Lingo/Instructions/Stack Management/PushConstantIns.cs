namespace Shockky.Lingo.Bytecode.Instructions
{
    public class PushConstantIns : Primitive
    {
        private int _valueIndex;
        public int ValueIndex
        {
            get => _valueIndex;
            set
            {
                _valueIndex = value;
                base.Value = Pool.Literals[value].Value;
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

        protected override int SetValue(object value)
            => _valueIndex = Pool.AddLiteral(value);
    }
}