namespace Shockky.Lingo.Bytecode.Instructions
{
    public class GetPropertyIns : Instruction
    {
        public int NameIndex => Value;
        public string Name => Pool.NameList[NameIndex];

        public GetPropertyIns(LingoHandler handler)
            : base(OPCode.GetProperty, handler)
        { }
        public GetPropertyIns(LingoHandler handler, int propertyNameIndex)
            : this(handler)
        {
            Value = propertyNameIndex;
        }

        public override int GetPushCount() => 1;
    }
}