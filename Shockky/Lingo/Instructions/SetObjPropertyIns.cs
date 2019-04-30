namespace Shockky.Lingo.Bytecode.Instructions
{
    public class SetObjPropertyIns : VariableAssignment
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

        public SetObjPropertyIns(LingoHandler handler)
            : base(OPCode.SetObjProp, handler)
        { }
        public SetObjPropertyIns(LingoHandler handler, int propertyNameIndex)
            : this(handler)
        {
            PropertyNameIndex = propertyNameIndex;
        }

        public override int GetPopCount() => 2;
    }
}