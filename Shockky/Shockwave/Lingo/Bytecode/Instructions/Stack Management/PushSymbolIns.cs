using Shockky.IO;
using Shockky.Shockwave.Lingo.Bytecode.Instructions.Enum;
using Shockky.Shockwave.Lingo.Enum;

namespace Shockky.Shockwave.Lingo.Bytecode.Instructions
{
    public class PushSymbolIns : Instruction
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

        public PushSymbolIns(LingoHandler handler)
            : base(OPCode.PushSymbol, handler)
        { }
        public PushSymbolIns(LingoHandler handler, string name)
            : this(handler)
        {
            Name = name;
        }

        public PushSymbolIns(LingoHandler handler, ShockwaveReader input, byte opByte)
            : base(OPCode.PushSymbol, handler, input, opByte)
        {
            NameIndex = base.Value;
        }

        public override int GetPushCount() => 1;
    }
}
