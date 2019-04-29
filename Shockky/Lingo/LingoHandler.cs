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

        public LingoHandler(ScriptChunk script, ShockwaveReader input)
            : this(script)
        {
            NameIndex = input.ReadBigEndian<short>();
            HandlerVectorPosition = input.ReadBigEndian<short>();

            Body = new LingoHandlerBody(this, input);
            _codeOffset = input.ReadBigEndian<int>();

            Arguments.Capacity = input.ReadBigEndian<short>();
            _argumentsOffset = input.ReadBigEndian<int>();

            Locals.Capacity = input.ReadBigEndian<short>();
            _localsOffset = input.ReadBigEndian<int>();
            
            short unk1Length = input.ReadBigEndian<short>();
            int unk1Offset = input.ReadBigEndian<int>();
            
            int unk2Length = input.ReadBigEndian<int>();
            int unk2Offset = input.ReadBigEndian<short>();

            BytesPerLine.Capacity = input.ReadBigEndian<short>();
            _lineOffset = input.ReadBigEndian<int>();

            Body.StackHeight = input.ReadBigEndian<int>();
        }

        public void Populate(ShockwaveReader input, long scriptChunkOffset)
        {
            input.Position = scriptChunkOffset + _codeOffset;
            input.Read(Body.Code, 0, Body.Code.Length);

            input.PopulateVList(scriptChunkOffset + _argumentsOffset, Arguments, input.ReadBigEndian<short>);
            input.PopulateVList(scriptChunkOffset + _localsOffset, Locals, input.ReadBigEndian<short>);
            input.PopulateVList(scriptChunkOffset + _lineOffset, new List<byte>(), input.ReadByte);
        }

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
            output.WriteBigEndian(NameIndex);
            output.WriteBigEndian((short)HandlerVectorPosition);

            output.WriteBigEndian(Body.Code.Length);
            output.WriteBigEndian(0);

            output.WriteBigEndian((short)Arguments.Capacity);
            output.WriteBigEndian(0);

            output.WriteBigEndian((short)Locals.Capacity);
            output.WriteBigEndian(0);
        }
    }
}
