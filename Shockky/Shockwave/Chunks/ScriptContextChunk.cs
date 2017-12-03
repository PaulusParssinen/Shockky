using System.Collections.Generic;
using Shockky.IO;

namespace Shockky.Shockwave.Chunks
{
    public class ScriptContextChunk : ChunkItem
    {
        public List<ScriptContextSection> Sections { get; set; }

        public int NameTableSectionId { get; set; }

        public ScriptContextChunk(ShockwaveReader input, ChunkEntry entry)
            : base(entry.Header)
        {
            int unk0 = input.ReadInt32(true);
            int unk1 = input.ReadInt32(true);
            int entryCount = input.ReadInt32(true);
            int entryCount2 = input.ReadInt32(true);
            
            short entryOffset = input.ReadInt16(true);
            short entrySize = input.ReadInt16(true); //CONST

            int unk3 = input.ReadInt32(true); //0

            int fileType = input.ReadInt32(true); // TODO: Enum in future
            int unk5 = input.ReadInt32(true);
            NameTableSectionId = input.ReadInt32(true); //So there can be multiple nametable chunks in a movie/dxr and this specifies which of those this scriptcontext uses

            short validCount = input.ReadInt16(true);
            byte[] flags = input.ReadBytes(2);
            short freePointer = input.ReadInt16(true);

            int handlerNameCount = (entryOffset - (int)input.Position) / 2; //Yeah this sketchy, idk whatsup here yet
            var list = input.ReadShortList(handlerNameCount);

            Sections = input.ReadList<ScriptContextSection>( //aight so thsi ScriptContext chunk contains _usually_ few scriptsections which again tell where the "ScriptChunks" are located, 
                entryCount, entryOffset);
        }
    }
}
