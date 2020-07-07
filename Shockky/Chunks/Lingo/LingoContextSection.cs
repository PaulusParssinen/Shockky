using Shockky.IO;

namespace Shockky.Chunks
{
    public class LingoContextSection : ShockwaveItem
    {
        public int Unknown { get; set; }
        public int Id { get; set; }
        public LingoContextSectionFlags Flags { get; set; }
        public short Link { get; set; }

        public LingoContextSection()
        { }
        public LingoContextSection(ref ShockwaveReader input)
        {
            Unknown = input.ReadInt32();
            Id = input.ReadInt32();
            Flags = (LingoContextSectionFlags)input.ReadInt16();
            Link = input.ReadInt16();
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
            output.Write(Unknown);
            output.Write(Id);
            output.Write((short)Flags);
            output.Write(Link);
        }
    }
}