using System.Collections.Generic;
using System;

using Shockky.IO;

namespace Shockky.Shockwave.Chunks
{
    public class ScriptContextChunk : ChunkItem
    {
        private const short SECTION_SIZE = 12;

        public List<ScriptContextSection> Sections { get; }

        public int Type { get; set; }
        public int NameListChunkId { get; set; }

        public ScriptContextChunk(ShockwaveReader input, ChunkHeader header)
            : base(header)
        {
            Remnants.Enqueue(input.ReadBigEndian<int>());
            Remnants.Enqueue(input.ReadBigEndian<int>());

            int entryCount = input.ReadBigEndian<int>();
            input.ReadBigEndian<int>();
            
            short entryOffset = input.ReadBigEndian<short>(); //TODO: Research
            input.ReadBigEndian<short>();

            Remnants.Enqueue(input.ReadBigEndian<int>()); //0
            Type = input.ReadBigEndian<int>(); //TODO: Enum
            Remnants.Enqueue(input.ReadBigEndian<int>());

            NameListChunkId = input.ReadBigEndian<int>();

            Remnants.Enqueue(input.ReadBigEndian<short>()); //validCount
            Remnants.Enqueue(input.ReadBytes(2)); //flags, short?
            Remnants.Enqueue(input.ReadBigEndian<short>()); //freePtr

            input.Position = Header.Offset + entryOffset;

            Sections = new List<ScriptContextSection>(entryCount);
            for (int i = 0; i < entryCount; i++)
            {
                Sections.Add(new ScriptContextSection(input));
            }
        }

        public override void WriteBodyTo(ShockwaveWriter output)
        {
            const short ENTRY_OFFSET = 48;

            output.WriteBigEndian((int)Remnants.Dequeue());
            output.WriteBigEndian((int)Remnants.Dequeue());
            output.WriteBigEndian(Sections?.Count ?? 0); //TODO:
            output.WriteBigEndian(Sections?.Count ?? 0); //TODO:

            output.WriteBigEndian(ENTRY_OFFSET);
            output.WriteBigEndian(SECTION_SIZE);

            output.WriteBigEndian((int)Remnants.Dequeue());
            output.WriteBigEndian(Type);
            output.WriteBigEndian((int)Remnants.Dequeue());

            output.WriteBigEndian(NameListChunkId);

            output.WriteBigEndian((short)Remnants.Dequeue());
            output.Write((byte[])Remnants.Dequeue());
            output.WriteBigEndian((short)Remnants.Dequeue());

            foreach (ScriptContextSection section in Sections)
            {
                section.WriteTo(output);
            }
        }

        public override int GetBodySize()
        {
            int size = 0;
            size += sizeof(int);
            size += sizeof(int);
            size += sizeof(int);
            size += sizeof(int);
            size += sizeof(short);
            size += sizeof(short);
            size += sizeof(int);
            size += sizeof(int);
            size += sizeof(int);
            size += sizeof(int);
            size += sizeof(short);
            size += 2;
            size += sizeof(short);
            size += Sections.Count * SECTION_SIZE;
            return size;
        }
    }
}
