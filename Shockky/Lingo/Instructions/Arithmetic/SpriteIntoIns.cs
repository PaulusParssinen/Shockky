using System;

namespace Shockky.Lingo.Instructions
{
    public class SpriteIntoIns : Computation
    {
        public SpriteIntoIns()
            : base(OPCode.IntoSprite, BinaryOperatorKind.SpriteWithin)
        { }

        protected override object Execute(object left, object right) => throw new NotSupportedException();
    }
}