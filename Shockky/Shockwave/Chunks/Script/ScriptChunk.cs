using System;
using System.Collections.Generic;
using System.Diagnostics;
using Shockky.IO;
using Shockky.Shockwave.Lingo;

namespace Shockky.Shockwave.Chunks
{
    public class ScriptChunk : ChunkItem
    {
        public List<LingoHandler> Handlers => Pool.Handlers;
        
        public LingoValuePool Pool { get; }
        
        public int Unknown1 { get; set; }
        public int Unknown2 { get; set; }
        public int TotalLength { get; set; }

        public short HeaderLength { get; set; }
        public short ScriptNumber { get; set; }

        public int BehaviourScript { get; set; }

        public ScriptChunk(ShockwaveReader input, ChunkHeader header)
            : base(header)
        {
            Unknown1 = input.ReadBigEndian<int>();
            Unknown2 = input.ReadBigEndian<int>();

            TotalLength = input.ReadBigEndian<int>();
            input.ReadBigEndian<int>(); //totalLength2

            HeaderLength = input.ReadBigEndian<short>();
            ScriptNumber = input.ReadBigEndian<short>();

            short unk0 = input.ReadBigEndian<short>();
            int unk1 = input.ReadInt32(); // -1
            int unk2 = input.ReadInt32(); // 0
            int unk3 = input.ReadInt32();
            int unk4 = input.ReadInt32(); // 0

            BehaviourScript = input.ReadBigEndian<int>(); //enum, Behav=0, Global=2

            int unk5varCount = input.ReadBigEndian<int>();
            short scriptId = input.ReadBigEndian<short>();

            short unk6 = input.ReadInt16();
            
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

            //TODO:

            size += 18;

            size += sizeof(int);

            size += 8;

            return size;
        }

        public override void WriteBodyTo(ShockwaveWriter output)
        {
            throw new NotImplementedException();
            output.WriteBigEndian(Unknown1);
            output.WriteBigEndian(Unknown2);

            output.WriteBigEndian(TotalLength);
            output.WriteBigEndian(TotalLength);

            output.WriteBigEndian(HeaderLength);
            output.WriteBigEndian(ScriptNumber);
        }
    }
}
