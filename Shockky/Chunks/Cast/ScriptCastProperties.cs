using System;
using System.Drawing;

using Shockky.IO;

namespace Shockky.Chunks.Cast
{
    public class ScriptCastProperties : ICastTypeProperties
    {
        public short ScriptNumber { get; set; }

        public ScriptCastProperties()
        { }
        public ScriptCastProperties(ShockwaveReader input)
        {
            ScriptNumber = input.ReadBigEndian<short>();
        }

        public int GetBodySize()
        {
            int size = 0;
            size += sizeof(short);
            return size;
        }

        public void WriteTo(ShockwaveWriter output)
        {
            output.WriteBigEndian(ScriptNumber);
        }

        public Rectangle Rectangle {
            get => throw new NotSupportedException();
            set => throw new NotSupportedException();
        }
    }
}
