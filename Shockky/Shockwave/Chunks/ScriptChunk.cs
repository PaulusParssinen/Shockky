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

            int unknown1 = input.ReadInt32(true);
            int unknown2 = input.ReadInt32(true);
           
            int totalLength = input.ReadInt32(true);
            int totalLength2 = input.ReadInt32(true);

            short headerLength = input.ReadInt16(true);
            short scriptNumber = input.ReadInt16(true);

            input.Position = 38;
            int behaviourScript = input.ReadInt32(true); 

            input.Position = 50;

            Entries = new List<ScriptChunkEntry>(6)
            {
                new ScriptChunkEntry(ScriptEntryType.HandlerVectors, ref input),
                new ScriptChunkEntry(ScriptEntryType.Properties, ref input),
                new ScriptChunkEntry(ScriptEntryType.Globals, ref input),
                new ScriptChunkEntry(ScriptEntryType.Handlers, ref input),
                new ScriptChunkEntry(ScriptEntryType.Literals, ref input),
                new ScriptChunkEntry(ScriptEntryType.LiteralsData, ref input),
            };

            Script = new LingoScript(this, input);
        }
    }
}
