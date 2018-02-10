using System.Collections.Generic;
using Shockky.IO;
using Shockky.Shockwave.Lingo.Bytecode.Instructions.Enum;

namespace Shockky.Shockwave.Lingo.Bytecode.Instructions
{
    public class NewListIns : Instruction
    {
        public int ItemsCount { get; } //TODO: Better name

        public List<Instruction> Items { get; private set; }

        public NewListIns(ShockwaveReader input, LingoHandler handler)
            : base(OPCode.NewList, handler)
        {
            ItemsCount = input.ReadByte(); //Pop this amount off the stack..
        }

        public override int GetPushCount()
        {
            return ItemsCount;
        }

     /*   public override void Translate()
        {
            Items = Handler.Expressions.Take(ItemsCount).ToList(); //TODO: ToList gayyy, perf
        }*/

        public string ToString(bool brackets)
        {
            string listStr = string.Join(", ", Items);

            return brackets ? '[' + listStr + ']' : listStr;
        }

        public override string ToString()
            => ToString(true);
    }
}
