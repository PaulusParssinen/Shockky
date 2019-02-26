using Shockky.IO;

namespace Shockky.Chunks
{
    public class ScriptContextSection : ShockwaveItem
    {
        public int Unknown { get; set; }
        public int Id { get; set; }
        public short Flags { get; set; }
        public short Link { get; set; }

        public ScriptContextSection(ShockwaveReader input)
        {
            Unknown = input.ReadBigEndian<int>();
            Id = input.ReadBigEndian<int>();
            Flags = input.ReadBigEndian<short>(); // 4=Used
            Link = input.ReadBigEndian<short>();
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
            output.WriteBigEndian(Unknown);
            output.WriteBigEndian(Id);
            output.WriteBigEndian(Flags);
            output.WriteBigEndian(Link);
        }
    }
}