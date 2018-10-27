using System.Collections.Generic;

using Shockky.IO;
using Shockky.Shockwave.Chunks;

namespace Shockky.Shockwave.Lingo
{
    public class LingoHandler : LingoItem
    {
        protected override string DebuggerDisplay => $"on {Name}";

        private int _codeLength,
                    _codeOffset;
        private int _argumentsLength,
                    _argumentsOffset;
        private int _localsLength,
                    _localsOffset;
        private int _lineLength,
                    _lineOffset;

        //TODO: Do I really need to wrap them like dis
        public List<short> Arguments { get; }
        public List<short> Locals { get; }

        public short NameIndex { get; set; }
        public string Name => Script.Pool.GetName(NameIndex);

        public int HandlerVectorPosition { get; }
        
        public LingoHandlerBody Body { get; }

        public LingoHandler(ScriptChunk script)
            : base(script)
        {
            Arguments = new List<short>();
            Locals = new List<short>();
        }

        public LingoHandler(ScriptChunk script, ShockwaveReader input)
            : this(script)
        {
            NameIndex = input.ReadBigEndian<short>();
            HandlerVectorPosition = input.ReadBigEndian<short>();
            
            _codeLength = input.ReadBigEndian<int>();
            _codeOffset = input.ReadBigEndian<int>();

            _argumentsLength = input.ReadBigEndian<short>();
            _argumentsOffset = input.ReadBigEndian<int>();

            _localsLength = input.ReadBigEndian<short>();
            _localsOffset = input.ReadBigEndian<int>();
            
            short unk1Length = input.ReadBigEndian<short>();
            int unk1Offset = input.ReadBigEndian<int>();
            
            int unk2Length = input.ReadBigEndian<int>();
            int unk2Offset = input.ReadBigEndian<short>();

            _lineLength = input.ReadBigEndian<short>();
            _lineOffset = input.ReadBigEndian<int>();

            Body = new LingoHandlerBody(this, input);
        }

        public void Populate(ShockwaveReader input, long scriptChunkOffset)
        {
            input.Position = scriptChunkOffset + _codeOffset;
            Body.Code = input.ReadBytes(_codeLength);

            input.PopulateVList(_argumentsLength, scriptChunkOffset + _argumentsOffset, Arguments, input.ReadBigEndian<short>);
            input.PopulateVList(_localsLength, scriptChunkOffset + _localsOffset, Locals, input.ReadBigEndian<short>);
            input.PopulateVList(_lineLength, scriptChunkOffset + _lineOffset, new List<byte>(), input.ReadByte);
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
            Body.WriteTo(output);
        }
    }
}
