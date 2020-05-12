using System.Text;

using Shockky.IO;

namespace Shockky.Chunks.Cast
{
    public class CastListEntry : ShockwaveItem
    {
        public string Name { get; set; }
        public string FilePath { get; set; }

        public short PreloadSettings { get; set; }
        public short MemberMin { get; set; }
        public short MemberCount { get; set; }

        public int Id { get; set; }

        public CastListEntry()
        { }
        public CastListEntry(ref ShockwaveReader input)
        {
            Name = input.ReadString();
            FilePath = input.ReadString();

            PreloadSettings = input.ReadInt16();
            MemberMin = input.ReadInt16();
            MemberCount = input.ReadInt16();

            Id = input.ReadInt32();
        }

        public override int GetBodySize()
        {
            int size = 0;
            size += Encoding.UTF8.GetByteCount(Name) + 1;
            size += Encoding.UTF8.GetByteCount(FilePath) + 1;
            size += sizeof(short);
            size += sizeof(short);
            size += sizeof(short);
            size += sizeof(int);
            return size;
        }
        public override void WriteTo(ShockwaveWriter output)
        {
            output.Write(Name);
            output.Write(FilePath);
            output.Write(PreloadSettings);
            output.Write(MemberMin);
            output.Write(MemberCount);
            output.Write(Id);
        }
    }
}
