using System.Text;

using Shockky.IO;

namespace Shockky.Shockwave.Chunks.Cast
{
    public class CastListEntry : ShockwaveItem
    {
        public string Name { get; set; }
        public string FilePath { get; set; }

        public short PreloadSettings { get; set; }
        public short StorageType { get; set; }
        public short MemberCount { get; set; }

        public int Id { get; set; }

        public CastListEntry(ShockwaveReader input)
        {
            Name = input.ReadString();
            FilePath = input.ReadString();

            PreloadSettings = input.ReadBigEndian<short>();
            StorageType = input.ReadBigEndian<short>();
            MemberCount = input.ReadBigEndian<short>();

            Id = input.ReadBigEndian<int>();
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
            output.WriteBigEndian(PreloadSettings);
            output.WriteBigEndian(StorageType);
            output.WriteBigEndian(MemberCount);
            output.WriteBigEndian(Id);
        }
    }
}
