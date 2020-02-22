using System;

namespace Shockky.Lingo.Instructions
{
    public class IntoSpriteIns : Computation
    {
        public IntoSpriteIns()
            : base(OPCode.IntoSprite, BinaryOperatorKind.SpriteWithin)
        { }

        protected override object Execute(object left, object right) => throw new NotSupportedException();
    }
}