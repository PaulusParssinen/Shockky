using Shockky.IO;
using Shockky.Shockwave.Lingo.Bytecode.Instructions.Enum;

namespace Shockky.Shockwave.Lingo.Bytecode.Instructions
{
    public class GetObjPropertyIns : Instruction
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

        public GetObjPropertyIns(LingoHandler handler)
            : base(OPCode.GetObjProp, handler)
        { }
        public GetObjPropertyIns(LingoHandler handler, string objPropertyName)
            : this(handler)
        {
            Value = objPropertyName;
        }
        public GetObjPropertyIns(LingoHandler handler, ShockwaveReader input, byte opByte)
            : base(OPCode.GetObjProp, handler, input, opByte)
        { }
    }
}
