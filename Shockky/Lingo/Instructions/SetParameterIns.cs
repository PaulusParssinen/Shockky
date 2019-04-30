namespace Shockky.Lingo.Bytecode.Instructions
{
    public class SetParameterIns : VariableAssignment
    {
        private int _argumentNameIndex;
        public int ArgumentNameIndex
        {
            get => _argumentNameIndex;
            set
            {
                _argumentNameIndex = value;
                Name = Pool.NameList[Handler.Arguments[value]];
            }
        }

        public SetParameterIns(LingoHandler handler)
            : base(OPCode.SetParameter, handler)
        { }
        public SetParameterIns(LingoHandler handler, int argumentNameIndex)
            : this(handler)
        {
            ArgumentNameIndex = argumentNameIndex;
            //TODO: index under int16 in namelist with this one too
        }

        protected override int SetName(string name)
            => Handler.Arguments[Value] = (short)Pool.AddName(name);
    }
}