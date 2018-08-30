using Shockky.IO;

namespace Shockky.Shockwave.Chunks
{
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
            output.WriteBigEndian(Unknown0);
            output.WriteBigEndian(SectionId);
            output.WriteBigEndian(Used ? 4 : 0); // TODO:
            output.WriteBigEndian(Link);
        }
    }
}