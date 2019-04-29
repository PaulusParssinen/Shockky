namespace Shockky.Lingo.Bytecode.Instructions
{
    public class GetParameterIns : VariableReference
    {
        private int _nameIndex;
        public int NameIndex
        {
            get => _nameIndex;
            set
            {
                _nameIndex = value;
                Name = Pool.NameList[Handler.Arguments[value]];
            }
        }

        public GetParameterIns(LingoHandler handler)
            : base(OPCode.GetParameter, handler)
        { }
        public GetParameterIns(LingoHandler handler, int argumentNameIndex)
            : this(handler)
        {
            NameIndex = argumentNameIndex;
            //TODO: index under int16 in namelist with this one too
        }

        protected override int SetName(string name)
          => Handler.Arguments[Value] = (short)Pool.AddName(name);
    }
}