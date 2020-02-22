using System;

namespace Shockky.Lingo.Instructions
{
    public class OntoSpriteIns : Computation
    {
        public OntoSpriteIns()
            : base(OPCode.OntoSprite, BinaryOperatorKind.SpriteIntersects)
        { }

        protected override object Execute(object left, object right) => throw new NotSupportedException();
    }
}
