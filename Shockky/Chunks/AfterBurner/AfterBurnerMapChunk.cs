using Shockky.IO;

namespace Shockky.Chunks
{
    public unsafe class AfterburnerMapChunk : ChunkItem
    {
        public AfterBurnerMapEntry[] Entries { get; set; }

        public AfterburnerMapChunk()
            : base(ChunkKind.ABMP)
        { }
        public AfterburnerMapChunk(ref ShockwaveReader input, ChunkHeader header)
            : base(header)
        {
            input.ReadByte();
            Remnants.Enqueue(input.Read7BitEncodedInt());

            using var deflaterInput = CreateDeflateReader(ref input);
            Remnants.Enqueue(deflaterInput.Read7BitEncodedInt());
            Remnants.Enqueue(deflaterInput.Read7BitEncodedInt());

            Entries = new AfterBurnerMapEntry[deflaterInput.Read7BitEncodedInt()];
            for (int i = 0; i < Entries.Length; i++)
            {
                Entries[i] = new AfterBurnerMapEntry(deflaterInput);
            }
        }

        public override void WriteBodyTo(ShockwaveWriter output)
        {
            output.Write((byte)0);
            output.Write7BitEncodedInt((int)Remnants.Dequeue());
            //TODO: Wrap dat compressor
            output.Write7BitEncodedInt((int)Remnants.Dequeue());
            output.Write7BitEncodedInt((int)Remnants.Dequeue());

            output.Write7BitEncodedInt(Entries.Length);
            foreach (var entry in Entries)
            {
                entry.WriteTo(output);
            }
        }

        public override int GetBodySize()
        {
            throw new System.NotImplementedException();
            int size = 0;
            size += sizeof(byte);
            return size;
        }
    }
}
