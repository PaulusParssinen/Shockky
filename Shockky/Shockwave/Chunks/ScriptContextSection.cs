using System.Diagnostics;
using Shockky.IO;

namespace Shockky.Shockwave.Chunks
{
    [DebuggerDisplay("SectionId: {SectionId}")]
    public class ScriptContextSection : ShockwaveItem
    {
        public int Unknown0 { get; set; }
        public int SectionId { get; set; }
        public bool Used { get; set; }
        public short Link { get; set; }

        public ScriptContextSection(ref ShockwaveReader input)
        {
            Unknown0 = input.ReadInt32(true);
            SectionId = input.ReadInt32(true);
            Used = input.ReadInt16(true) == 4;
            Link = input.ReadInt16(true); //TODO: If unused, link to next unused 
        }
    }
}
