namespace Shockky.Lingo.Bytecode.Instructions
{
    public class GetParameterIns : Instruction
    {
        public int NameIndex => Value;
        public string Name => Pool.NameList[Handler.Arguments[NameIndex]];

        public GetParameterIns(LingoHandler handler)
            : base(OPCode.GetParameter, handler)
        { }
        public GetParameterIns(LingoHandler handler, int argumentNameIndex)
            : this(handler)
        {
            Value = argumentNameIndex;
            //TODO: index under int16 in namelist with this one too
        }
        public override int GetPushCount() => 1;
    }
}