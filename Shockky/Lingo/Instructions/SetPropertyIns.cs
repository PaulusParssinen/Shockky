namespace Shockky.Lingo.Instructions
{
    public class SetPropertyIns : VariableAssignment
    {
        private int _propertNameIndex;
        public int PropertyNameIndex
        {
            get => _propertNameIndex;
            set
            {
                _propertNameIndex = value;
                Name = Pool.NameList[value];
            }
        }

        public SetPropertyIns(LingoHandler handler)
            : base(OPCode.SetProperty, handler)
        { }
        public SetPropertyIns(LingoHandler handler, int propertyNameIndex)
            : this(handler)
        {
            PropertyNameIndex = propertyNameIndex;
        }
    }
}