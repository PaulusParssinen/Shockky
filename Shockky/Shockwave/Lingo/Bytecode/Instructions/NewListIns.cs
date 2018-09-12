namespace Shockky.Shockwave.Lingo.Bytecode.Instructions
{
    public class NewListIns : Instruction
    {
        public int ItemCount => Value;

        public NewListIns(bool argList)
            : base(argList ? OPCode.NewArgList : OPCode.NewList)
        { }
        public NewListIns(LingoHandler handler, bool argList)
            : base(argList ? OPCode.NewArgList : OPCode.NewList, handler)
        { }
        public NewListIns(LingoHandler handler, int itemCount, bool argList)
            : this(handler, argList)
        {
            Value = itemCount;
        }

        public override int GetPushCount() => ItemCount;

     /*   public override void Translate()
        {
            Items = Handler.Expressions.Take(ItemsCount).ToList();
        }

        public string ToString(bool brackets)
            
        {
            string listStr = string.Join(", ", Items);

            return brackets ? '[' + listStr + ']' : listStr;
        }*/
    }
}