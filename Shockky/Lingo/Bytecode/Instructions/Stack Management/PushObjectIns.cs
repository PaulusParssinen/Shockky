namespace Shockky.Lingo.Bytecode.Instructions
{
    public class PushObjectIns : Instruction
    {
        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                base.Value = _nameIndex = Pool.AddName(value);
            }
        }

        private int _nameIndex;
        public int NameIndex
        {
            get => _nameIndex;
            set
            {
                base.Value = value;
                _nameIndex = value;
                _name = Pool.NameList[value];
            }
        }

        public PushObjectIns(LingoHandler handler)
            : base(OPCode.PushObject, handler)
        { }
        public PushObjectIns(LingoHandler handler, int nameIndex)
            : this(handler)
        {
            NameIndex = nameIndex;
        }
        public PushObjectIns(LingoHandler handler, string name)
            : this(handler)
        {
            Name = name;
        }

        public override int GetPushCount() => 1;
    }
}
