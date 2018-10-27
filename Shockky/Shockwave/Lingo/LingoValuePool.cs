using System;
using System.Collections.Generic;

using Shockky.IO;
using Shockky.Shockwave.Chunks;

namespace Shockky.Shockwave.Lingo
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

            short handlerVectorCount = input.ReadBigEndian<short>();
            int handlerVectorOffset = input.ReadBigEndian<int>();
            int handlerVectorFlags = input.ReadInt32();

            short propertiesCount = input.ReadBigEndian<short>();
            int propertiesOffset = input.ReadBigEndian<int>();

            short globalsCount = input.ReadBigEndian<short>();
            int globalsOffset = input.ReadBigEndian<int>();

            short handlersCount = input.ReadBigEndian<short>();
            int handlersOffset = input.ReadBigEndian<int>();

            short literalsCount = input.ReadBigEndian<short>();
            int literalsOffset = input.ReadBigEndian<int>();

            int literalsDataCount = input.ReadBigEndian<int>();
            int literalsDataOffset = input.ReadBigEndian<int>();

            input.PopulateVList(propertiesCount, script.Header.Offset + propertiesOffset, Properties, input.ReadBigEndian<short>);
            input.PopulateVList(globalsCount, script.Header.Offset + globalsOffset, Globals, input.ReadBigEndian<short>);

            input.PopulateVList(handlersCount, script.Header.Offset + handlersOffset, Handlers, ReadHandler);
            foreach(var handler in Handlers)
            {
                handler.Populate(input, script.Header.Offset);
            }
            
            input.PopulateVList(literalsCount, script.Header.Offset + literalsOffset, Literals, ReadLiteral);
            /*foreach (var literal in Literals)
            { 
                literal.ReadValue(input, script.Header.Offset + literalsDataOffset);
            }*/

            input.PopulateVList(handlerVectorCount, script.Header.Offset + handlerVectorOffset, HandlerVectors, input.ReadBigEndian<short>, 
                forceLengthCheck: false);
        }

        public string GetName(int index)
        {
            return NameTableChunk?.Names[index] ?? throw new Exception("u wot");
        }

        public int AddName(string name, bool recycle = true)
        {
            return AddConstant(NameList, name, recycle);
        }

        public int AddLiteral(object value, bool recycle = true)
        {
            LiteralKind type;

            switch (Type.GetTypeCode(value.GetType()))
            {
                case TypeCode.Int32:
                case TypeCode.UInt32:
                    type = LiteralKind.Integer;
                    break;
                case TypeCode.String:
                    type = LiteralKind.String;
                    break;
                default: return -1;
            }

            return AddLiteral(type, value, recycle);
        }
        public int AddLiteral(LiteralKind kind, object value, bool recycle = true)
        {
            int index = (recycle ? Literals.FindIndex(i => i.Kind == kind && i.Value == value) : -1);
            if (index == -1)
            {
                Literals.Add(new LingoLiteral(kind, value));
                index = (Literals.Count - 1);
            }
            return index;
        }
        
        //TODO: implement ConstantKind?

        protected virtual int AddConstant<T>(List<T> constants, T value, bool recycle)
        {
            int index = (recycle ? constants.IndexOf(value, 1) : -1);
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
            throw new NotImplementedException();

        }
    }
}
