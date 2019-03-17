using System.Collections.Generic;

using Shockky.IO;

namespace Shockky.Chunks
{
    public class AfterburnerMapChunk : ChunkItem
    {
        public List<AfterBurnerMapEntry> Entries { get; set; }

        public AfterburnerMapChunk(ShockwaveReader input, ChunkHeader header)
            : base(header)
        {
            input.ReadByte();
            Remnants.Enqueue(input.Read7BitEncodedInt());

            var decompressedInput = WrapDecompressor(input);
            Remnants.Enqueue(decompressedInput.Read7BitEncodedInt());
            Remnants.Enqueue(decompressedInput.Read7BitEncodedInt());

            Entries = new List<AfterBurnerMapEntry>(decompressedInput.Read7BitEncodedInt());
            for (int i = 0; i < Entries.Capacity; i++)
            {
                Entries.Add(new AfterBurnerMapEntry(decompressedInput));
            }
        }

        public override void WriteBodyTo(ShockwaveWriter output)
        {
            output.Write((byte)0);
            output.Write7BitEncodedInt((int)Remnants.Dequeue());
            //TODO: Wrap dat compressor
            output.Write7BitEncodedInt((int)Remnants.Dequeue());
            output.Write7BitEncodedInt((int)Remnants.Dequeue());

            output.Write7BitEncodedInt(Entries.Count);
            foreach (var entry in Entries)
            {
                output.Write(entry);
            }
        }

        public override int GetBodySize()
        {
            throw new System.NotImplementedException();
            int size = 0;
            size += sizeof(byte);
            //TODO:
            return size;
        }
    }
}
