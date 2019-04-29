namespace Shockky.Lingo.Bytecode.Instructions
{
    public class SetLocalIns : VariableAssignment
    {
        //TODO: The local referencing is fked up here
        private int _localNameIndex;
        public int LocalNameIndex
        {
            get => _localNameIndex;
            set
            {
                _localNameIndex = value;
                Name = Pool.NameList[Handler.Locals[value]];
            }
        }

        public SetLocalIns(LingoHandler handler)
            : base(OPCode.SetLocal, handler)
        { }
        public SetLocalIns(LingoHandler handler, int localIndex)
            : this(handler)
        {
            LocalNameIndex = localIndex;
        }
        public SetLocalIns(LingoHandler handler, string local)
            : this(handler)
        {
            Name = local;
        }

        protected override int SetName(string name)
            => Handler.Locals[Value] = (short)Pool.AddName(name); //TODO: uhoh int16 & recycling + index tracking
    }
}