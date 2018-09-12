namespace Shockky.Shockwave.Lingo.Bytecode.Instructions
{
    public class GetObjPropertyIns : Instruction
    {
        private string _value;
        public new string Value
        {
            get => _value;
            set
            {
                _value = value;
                _valueIndex = Pool.AddName(value);
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
                _value = Pool.NameList[value];
            }
        }

        public GetObjPropertyIns(LingoHandler handler)
            : base(OPCode.GetObjProp, handler)
        { }
        public GetObjPropertyIns(LingoHandler handler, int propertyNameIndex)
            : this(handler)
        {
            ValueIndex = propertyNameIndex;
        }
        public GetObjPropertyIns(LingoHandler handler, string propertyName)
            : this(handler)
        {
            Value = propertyName;
        }
    }
}