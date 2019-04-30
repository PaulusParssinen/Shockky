namespace Shockky.Lingo.Bytecode.Instructions
{
    public class GetObjPropertyIns : VariableReference
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

        public GetObjPropertyIns(LingoHandler handler)
            : base(OPCode.GetObjProp, handler)
        {
            IsObjectReference = true;
        }
        public GetObjPropertyIns(LingoHandler handler, int propertyNameIndex)
            : this(handler)
        {
            PropertyNameIndex = propertyNameIndex;
        }
        public GetObjPropertyIns(LingoHandler handler, string propertyName)
            : this(handler)
        {
            Name = propertyName;
        }

        public override int GetPopCount() => 1;
    }
}