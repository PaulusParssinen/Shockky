using Shockky.IO;

namespace Shockky.Shockwave.Lingo.Bytecode.Instructions
{
    public class NewListIns : Instruction
    {
        public int ItemCount { get; } 

        public NewListIns(LingoHandler handler, ShockwaveReader input, byte opByte, bool argList)
            : base(OPCode.NewList, handler, input, opByte)
        {
            ItemCount = Value; //not sure why I didnt implement short reading here
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