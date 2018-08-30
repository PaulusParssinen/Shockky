using System.Collections.Generic;

using Shockky.IO;
using Shockky.Shockwave.Lingo;

namespace Shockky.Shockwave.Chunks
{
    public class ScriptChunk : ChunkItem
    {
        public LingoValuePool Pool { get; }
        
        public int TotalLength { get; set; }

        public short HeaderLength { get; set; }
        public short ScriptNumber { get; set; }

        public int BehaviourScript { get; set; }

        public List<LingoHandler> Handlers => Pool.Handlers;

        public ScriptChunk(ShockwaveReader input, ChunkHeader header)
            : base(header)
        {
            Remnants.Enqueue(input.ReadBigEndian<int>());
            Remnants.Enqueue(input.ReadBigEndian<int>());

            TotalLength = input.ReadBigEndian<int>();
            Remnants.Enqueue(input.ReadBigEndian<int>()); //totalLength2

            HeaderLength = input.ReadBigEndian<short>();
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

        public override int GetBodySize()
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
            size += Pool.GetBodySize();
            return size;
        }

        public override void WriteBodyTo(ShockwaveWriter output)
        {
            output.WriteBigEndian((int)Remnants.Dequeue());
            output.WriteBigEndian((int)Remnants.Dequeue());

            output.WriteBigEndian(TotalLength);
            output.WriteBigEndian(TotalLength);

            output.WriteBigEndian(HeaderLength);
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
