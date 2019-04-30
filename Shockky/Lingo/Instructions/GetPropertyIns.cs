namespace Shockky.Lingo.Bytecode.Instructions
{
    public class GetPropertyIns : VariableReference
    {
        private int _propertyNameIndex;
        public int PropertyNameIndex
        {
            get => _propertyNameIndex;
            set
            {
                _propertyNameIndex = value;
                Name = Pool.NameList[value];
            }
        }

        public GetPropertyIns(LingoHandler handler)
            : base(OPCode.GetProperty, handler)
        { }
        public GetPropertyIns(LingoHandler handler, int propertyNameIndex)
            : this(handler)
        {
            PropertyNameIndex = propertyNameIndex;
        }
    }
}