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

        public ScriptChunkEntry(ScriptEntryType type, ShockwaveReader input)
        {
            Type = type;

            Length = (type == ScriptEntryType.LiteralsData) ? 
                input.ReadBigEndian<int>() : input.ReadBigEndian<short>();

            Offset = input.ReadBigEndian<int>();

            if (type == ScriptEntryType.HandlerVectors)
                Flags = input.ReadBigEndian<int>();
        }
    }
}
