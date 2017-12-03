using System.Diagnostics;
using Shockky.IO;
using Shockky.Shockwave.Chunks.Enum;

namespace Shockky.Shockwave.Chunks
{
    [DebuggerDisplay("{Type} | Length: {Length} | Offset: {Offset} | Flags: {Flags}")]
    public class ScriptChunkEntry
    {
        public ScriptEntryType Type { get; set; }

        public int Length { get; set; }
        public int Offset { get; set; }
        public int Flags { get; set; }

        public ScriptChunkEntry(ScriptEntryType type, ref ShockwaveReader input)
        {
            Type = type;

            Length = (type == ScriptEntryType.LiteralsData) ? 
                input.ReadInt32(true) : input.ReadInt16(true);

            Offset = input.ReadInt32(true);

            if (type == ScriptEntryType.HandlerVectors)
                Flags = input.ReadInt32(true);
        }
    }
}
