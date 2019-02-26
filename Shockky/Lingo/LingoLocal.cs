using Shockky.IO;
using Shockky.Chunks;

namespace Shockky.Lingo
{
    public class LingoLocal : LingoItem
    {
        public int NameIndex { get; set; }
        public string Name => Script.Pool.GetName(NameIndex);

        public LingoLocal(ScriptChunk script)
            : base(script)
        { }

        public override int GetBodySize() => sizeof(short);

        public override void WriteTo(ShockwaveWriter output)
        {
            output.WriteBigEndian((short)NameIndex);
        }

        protected override string DebuggerDisplay => ToString();
        public override string ToString() => Name;
    }
}