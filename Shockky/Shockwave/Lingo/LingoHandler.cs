using System;
using System.Collections.Generic;
using Shockky.IO;
using Shockky.Shockwave.Chunks;

namespace Shockky.Shockwave.Lingo
{
    public class LingoHandler : LingoItem
    {
        protected override string DebuggerDisplay => $"on {Name}";
        
        //TODO: Do I really need to wrap them like dis
        public List<short> Arguments { get; }
        public List<short> Locals { get; }

        public short NameIndex { get; set; }
        public string Name => Script.Pool.GetName(NameIndex);

        public int HandlerVectorPosition { get; }

        public int LineCount { get; }
        public int LineOffset { get; }
        
        public LingoHandlerBody Body { get; }

        public LingoHandler(ScriptChunk scriptChunk)
            : base(scriptChunk)
        {
            Arguments = new List<short>();
            Locals = new List<short>();
        }

        public LingoHandler(ScriptChunk script, ShockwaveReader input)
            : this(script)
        {
            NameIndex = input.ReadBigEndian<short>();
            HandlerVectorPosition = input.ReadBigEndian<short>();

            Body = new LingoHandlerBody(this, input);

            short argumentsLength = input.ReadBigEndian<short>();
            int argumentsOffset = input.ReadBigEndian<int>();

            short localsLength = input.ReadBigEndian<short>();
            int localsOffset = input.ReadBigEndian<int>();

            Populate(argumentsLength, argumentsOffset, input, Arguments, input.ReadBigEndian<short>);
            Populate(localsLength, localsOffset, input, Locals, input.ReadBigEndian<short>);

            input.ReadBigEndian<short>(); //unk1Count
            input.ReadBigEndian<int>(); //unk1Offset

            input.ReadBigEndian<int>();
            input.ReadBigEndian<short>();

            LineCount = input.ReadBigEndian<short>();
            LineOffset = input.ReadBigEndian<int>(); //Apparently relative to whole section

            Body.StackHeight = input.ReadBigEndian<int>();
        }

        public void Populate<T>(int length, int offset,
            ShockwaveReader input,
            List<T> list, Func<T> reader) //uhh, duplicate code
        {
            long ogPos = input.Position;
            input.Position = offset;

            list.Capacity = length;
            for (int i = 0; i < list.Capacity; i++)
            {
                var value = reader();
                list.Add(value);
            }

            input.Position = ogPos;
        }

        public override int GetBodySize()
        {
            int size = 0;
            size += sizeof(short);
            size += sizeof(short);
            size += Body.GetBodySize();
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
            throw new NotImplementedException();
            output.WriteBigEndian(NameIndex);
            output.WriteBigEndian((short)HandlerVectorPosition);
            Body.WriteTo(output);
        }
    }
}
