using System.Collections.Generic;

using Shockky.IO;
using Shockky.Chunks;

namespace Shockky.Lingo
{
    public class LingoHandler : LingoItem
    {
        protected override string DebuggerDisplay => $"on {Name}";

        private readonly int _codeOffset,
            _argumentsOffset,
            _localsOffset,
            _lineOffset;

        public List<short> Arguments { get; }
        public List<short> Locals { get; }
        public List<byte> BytesPerLine { get; }

        public short NameIndex { get; set; }
        public string Name => Script.Pool.GetName(NameIndex);

        public int HandlerVectorPosition { get; }
        
        public LingoHandlerBody Body { get; }

        public LingoHandler(ScriptChunk script)
            : base(script)
        {
            Arguments = new List<short>();
            Locals = new List<short>();
            BytesPerLine = new List<byte>();
        }

        public LingoHandler(ScriptChunk script, ref ShockwaveReader input)
            : this(script)
        {
            NameIndex = input.ReadInt16();
            HandlerVectorPosition = input.ReadInt16();

            Body = new LingoHandlerBody(this, input);
            _codeOffset = input.ReadInt32();

            Arguments.Capacity = input.ReadInt16();
            _argumentsOffset = input.ReadInt32();

            Locals.Capacity = input.ReadInt16();
            _localsOffset = input.ReadInt32();
            
            short unk1Length = input.ReadInt16();
            int unk1Offset = input.ReadInt32();
            
            int unk2Length = input.ReadInt32();
            int unk2Offset = input.ReadInt16();

            BytesPerLine.Capacity = input.ReadInt16();
            _lineOffset = input.ReadInt32();

            Body.StackHeight = input.ReadInt32();
        }

        public void Populate(ref ShockwaveReader input, int scriptChunkOffset)
        {
            input.Advance(scriptChunkOffset + _codeOffset);
            input.ReadBytes(Body.Code);

            //input.PopulateVList(scriptChunkOffset + _argumentsOffset, Arguments, input.ReadInt16);
            //input.PopulateVList(scriptChunkOffset + _localsOffset, Locals, input.ReadInt16);
            //input.PopulateVList(scriptChunkOffset + _lineOffset, new List<byte>(), input.ReadByte);
        }

        //AddLocal()

        public override int GetBodySize()
        {
            int size = 0;
            size += sizeof(short);
            size += sizeof(short);
            size += sizeof(int);
            size += sizeof(int);
            size += sizeof(short);
            size += sizeof(int);
            size += sizeof(short);
            size += sizeof(int);
            size += sizeof(short);
            size += sizeof(int);
            size += sizeof(int);
            size += sizeof(short);
            size += sizeof(short);
            size += sizeof(int);

            size += sizeof(int);
            return size;
        }

        public override void WriteTo(ShockwaveWriter output)
        {
            output.Write(NameIndex);
            output.Write((short)HandlerVectorPosition);

            output.Write(Body.Code.Length);
            output.Write(0);

            output.Write((short)Arguments.Capacity);
            output.Write(0);

            output.Write((short)Locals.Capacity);
            output.Write(0);
        }
    }
}
