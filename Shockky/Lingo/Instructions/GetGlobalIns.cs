namespace Shockky.Lingo.Bytecode.Instructions
{
    public class GetGlobalIns : VariableReference
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

        public GetGlobalIns(LingoHandler handler)
            : base(OPCode.GetGlobal, handler)
        { }
        public GetGlobalIns(LingoHandler handler, int globalNameIndex)
            : this(handler)
        {
            ValueIndex = globalNameIndex;
        }
        public GetGlobalIns(LingoHandler handler, string global)
            : this(handler)
        {
            Name = global;
        }

        protected override int SetName(string name) => Pool.AddName(name);
    }
}