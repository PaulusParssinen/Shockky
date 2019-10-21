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
        public ScriptChunk(ShockwaveReader input, ChunkHeader header)
            : base(header)
        {
            Remnants.Enqueue(input.ReadBigEndian<int>());
            Remnants.Enqueue(input.ReadBigEndian<int>());

            input.ReadBigEndian<int>();
            input.ReadBigEndian<int>();

            input.ReadBigEndian<short>();
            ScriptNumber = input.ReadBigEndian<short>();

            Remnants.Enqueue(input.ReadBigEndian<short>());
            Remnants.Enqueue(input.ReadInt32()); // -1
            Remnants.Enqueue(input.ReadInt32()); // 0
            Remnants.Enqueue(input.ReadInt32());
            Remnants.Enqueue(input.ReadInt32()); // 0

            BehaviourScript = input.ReadBigEndian<int>(); //enum, Behav=0, Global=2

            Remnants.Enqueue(input.ReadBigEndian<int>());
            Remnants.Enqueue(input.ReadBigEndian<short>()); //scriptId

            Remnants.Enqueue(input.ReadInt16());
            
            Pool = new LingoValuePool(this, input);
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
            //TODO: RELEASE compilation should be able to handle these well, otherwise gonna start hardcoding them as constants
            int size = 0;
            size += GetHeaderSize();
            size += Pool.GetBodySize();
            return size;
        }

        public override void WriteBodyTo(ShockwaveWriter output)
        {
            const short HEADER_LENGTH = 92;

            output.WriteBigEndian((int)Remnants.Dequeue());
            output.WriteBigEndian((int)Remnants.Dequeue());

            output.WriteBigEndian(HEADER_LENGTH + Pool.GetBodySize());
            output.WriteBigEndian(HEADER_LENGTH + Pool.GetBodySize());

            output.WriteBigEndian(HEADER_LENGTH);
            output.WriteBigEndian(ScriptNumber);

            output.WriteBigEndian((short)Remnants.Dequeue());
            output.Write((int)Remnants.Dequeue());
            output.Write((int)Remnants.Dequeue());
            output.Write((int)Remnants.Dequeue());
            output.Write((int)Remnants.Dequeue());
            
            output.WriteBigEndian(BehaviourScript);
            
            output.WriteBigEndian((int)Remnants.Dequeue());
            output.WriteBigEndian((short)Remnants.Dequeue());

            output.Write((short)Remnants.Dequeue());

            Pool.WriteTo(output);
        }
    }
}
