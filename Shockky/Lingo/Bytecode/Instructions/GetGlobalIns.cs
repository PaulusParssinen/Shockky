using Shockky.IO;

namespace Shockky.Lingo.Bytecode.Instructions
{
    public class GetGlobalIns : Instruction
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
                base.Value = value;
                _valueIndex = value;
                _value = Pool.NameList[value];
            }
        }

        public GetGlobalIns(LingoHandler handler)
            : base(OPCode.GetGlobal, handler)
        { }
        public GetGlobalIns(LingoHandler handler, int globalNameIndex)
            : this(handler)
        {
            ValueIndex = globalNameIndex;
        }
        public GetGlobalIns(LingoHandler handler, string global)
            : this(handler)
        {
            Value = global;
        }
    }
}