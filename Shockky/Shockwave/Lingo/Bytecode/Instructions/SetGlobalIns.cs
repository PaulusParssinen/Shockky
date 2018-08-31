using Shockky.IO;
namespace Shockky.Shockwave.Lingo.Bytecode.Instructions
{
    public class SetGlobalIns : Instruction
    {
        private string _value;
        public new string Value
        {
            get => _value;
            set
            {
                _value = value;
                _valueIndex = Pool.AddName(value);
            }
        }

        private int _valueIndex;
        public int ValueIndex
        {
            get => _valueIndex;
            set
            {
                _valueIndex = value;
                _value = Pool.NameList[value];
            }
        }

        public SetGlobalIns(LingoHandler handler)
            : base(OPCode.SetGlobal, handler)
        { }

        public SetGlobalIns(LingoHandler handler, string global)
            : this(handler)
        {
            Value = global;
        }

        public SetGlobalIns(LingoHandler handler, ShockwaveReader input, byte opByte)
            : base(OPCode.SetGlobal, handler, input, opByte)
        { }

        public override int GetPopCount() => 1;
    }
}