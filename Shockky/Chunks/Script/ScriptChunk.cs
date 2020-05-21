using System.Collections.Generic;

using Shockky.IO;
using Shockky.Lingo;

namespace Shockky.Chunks
{
    public class ScriptChunk : ChunkItem
    {
        public LingoValuePool Pool { get; }

        public short ScriptNumber { get; set; }
        public int BehaviourScript { get; set; }

        public List<LingoHandler> Handlers => Pool.Handlers;

        public ScriptChunk()
            : base(ChunkKind.Lscr)
        { }
        public ScriptChunk(ref ShockwaveReader input, ChunkHeader header)
            : base(header)
        {
            input.IsBigEndian = true;

            Remnants.Enqueue(input.ReadInt32());
            Remnants.Enqueue(input.ReadInt32());

            input.ReadInt32();
            input.ReadInt32();

            input.ReadInt16();
            ScriptNumber = input.ReadInt16();

            Remnants.Enqueue(input.ReadInt16());
            Remnants.Enqueue(input.ReadBEInt32()); // -1
            Remnants.Enqueue(input.ReadBEInt32()); // 0
            Remnants.Enqueue(input.ReadBEInt32());
            Remnants.Enqueue(input.ReadBEInt32()); // 0

            BehaviourScript = input.ReadInt32(); //enum, Behav=0, Global=2

            Remnants.Enqueue(input.ReadInt32());
            Remnants.Enqueue(input.ReadInt16()); //scriptId

            Remnants.Enqueue(input.ReadBEInt16());
            
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

            output.Write((int)Remnants.Dequeue());
            output.Write((int)Remnants.Dequeue());

            output.Write(HEADER_LENGTH + Pool.GetBodySize());
            output.Write(HEADER_LENGTH + Pool.GetBodySize());

            output.Write(HEADER_LENGTH);
            output.Write(ScriptNumber);

            output.Write((short)Remnants.Dequeue());
            output.WriteBE((int)Remnants.Dequeue());
            output.WriteBE((int)Remnants.Dequeue());
            output.WriteBE((int)Remnants.Dequeue());
            output.WriteBE((int)Remnants.Dequeue());
            
            output.Write(BehaviourScript);
            
            output.Write((int)Remnants.Dequeue());
            output.Write((short)Remnants.Dequeue());

            output.WriteBE((short)Remnants.Dequeue());

            Pool.WriteTo(output);
        }
    }
}
