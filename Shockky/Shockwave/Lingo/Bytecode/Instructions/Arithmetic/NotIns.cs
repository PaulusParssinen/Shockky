using Shockky.Shockwave.Lingo.Bytecode.Instructions.Enum;

namespace Shockky.Shockwave.Lingo.Bytecode.Instructions.Arithmetic
{
    public class NotIns : Instruction
    {
        public NotIns(LingoHandler handler) 
            : base(OPCode.Not, handler)
        { }

        public override int GetPopCount()
        {
            return 1;
        }

        public override int GetPushCount()
        {
            return 1;
        }

      /*  public override void Execute(LingoMachine machine)
        {
            bool value = (bool)machine.Values.Pop(); //TODO: idk it might be a function or shit that is passed ot dis..

            machine.Values.Push(!value);
        }*/
    }
}
