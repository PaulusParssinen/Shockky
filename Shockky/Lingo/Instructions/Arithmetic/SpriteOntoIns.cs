using System;

namespace Shockky.Lingo.Instructions
{
    public class SpriteOntoIns : Computation
    {
        public SpriteOntoIns()
            : base(OPCode.OntoSprite, BinaryOperatorKind.SpriteIntersects)
        { }

        protected override object Execute(object left, object right) => throw new NotSupportedException();    
    }
}
