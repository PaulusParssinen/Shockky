using System;
using System.Collections.Generic;
using System.Diagnostics;
using Shockky.IO;
using Shockky.Shockwave.Chunks.Enum;
using Shockky.Shockwave.Lingo;

namespace Shockky.Shockwave.Chunks
{
    public class ScriptChunk : ChunkItem
    {
        private readonly ShockwaveReader _input;

        public List<ScriptChunkEntry> Entries { get; }
        public List<string> NameList { get; }

        public LingoScript Script { get; private set; }

        public ScriptChunkEntry this[ScriptEntryType type]
            => Entries[(int) type];

        public int Unknown1 { get; set; }
        public int Unknown2 { get; set; }

        public int TotalLength { get; set; }

        public short HeaderLength { get; set; }
        public short ScriptNumber { get; set; }

        public int BehaviourScript { get; set; }

        public ScriptChunk(ShockwaveReader input, ChunkEntry entry)
            : base(entry.Header)
        {
            Unknown1 = input.ReadBigEndian<int>();
            Unknown2 = input.ReadBigEndian<int>();
           
            TotalLength = input.ReadBigEndian<int>();
            input.ReadBigEndian<int>(); //totalLength2

            HeaderLength = input.ReadBigEndian<short>();
            ScriptNumber = input.ReadBigEndian<short>();
            
            input.Position = 38; //TODO: We need to figure this stuff out!
            BehaviourScript = input.ReadBigEndian<int>(); 

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

            _input = input;
        }

        public void Disassemble(List<string> nameList) //TODO: or something idk
        {
            Script = new LingoScript(this, _input, nameList); //TODO: callback for handler dissasembly or somethign
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
            Debugger.Break(); //Not finished, noob
            output.WriteBigEndian(Unknown1);
            output.WriteBigEndian(Unknown2);

            output.WriteBigEndian(TotalLength);
            output.WriteBigEndian(TotalLength);

            output.WriteBigEndian(HeaderLength);
            output.WriteBigEndian(ScriptNumber);
        }
    }
}
