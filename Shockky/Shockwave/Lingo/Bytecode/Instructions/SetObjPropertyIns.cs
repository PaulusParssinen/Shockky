using Shockky.IO;
using Shockky.Shockwave.Lingo.Bytecode.Instructions.Enum;

namespace Shockky.Shockwave.Lingo.Bytecode.Instructions
{
    public class SetObjPropertyIns : Instruction
    {

        public SetObjPropertyIns(LingoHandler handler)
            : base(OPCode.SetObjProp, handler)
        { }
        public SetObjPropertyIns(LingoHandler handler, string objProperty)
            : this(handler)
        { }

        public SetObjPropertyIns(LingoHandler handler, ShockwaveReader input, byte opByte)
            : base(OPCode.SetObjProp, handler, input, opByte)
        { }

        public override int GetPopCount() => 1; //or2

        /*   public override void Translate()
           {
               var value = Handler.Expressions.Pop();
               var obj = Handler.Expressions.Pop();
           }*/
    }
}
