using System;
using System.Diagnostics;
using System.Collections.Generic;

using Shockky.IO;
using Shockky.Chunks;

namespace Shockky.Lingo
{
    public class LingoValuePool : ShockwaveItem
    {
        private LingoNameChunk _lingoNameChunk;
        public LingoNameChunk NameTableChunk
        {
            get => _lingoNameChunk;
            set
            {
                _lingoNameChunk = value;
                NameList = _lingoNameChunk?.Names ?? new List<string>();
            }
        }

        public LingoScriptChunk Script { get; set; }

        public List<string> NameList { get; set; }

        public List<short> HandlerVectors { get; }
        public List<short> Properties { get; }
        public List<short> Globals { get; }
        public List<LingoHandler> Handlers { get; }
        public List<LingoLiteral> Literals { get; }

        public HandlerVectorFlags HandlerVectorFlags { get; set; }

        public LingoValuePool()
        {
            NameList = new List<string>();

            HandlerVectors = new List<short>();
            Properties = new List<short>();
            Globals = new List<short>();
            Handlers = new List<LingoHandler>();
            Literals = new List<LingoLiteral>();
        }
        public LingoValuePool(LingoScriptChunk script)
            : this()
        {
            Script = script;
        }
        public LingoValuePool(LingoScriptChunk script, ref ShockwaveReader input)
            : this(script)
        {
            HandlerVectors.Capacity = input.ReadInt16();
            int handlerVectorOffset = input.ReadInt32();

            HandlerVectorFlags = (HandlerVectorFlags)input.ReadInt32();

            Properties.Capacity = input.ReadInt16();
            int propertiesOffset = input.ReadInt32();

            Globals.Capacity = input.ReadInt16();
            int globalsOffset = input.ReadInt32();

            Handlers.Capacity = input.ReadInt16();
            int handlersOffset = input.ReadInt32();

            Literals.Capacity = input.ReadInt16();
            int literalsOffset = input.ReadInt32();

            int literalDataLength = input.ReadInt32();
            int literalDataOffset = input.ReadInt32();

            input.Position = propertiesOffset;
            for (int i = 0; i < Properties.Capacity; i++)
            {
                Properties.Add(input.ReadInt16());
            }

            input.Position = globalsOffset;
            for (int i = 0; i < Globals.Capacity; i++)
            {
                Globals.Add(input.ReadInt16());
            }

            input.Position = handlersOffset;
            for (int i = 0; i < Handlers.Capacity; i++)
            {
                Handlers.Add(new LingoHandler(script, ref input));
            }

            input.Position = literalsOffset;

            var literalEntries = new (LiteralKind Kind, int Offset)[Literals.Capacity];
            for (int i = 0; i < Literals.Capacity; i++)
            {
                literalEntries[i].Kind = (LiteralKind)input.ReadInt32();
                literalEntries[i].Offset = input.ReadInt32();
            }

            input.Position = literalDataOffset;
            for (int i = 0; i < Literals.Capacity; i++)
            {
                (LiteralKind kind, int offset) = literalEntries[i];

                Literals.Add(LingoLiteral.Read(ref input, kind, literalDataOffset + offset));

                if (literalDataOffset + literalDataLength < input.Position)
                    throw new IndexOutOfRangeException();
            }

            input.Position = handlerVectorOffset;
            for (int i = 0; i < HandlerVectors.Capacity; i++)
            {
                HandlerVectors.Add(input.ReadInt16());
            }
        }

        public string GetName(int index)
        {
            return NameTableChunk?.Names[index];
        }

        public int AddName(string name, bool recycle = true)
        {
            return AddConstant(NameList, name, recycle);
        }

        public int AddLiteral(object value, bool recycle = true)
        {
            LiteralKind kind = Type.GetTypeCode(value.GetType()) switch
            {
                TypeCode.String => LiteralKind.String,
                TypeCode.Int32 => LiteralKind.Integer,
                TypeCode.Double => LiteralKind.FloatingPoint,

                _ => throw new ArgumentException()
            };

            return AddConstant(Literals, new LingoLiteral(kind, value), recycle);
        }
        
        //TODO: implement ConstantKind?

        public int AddConstant<T>(List<T> constants, T value, bool recycle)
        {
            int index = (recycle ? constants.IndexOf(value) : -1);
            if (index == -1)
            {
                constants.Add(value);
                index = (constants.Count - 1);
            }
            return index;
        }

        public override int GetBodySize()
        {
            int size = 0;
            size += sizeof(short);
            size += sizeof(int);
            size += sizeof(int);

            size += sizeof(short);
            size += sizeof(int);

            size += sizeof(short);
            size += sizeof(int);

            size += sizeof(short);
            size += sizeof(int);

            size += sizeof(short);
            size += sizeof(int);

            size += sizeof(int);
            size += sizeof(int);
            return size;
        }

        public override void WriteTo(ShockwaveWriter output)
        {
            const int HANDLER_SIZE = 46;
            const int LITERAL_ENTRY_SIZE = 8; //TODO: You know what, I might really switch to the constants lengths afterall

            int entriesOffset = Script.GetHeaderSize() + Script.GetBodySize();

            output.Write((short)HandlerVectors.Capacity);
            output.Write(0);
            output.WriteBE((int)HandlerVectorFlags);
            
            output.Write((short)Properties.Capacity);
            output.Write(entriesOffset);
            entriesOffset += Properties.Capacity * sizeof(short);
            
            output.Write((short)Globals.Capacity);
            output.Write(entriesOffset);
            entriesOffset += Globals.Capacity * sizeof(short);
            
            output.Write((short)Handlers.Capacity);
            output.Write(entriesOffset);
            entriesOffset += Handlers.Capacity * HANDLER_SIZE;
            throw new NotImplementedException();
        }
    }
}
