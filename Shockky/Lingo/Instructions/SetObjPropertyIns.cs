namespace Shockky.Lingo.Bytecode.Instructions
{
    public class SetObjPropertyIns : Instruction
    {
        public SetObjPropertyIns(LingoHandler handler)
            : base(OPCode.SetObjProp, handler)
        { }
        public SetObjPropertyIns(LingoHandler handler, int propertyNameIndex)
            : this(handler)
        {
            Value = propertyNameIndex;
        }

        public override int GetPopCount() => 1; //or2

        /*   public override void Translate()
           {
               var value = Handler.Expressions.Pop();
               var obj = Handler.Expressions.Pop();
           }*/
    }
}