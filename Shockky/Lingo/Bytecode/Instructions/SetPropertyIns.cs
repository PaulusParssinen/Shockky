namespace Shockky.Lingo.Bytecode.Instructions
{
    public class SetPropertyIns : Instruction
    {
        public int NameIndex => Value;
        public string Name => Pool.NameList[Pool.Properties[NameIndex]];

        public SetPropertyIns(LingoHandler handler)
            : base(OPCode.SetProperty, handler)
        { }
        public SetPropertyIns(LingoHandler handler, int propertyNameIndex)
            : this(handler)
        {
            Value = propertyNameIndex;
        }

        public override int GetPopCount() => 1;
    }
}