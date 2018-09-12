namespace Shockky.Shockwave.Lingo.Bytecode.Instructions
{
    public class SetParameterIns : Instruction
    {
        public int NameIndex => Value;
        public string Name => Pool.NameList[Handler.Arguments[NameIndex]];

        public SetParameterIns(LingoHandler handler)
            : base(OPCode.SetParameter, handler)
        { }
        public SetParameterIns(LingoHandler handler, int argumentNameIndex)
            : this(handler)
        {
            Value = argumentNameIndex;
            //TODO: index under int16 in namelist with this one too
        }

        public override int GetPopCount() => 1;
    }
}