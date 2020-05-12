using Shockky.IO;

namespace Shockky.Chunks
{
    public class ScriptContextSection : ShockwaveItem
    {
        public int Unknown { get; set; }
        public int Id { get; set; }
        public short Flags { get; set; }
        public short Link { get; set; }

        public ScriptContextSection()
        { }
        public ScriptContextSection(ref ShockwaveReader input)
        {
            Unknown = input.ReadInt32();
            Id = input.ReadInt32();
            Flags = input.ReadInt16(); // 4=Used
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
            output.Write(Flags);
            output.Write(Link);
        }
    }
}