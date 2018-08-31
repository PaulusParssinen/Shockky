using Shockky.IO;
namespace Shockky.Shockwave.Lingo.Bytecode.Instructions
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
        public SetPropertyIns(LingoHandler handler, ShockwaveReader input, byte opByte)
            : base(OPCode.SetProperty, handler, input, opByte)
        { }

        public override int GetPopCount() => 1;
    }
}