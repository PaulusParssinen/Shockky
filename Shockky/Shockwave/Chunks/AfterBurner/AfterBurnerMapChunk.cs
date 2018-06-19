using System.Collections.Generic;
using Shockky.IO;
using Shockky.Shockwave.Chunks.Interface;

namespace Shockky.Shockwave.Chunks
{
    public class AfterburnerMapChunk : CompressedChunkItem, IChunkEntryMap
    {
        public byte[] UnknownData { get; }

        public int Unknown1 { get; set; }
        public int Unknown2 { get; set; }

        public List<IChunkEntry> Entries { get; set; }

        public AfterburnerMapChunk(ShockwaveReader input, ChunkHeader header)
            : base(ref input, header, false)
        {
            UnknownData = input.ReadBytes(3); //TODO: Wthell
            input = WrapDecompressor(input);
            Unknown1 = input.Read7BitEncodedInt();
            Unknown2 = input.Read7BitEncodedInt();

            Entries = new List<IChunkEntry>(input.Read7BitEncodedInt());
            for(int i = 0; i < Entries.Capacity; i++)
            {
                Entries.Add(new AfterBurnerMapEntry(input));
            }
        }

        public override void WriteBodyTo(ShockwaveWriter output)
        {
            output.Write(UnknownData);
            //TODO: Wrap dat compressor
            output.Write7BitEncodedInt(Unknown1);
            output.Write7BitEncodedInt(Unknown2);
            output.Write7BitEncodedInt(Entries.Count);
            foreach (var entry in Entries)
                entry.WriteTo(output);
        }
    }
}
