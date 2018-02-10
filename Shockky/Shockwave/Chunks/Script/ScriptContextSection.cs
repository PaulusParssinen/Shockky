using System;
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

        public ScriptContextSection(ShockwaveReader input)
        {
            Unknown0 = input.ReadBigEndian<int>();
            SectionId = input.ReadBigEndian<int>();
            Used = input.ReadBigEndian<short>() == 4;
            Link = input.ReadBigEndian<short>(); //TODO: If not used, link to next unused !?
        }

        public override int GetBodySize()
        {
            int size = 0;
            size += sizeof(int);
            size += sizeof(int);
            size += sizeof(short);
            size += sizeof(short);
            return size;
        }

        public override void WriteTo(ShockwaveWriter output)
        {
            output.Write(Unknown0);
            output.Write(SectionId);
            output.Write(Used);
            output.Write(Link);
        }
    }
}