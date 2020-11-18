using Shockky.IO;
using Shockky.Lingo;

namespace Shockky.Chunks
{
    public class LingoScriptChunk : ChunkItem
    {
        public LingoValuePool Pool { get; }

        public short ScriptNumber { get; set; }
        public short CastMemberRef { get; set; }
        public int Type { get; set; }

        public LingoScriptChunk()
            : base(ChunkKind.Lscr)
        { }
        public LingoScriptChunk(ref ShockwaveReader input, ChunkHeader header)
            : base(header)
        {
            input.IsBigEndian = true;

            input.ReadInt32();
            input.ReadInt32();
            
            input.ReadInt32();
            input.ReadInt32();

            input.ReadInt16();
            ScriptNumber = input.ReadInt16();

            Remnants.Enqueue(input.ReadInt16());
            input.ReadBEInt32();
            input.ReadBEInt32();
            Remnants.Enqueue(input.ReadBEInt32());
            input.ReadBEInt32();

            Type = input.ReadInt32(); //TODO: enum, Behav=0, Global=2

            Remnants.Enqueue(input.ReadInt32());
            CastMemberRef = input.ReadInt16(); //scriptId

            Remnants.Enqueue(input.ReadBEInt16()); //factoryNameId?
            
            Pool = new LingoValuePool(this, ref input);
        }

        public int GetHeaderSize()
        {
            int size = 0;
            size += sizeof(int);
            size += sizeof(int);

            size += sizeof(int);
            size += sizeof(int);

            size += sizeof(short);
            size += sizeof(short);

            size += sizeof(short);
            size += sizeof(int);
            size += sizeof(int);
            size += sizeof(int);
            size += sizeof(int);

            size += sizeof(int);

            size += sizeof(int);
            size += sizeof(short);

            size += sizeof(short);
            return size;
        }

        public override int GetBodySize()
        {
            int size = 0;
            size += GetHeaderSize();
            size += Pool.GetBodySize();
            return size;
        }

        public override void WriteBodyTo(ShockwaveWriter output)
        {
            const short HEADER_LENGTH = 92;

            output.Write(0);
            output.Write(0);

            output.Write(HEADER_LENGTH + Pool.GetBodySize());
            output.Write(HEADER_LENGTH + Pool.GetBodySize());

            output.Write(HEADER_LENGTH);
            output.Write(ScriptNumber);

            output.Write((short)Remnants.Dequeue());
            output.WriteBE(-1);
            output.WriteBE(0);
            output.WriteBE((int)Remnants.Dequeue());
            output.WriteBE(0);
            
            output.Write(Type);
            
            output.Write((int)Remnants.Dequeue());
            output.Write(CastMemberRef);

            output.WriteBE((short)Remnants.Dequeue());

            Pool.WriteTo(output);
        }
    }
}
