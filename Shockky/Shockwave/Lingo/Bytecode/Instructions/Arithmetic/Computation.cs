using Shockky.Shockwave.Lingo.Bytecode.Instructions.Enum;

namespace Shockky.Shockwave.Lingo.Bytecode.Instructions
{
    public abstract class Computation : Instruction
    {
        public string Translation { get; } //TODO: maybe rename idk

        public Instruction Right { get; private set; }
        public Instruction Left { get; private set; }

        protected Computation(OPCode op)
            : base(op)
        { }

        public override int GetPopCount()
        {
            return 2;
        }

        public override int GetPushCount()
        {
            return 1;
        }

   /*     public override void Execute(LingoMachine machine)
        {
            object right = machine.Values.Pop();
            object left = machine.Values.Pop();
                            
            object result = null;
            if (left != null && right != null)
            {
                result = Execute(left, right);
            }
            machine.Values.Push(result);
        }
        protected abstract object Execute(object left, object right);
		*/
        public static bool IsValid(OPCode op) => true; //TODO:
    }
}
