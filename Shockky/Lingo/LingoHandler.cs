using System;
using System.Collections.Generic;

using Shockky.IO;
using Shockky.Chunks;

namespace Shockky.Lingo
{
    public class LingoHandler : LingoItem
    {
        protected override string DebuggerDisplay => $"on {Name}";

        public List<short> Arguments { get; }
        public List<short> Locals { get; }
        public List<byte> BytesPerLine { get; }

        public short NameIndex { get; set; }
        public string Name => Script.Pool.GetName(NameIndex);

        public int HandlerVectorPosition { get; }
        
        public LingoHandlerBody Body { get; }

        public LingoHandler(LingoScriptChunk script)
            : base(script)
        {
            Arguments = new List<short>();
            Locals = new List<short>();
            BytesPerLine = new List<byte>();
        }

        public LingoHandler(LingoScriptChunk script, ref ShockwaveReader input)
            : this(script)
        {
            NameIndex = input.ReadInt16();
            HandlerVectorPosition = input.ReadInt16();

            Body = new LingoHandlerBody(this, ref input);
            int codeOffset = input.ReadInt32();

            Arguments.Capacity = input.ReadInt16();
            int argumentsOffset = input.ReadInt32();

            Locals.Capacity = input.ReadInt16();
            int localsOffset = input.ReadInt32();

            input.ReadInt16(); //offset(?)
            input.ReadInt32(); //length(?)

            input.ReadInt32(); //offset?
            input.ReadInt16(); //length?

            BytesPerLine.Capacity = input.ReadInt16();
            int lineOffset = input.ReadInt32();

            Body.StackHeight = input.ReadInt32();

            int handlerEndOffset = input.Position;

            input.Position = codeOffset;
            input.ReadBytes(Body.Code);

            input.Position = argumentsOffset;
            for (int i = 0; i < Arguments.Capacity; i++)
            {
                Arguments.Add(input.ReadInt16());
            }

            input.Position = localsOffset;
            for (int i = 0; i < Locals.Capacity; i++)
            {
                Locals.Add(input.ReadInt16());
            }

            input.Position = lineOffset;
            for (int i = 0; i < BytesPerLine.Capacity; i++)
            {
                BytesPerLine.Add(input.ReadByte());
            }

            input.Position = handlerEndOffset;
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
            throw new NotImplementedException();

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
