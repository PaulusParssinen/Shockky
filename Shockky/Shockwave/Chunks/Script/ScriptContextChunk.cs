using System.Collections.Generic;
using System.Linq;
using Shockky.IO;

namespace Shockky.Shockwave.Chunks
{
    public class ScriptContextChunk : ChunkItem
    {
        public List<ScriptContextSection> Sections { get; }
        public List<string> HandlerNames { get; set; }

        public int NameTableSectionIndex { get; set; }

        public ScriptContextChunk(ShockwaveReader input, ChunkEntry entry)
            : base(entry.Header)
        {
            int unk0 = input.ReadBigEndian<int>();
            int unk1 = input.ReadBigEndian<int>();
            int entryCount = input.ReadBigEndian<int>();
            int entryCount2 = input.ReadBigEndian<int>();
            
            short entryOffset = input.ReadBigEndian<short>();
            short entrySize = input.ReadBigEndian<short>(); //CONST 12 i guess

            int unk3 = input.ReadBigEndian<int>(); //0

            int fileType = input.ReadBigEndian<int>(); // TODO: Enum in future
            int unk5 = input.ReadBigEndian<int>();
            NameTableSectionIndex = input.ReadBigEndian<int>(); //So there can be multiple nametable chunks in a movie and this specifies which of those this scriptcontext uses

            short validCount = input.ReadBigEndian<short>();
            byte[] flags = input.ReadBytes(2);
            short freePointer = input.ReadBigEndian<short>();

            int handlerNameCount = (entryOffset - (int)input.Position) / 2; //TODO: Yeah this sketchy, idk whatsup here yet
            var list = input.ReadBigEndianList<short>(handlerNameCount); //Map to namelist?

            input.Position = entryOffset;

            Sections = new List<ScriptContextSection>(entryCount);
            for (int i = 0; i < entryCount; i++)
            {
                Sections.Add(new ScriptContextSection(input));
            }
        }

        public override void WriteTo(ShockwaveWriter output)
        {
            throw new System.NotImplementedException();
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
            size += Sections.Sum(s => s.GetBodySize());
            return size;
        }
    }
}
