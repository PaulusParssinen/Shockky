namespace Shockky.Lingo.Instructions
{
    public class SetGlobalIns : VariableAssignment
    {
        private int _valueIndex;
        public int ValueIndex
        {
            get => _valueIndex;
            set
            {
                _valueIndex = value;
                Name = Pool.NameList[value];
            }
        }

        public SetGlobalIns(LingoHandler handler)
            : base(OPCode.SetGlobal, handler)
        { }
        public SetGlobalIns(LingoHandler handler, int globalValueIndex)
            : this(handler)
        {
            ValueIndex = globalValueIndex;
        }
        public SetGlobalIns(LingoHandler handler, string global)
            : this(handler)
        {
            Name = global;
        }

        public override int GetPopCount() => 1;
    }
}