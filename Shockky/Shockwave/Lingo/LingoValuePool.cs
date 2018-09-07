using System;
using System.Collections.Generic;

using Shockky.IO;
using Shockky.Shockwave.Chunks;
using Shockky.Shockwave.Chunks.Script;

namespace Shockky.Shockwave.Lingo
{
    //TODO: HMM static create method? :thonk:
    public class LingoValuePool : ShockwaveItem
    {
        private readonly ShockwaveReader _input;

        private readonly ScriptChunkEntry _handlerVectorsEntry;
        private readonly ScriptChunkEntry _propertiesEntry;
        private readonly ScriptChunkEntry _globalsEntry;
        private readonly ScriptChunkEntry _handlersEntry;
        private readonly ScriptChunkEntry _literalsEntry;
        private readonly ScriptChunkEntry _literalDataEntry;

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
            _input = input;

            _handlerVectorsEntry = new ScriptChunkEntry(ScriptEntryType.HandlerVectors, input);
            _propertiesEntry = new ScriptChunkEntry(ScriptEntryType.Properties, input);
            _globalsEntry = new ScriptChunkEntry(ScriptEntryType.Globals, input);
            _handlersEntry = new ScriptChunkEntry(ScriptEntryType.Handlers, input);
            _literalsEntry = new ScriptChunkEntry(ScriptEntryType.Literals, input);
            _literalDataEntry = new ScriptChunkEntry(ScriptEntryType.LiteralsData, input);

            Populate(_handlerVectorsEntry, HandlerVectors, input.ReadBigEndian<short>);
            Populate(_propertiesEntry, Properties, input.ReadBigEndian<short>);
            Populate(_globalsEntry, Globals, input.ReadBigEndian<short>);
            Populate(_handlersEntry, Handlers, ReadHandler);
            Populate(_literalsEntry, Literals, ReadLiteral);

            for (int i = 0; i < Literals.Count; i++)
            {
                LingoLiteral literal = Literals[i];
                literal.ReadValue(input, _literalDataEntry.Offset);
            }

            //TODO: reset input pos imo?
        }

        private LingoLiteral ReadLiteral() => new LingoLiteral(Script, _input);
        private LingoHandler ReadHandler() => new LingoHandler(Script, _input);
        
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

        public void Populate<T>(ScriptChunkEntry entry, List<T> list, Func<T> reader)
        {
            long ogPos = _input.Position;
            _input.Position = Script.Header.Offset + entry.Offset;

            list.Capacity = entry.Length;
            for (int i = 0; i < list.Capacity; i++)
            {
                var value = reader();
                list.Add(value);
            }

            _input.Position = ogPos;
        }

        public override int GetBodySize()
        {
            int size = 0;
            size += _handlerVectorsEntry.GetBodySize();
            size += _propertiesEntry.GetBodySize();
            size += _globalsEntry.GetBodySize();
            size += _handlersEntry.GetBodySize();
            size += _literalsEntry.GetBodySize();
            size += _literalDataEntry.GetBodySize();
            return size;
        }

        public override void WriteTo(ShockwaveWriter output)
        {
            throw new NotImplementedException();
            output.Write(_handlerVectorsEntry);
            output.Write(_propertiesEntry);
            output.Write(_globalsEntry);
            output.Write(_handlersEntry);
            output.Write(_literalsEntry);
            output.Write(_literalDataEntry);
        }
    }
}
