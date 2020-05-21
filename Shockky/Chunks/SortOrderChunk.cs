using Shockky.IO;

namespace Shockky.Chunks
{
    public class SortOrderChunk : ChunkItem
    {
        public (short unk, short unk2)[] Entries { get; set; } //TODO: castMemRef related. ref => ?

        public SortOrderChunk()
            : base(ChunkKind.Sord)
        { }
        public SortOrderChunk(ref ShockwaveReader input, ChunkHeader header)
            : base(header)
        {
            input.IsBigEndian = true;

            input.ReadInt32();
            input.ReadInt32();

            Entries = new (short unk, short unk2)[input.ReadInt32()];
            input.ReadInt32();
            
            input.ReadInt16();
            input.ReadInt16(); //TODO: dir <= 0x500 ? sizeof(short) : sizeof(short) * 2 

            for (int i = 0; i < Entries.Length; i++)
            {
                Entries[i] = (input.ReadInt16(), input.ReadInt16());
            }
        }

        public override int GetBodySize()
        {
            int size = 0;
            size += sizeof(int);
            size += sizeof(int);

            size += sizeof(int);

            size += sizeof(short);
            size += sizeof(short);

            size += sizeof(short) * 2 * Entries.Length;
            return size;
        }

        public override void WriteBodyTo(ShockwaveWriter output)
        {
            const short ENTRIES_OFFSET = 20;
            const short ENTRY_SIZE = sizeof(short) + sizeof(short);

            output.Write(0);
            output.Write(0);

            output.Write(Entries.Length);
            output.Write(Entries.Length);

            output.Write(ENTRIES_OFFSET);
            output.Write(ENTRY_SIZE);

            foreach ((short first, short second) in Entries)
            {
                output.Write(first);
                output.Write(second);
            }
        }
    }
}
