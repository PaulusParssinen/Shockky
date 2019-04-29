namespace Shockky.Lingo.Bytecode.Instructions
{
    public class GetLocalIns : VariableReference
    {
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

        public GetLocalIns(LingoHandler handler)
            : base(OPCode.GetLocal, handler)
        { }

        public GetLocalIns(LingoHandler handler, int localIndex)
            : this(handler)
        {
            LocalNameIndex = localIndex;
            //Handler.Locals.Add, also adjust its index to be under int16 in namelist
        }

        protected override int SetName(string name)
            => Handler.Locals[Value] = (short)Pool.AddName(name); //TODO: uhoh int16 & recycling + index tracking
    }
}