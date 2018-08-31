using Shockky.IO;
namespace Shockky.Shockwave.Lingo.Bytecode.Instructions
{
    public class SetLocalIns : Instruction
    {
        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                //TODO: Locals recycling
                Handler.Locals[Value] = (short)Pool.AddName(value);
            }
        }
        
        public int NameIndex
        {
            get => Handler.Locals[Value];
            set
            {
                Value = value;
                _name = Pool.NameList[Handler.Locals[Value]];
            }
        }

        public SetLocalIns(LingoHandler handler)
            : base(OPCode.SetLocal, handler)
        { }
        public SetLocalIns(LingoHandler handler, string local)
            : this(handler)
        {
            Name = local;
        }
        public SetLocalIns(LingoHandler handler, ShockwaveReader input, byte opByte)
            : base(OPCode.SetLocal, handler, input, opByte)
        { }
        
        public override int GetPopCount() => 1;
    }
}