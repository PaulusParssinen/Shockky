using System;
using System.Collections.Generic;

using Shockky.IO;
using Shockky.Chunks;

namespace Shockky.Lingo
{
    public class LingoValuePool : ShockwaveItem
    {
        private NameTableChunk _nameTableChunk;
        public NameTableChunk NameTableChunk
        {
            get => _nameTableChunk;
            set
            {
                _nameTableChunk = value;
                NameList = _nameTableChunk?.Names ?? new List<string>();
            }
        }

        public ScriptChunk Script { get; set; }

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
        public LingoValuePool(ScriptChunk script)
            : this()
        {
            Script = script;
        }
        public LingoValuePool(ScriptChunk script, ShockwaveReader input)
            : this(script)
        {
            LingoHandler ReadHandler() => new LingoHandler(script, input);
            LingoLiteral ReadLiteral() => new LingoLiteral(input);

            HandlerVectors.Capacity = input.ReadBigEndian<short>();
            int handlerVectorOffset = input.ReadBigEndian<int>();
            HandlerVectorFlags = (HandlerVectorFlags)input.ReadInt32();

            Properties.Capacity = input.ReadBigEndian<short>();
            int propertiesOffset = input.ReadBigEndian<int>();

            Globals.Capacity = input.ReadBigEndian<short>();
            int globalsOffset = input.ReadBigEndian<int>();

            Handlers.Capacity = input.ReadBigEndian<short>();
            int handlersOffset = input.ReadBigEndian<int>();

            Literals.Capacity = input.ReadBigEndian<short>();
            int literalsOffset = input.ReadBigEndian<int>();

            int literalDataLength = input.ReadBigEndian<int>();
            int literalDataOffset = input.ReadBigEndian<int>();

            input.PopulateVList(script.Header.Offset + propertiesOffset, Properties, input.ReadBigEndian<short>);
            input.PopulateVList(script.Header.Offset + globalsOffset, Globals, input.ReadBigEndian<short>);

            input.PopulateVList(script.Header.Offset + handlersOffset, Handlers, ReadHandler);
            foreach (LingoHandler handler in Handlers)
            {
                handler.Populate(input, script.Header.Offset);
            }

            input.PopulateVList(script.Header.Offset + literalsOffset, Literals, ReadLiteral);
            foreach (LingoLiteral literal in Literals)
            {
                literal.ReadValue(input, script.Header.Offset + literalDataOffset);

                if (script.Header.Offset + literalDataOffset + literalDataLength <= input.Position)
                    break;
            }

            input.PopulateVList(script.Header.Offset + handlerVectorOffset, HandlerVectors, input.ReadBigEndian<short>, 
                forceLengthCheck: false);
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
            LiteralKind kind = Type.GetTypeCode(value?.GetType() ?? null) switch
            {
                TypeCode.String => LiteralKind.String,
                TypeCode.Int32 => LiteralKind.Integer,
                TypeCode.Int64 => LiteralKind.Float,

                _ => throw new ArgumentException()
            };

            return AddConstant(Literals, new LingoLiteral(kind, value), recycle);
        }
        
        //TODO: implement ConstantKind?

        protected virtual int AddConstant<T>(List<T> constants, T value, bool recycle)
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
            const int LITERAL_SIZE = 8; //TODO: You know what, I might really switch to the constants lengths afterall

            int entriesOffset = Script.GetHeaderSize() + Script.GetBodySize();

            output.WriteBigEndian((short)HandlerVectors.Capacity);
            output.WriteBigEndian(0);
            output.WriteBigEndian(HandlerVectorFlags);
            
            output.WriteBigEndian((short)Properties.Capacity);
            output.WriteBigEndian(entriesOffset);
            entriesOffset += Properties.Capacity * sizeof(short);
            
            output.WriteBigEndian((short)Globals.Capacity);
            output.WriteBigEndian(entriesOffset);
            entriesOffset += Globals.Capacity * sizeof(short);
            
            output.WriteBigEndian((short)Handlers.Capacity);
            output.WriteBigEndian(entriesOffset);
            entriesOffset += Handlers.Capacity * HANDLER_SIZE;
            throw new NotImplementedException();
        }
    }
}
