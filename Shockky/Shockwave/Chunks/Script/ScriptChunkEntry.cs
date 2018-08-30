using Shockky.IO;
using Shockky.Shockwave.Chunks.Enum;

namespace Shockky.Shockwave.Chunks.Script
{
    //TODO: __naming__ cause I can use this structure in other places too (handler's locals and arguments?)
    //Also organize
    public class ScriptChunkEntry : ShockwaveItem
    {
        protected override string DebuggerDisplay
            => $"Length: {Length} | Offset: {Offset} | Flags: {Flags}";

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

        public override int GetBodySize()
        {
            int size = 0;
            size += (Type == ScriptEntryType.LiteralsData) ? sizeof(int) : sizeof(short);
            size += sizeof(int);
            size += (Type == ScriptEntryType.HandlerVectors) ? sizeof(int) : 0;
            return size;
        }

        public override void WriteTo(ShockwaveWriter output)
        {
            if(Type == ScriptEntryType.LiteralsData)
                output.WriteBigEndian(Length);
            else output.WriteBigEndian((short)Length);

            output.WriteBigEndian(Offset);
            
            if (Type == ScriptEntryType.HandlerVectors)
                output.WriteBigEndian(Flags);
        }
    }
}
