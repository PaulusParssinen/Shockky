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
        public LingoValuePool(ScriptChunk script, ref ShockwaveReader input)
            : this(script)
        {
            //LingoHandler ReadHandler() => new LingoHandler(script, input);
            //LingoLiteral ReadLiteral() => new LingoLiteral(input);

            HandlerVectors.Capacity = input.ReadInt16();
            int handlerVectorOffset = input.ReadInt32();
            HandlerVectorFlags = (HandlerVectorFlags)input.ReadBEInt32();

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

            //TODO: PrefixedReader
            //input.PopulateVList(script.Header.Offset + propertiesOffset, Properties, input.ReadInt16);
            //input.PopulateVList(script.Header.Offset + globalsOffset, Globals, input.ReadInt16);
            //
            //input.PopulateVList(script.Header.Offset + handlersOffset, Handlers, ReadHandler);
            //foreach (LingoHandler handler in Handlers)
            //{
            //    handler.Populate(input, script.Header.Offset);
            //}
            //
            //input.PopulateVList(script.Header.Offset + literalsOffset, Literals, ReadLiteral);
            //foreach (LingoLiteral literal in Literals)
            //{
            //    literal.ReadValue(input, script.Header.Offset + literalDataOffset);
            //
            //    if (script.Header.Offset + literalDataOffset + literalDataLength <= input.Position)
            //        break;
            //}
            //
            //input.PopulateVList(script.Header.Offset + handlerVectorOffset, HandlerVectors, input.ReadInt16, 
            //    forceLengthCheck: false);
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
                TypeCode.Int64 => LiteralKind.FloatingPoint,

                _ => throw new ArgumentException()
            };

            return AddConstant(Literals, new LingoLiteral(kind, value), recycle);
        }
        
        //TODO: implement ConstantKind?

        public virtual int AddConstant<T>(List<T> constants, T value, bool recycle)
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
