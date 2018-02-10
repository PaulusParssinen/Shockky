using System;
using System.Collections.Generic;
using Shockky.IO;
using Shockky.Shockwave.Chunks.Enum;
using Shockky.Shockwave.Lingo;

namespace Shockky.Shockwave.Chunks
{
    public class ScriptChunk : ChunkItem
    {
        public List<ScriptChunkEntry> Entries { get; }
        public List<string> NameList { get; }

        public LingoScript Script { get; }

        public ScriptChunkEntry this[ScriptEntryType type]
            => Entries[(int) type];

        public ScriptChunk(ShockwaveReader input, ChunkEntry entry, List<string> nameList)
            : base(entry.Header)
        {
            NameList = nameList;

            int unknown1 = input.ReadBigEndian<int>();
            int unknown2 = input.ReadBigEndian<int>();
           
            int totalLength = input.ReadBigEndian<int>();
            int totalLength2 = input.ReadBigEndian<int>();

            short headerLength = input.ReadBigEndian<short>();
            short scriptNumber = input.ReadBigEndian<short>();
            
            input.Position = 38;
            int behaviourScript = input.ReadBigEndian<int>(); 

            input.Position = 50;

            Entries = new List<ScriptChunkEntry>
            {
                new ScriptChunkEntry(ScriptEntryType.HandlerVectors, input),
                new ScriptChunkEntry(ScriptEntryType.Properties, input),
                new ScriptChunkEntry(ScriptEntryType.Globals, input),
                new ScriptChunkEntry(ScriptEntryType.Handlers, input),
                new ScriptChunkEntry(ScriptEntryType.Literals, input),
                new ScriptChunkEntry(ScriptEntryType.LiteralsData, input),
            };

            Script = new LingoScript(this, input); 
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

            size += 18;

            size += sizeof(int);

            size += 8;

            

            return size;
        }

        public override void WriteTo(ShockwaveWriter output)
        {
            throw new NotImplementedException();
        }
    }
}
