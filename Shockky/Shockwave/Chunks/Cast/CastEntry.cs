using Shockky.IO;

namespace Shockky.Shockwave.Chunks.Cast
{
    public class CastEntry : ShockwaveItem
    {
        public int Id { get; set; }
        public int OwnerId { get; set; }

        public string Name { get; set; }
        public ChunkKind Kind => Name.ToChunkKind();

        public CastEntry(ShockwaveReader input)
        {
            Id = input.ReadInt32();
            OwnerId = input.ReadInt32();

	        Name = input.ReadReversedString(4);
        }

        public override int GetBodySize()
        {
            int size = 0;
            size += sizeof(int);
            size += sizeof(int);
            size += 4;
            return size;
        }

        public override void WriteTo(ShockwaveWriter output)
        {
            output.Write(Id);
            output.Write(OwnerId);
            output.WriteReversedString(Name);
        }
    }
}
