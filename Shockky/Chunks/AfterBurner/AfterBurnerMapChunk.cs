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
            Remnants.Enqueue(input.ReadVarInt());

            using DeflateShockwaveReader deflaterInput = CreateDeflateReader(ref input);
            Remnants.Enqueue(deflaterInput.ReadVarInt());
            Remnants.Enqueue(deflaterInput.ReadVarInt());

            Entries = new AfterBurnerMapEntry[deflaterInput.ReadVarInt()];
            for (int i = 0; i < Entries.Length; i++)
            {
                Entries[i] = new AfterBurnerMapEntry(deflaterInput);
            }
        }

        public override void WriteBodyTo(ShockwaveWriter output)
        {
            output.Write((byte)0);
            output.WriteVarInt((int)Remnants.Dequeue());
            //TODO: Wrap dat compressor
            output.WriteVarInt((int)Remnants.Dequeue());
            output.WriteVarInt((int)Remnants.Dequeue());

            output.WriteVarInt(Entries.Length);
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
