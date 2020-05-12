using Shockky.IO;

namespace Shockky.Chunks.Cast
{
    public class ScriptCastProperties : ShockwaveItem, ICastProperties
    {
        public short ScriptNumber { get; set; }

        public ScriptCastProperties()
        { }
        public ScriptCastProperties(ref ShockwaveReader input)
        {
            ScriptNumber = input.ReadInt16();
        }

        public override int GetBodySize()
        {
            int size = 0;
            size += sizeof(short);
            return size;
        }

        public override void WriteTo(ShockwaveWriter output)
        {
            output.Write(ScriptNumber);
        }
    }
}
