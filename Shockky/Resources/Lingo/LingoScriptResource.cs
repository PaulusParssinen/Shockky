using System.Diagnostics;

using Shockky.IO;
using Shockky.Lingo;

namespace Shockky.Resources;

public sealed class LingoScriptResource : IShockwaveItem, IResource
{
    public OsType Kind => OsType.Lscr;

    /// <summary>
    /// The index of the script within its owner <see cref="LingoContextResource">Context</see>.
    /// </summary>
    public short ContextIndex { get; set; }

    /// <summary>
    /// The index of the <see cref="LingoEnvironment">Environment</see> that owns the <see cref="LingoContextResource">Context</see> that owns this script.
    /// </summary>
    public short EnvironmentIndex { get; set; }

    /// <summary>
    /// For factory scripts, the index of the parent script within its owner <see cref="LingoContextResource"/>
    /// </summary>
    public short ParentContextIndex { get; set; }

    public short CastMemberId { get; set; }

    public LingoScriptFlags Flags { get; set; }
    public short FactoryNameIndex { get; set; }

    /// <summary>
    /// Represents all the events that this script handles.
    /// </summary>
    public LingoEventFlags EventFlags { get; set; }

    public List<short> EventHandlerIndices { get; set; }
    public List<short> Properties { get; set; }
    public List<short> Globals { get; set; }

    /// <summary>
    /// Represents all lingo handlers in this script.
    /// </summary>
    public List<LingoFunction> Functions { get; set; }

    public List<LingoLiteral> Literals { get; }

    public LingoScriptResource()
    {
        EventHandlerIndices = new List<short>();
        Properties = new List<short>();
        Globals = new List<short>();
        Functions = new List<LingoFunction>();
        Literals = new List<LingoLiteral>();
    }
    public LingoScriptResource(ref ShockwaveReader input)
    {
        input.ReverseEndianness = false;

        input.ReadInt32BigEndian();
        input.ReadInt32BigEndian();

        input.ReadInt32BigEndian();
        input.ReadInt32BigEndian();

        input.ReadInt16BigEndian();

        ContextIndex = input.ReadInt16BigEndian();
        EnvironmentIndex = input.ReadInt16BigEndian();
        ParentContextIndex = input.ReadInt16BigEndian();
        short environmentFactoryIndexGarbageDebug = input.ReadInt16BigEndian();

        input.ReadInt32BigEndian();
        input.ReadInt32BigEndian();
        input.ReadInt32BigEndian();

        Flags = (LingoScriptFlags)input.ReadInt32BigEndian();

        input.ReadInt32BigEndian();
        CastMemberId = input.ReadInt16BigEndian();

        FactoryNameIndex = input.ReadInt16BigEndian();

        EventHandlerIndices = new List<short>(input.ReadInt16BigEndian());
        int eventHandlerIndexOffset = input.ReadInt32BigEndian();

        EventFlags = (LingoEventFlags)input.ReadInt32BigEndian();

        Properties = new List<short>(input.ReadInt16BigEndian());
        int propertiesOffset = input.ReadInt32BigEndian();

        Globals = new List<short>(input.ReadInt16BigEndian());
        int globalsOffset = input.ReadInt32BigEndian();

        Functions = new List<LingoFunction>(input.ReadInt16BigEndian());
        int functionsOffset = input.ReadInt32BigEndian();

        Literals = new List<LingoLiteral>(input.ReadInt16BigEndian());
        int literalsOffset = input.ReadInt32BigEndian();

        int literalDataLength = input.ReadInt32BigEndian();
        int literalDataOffset = input.ReadInt32BigEndian();

        input.Position = propertiesOffset;
        for (int i = 0; i < Properties.Capacity; i++)
        {
            Properties.Add(input.ReadInt16BigEndian());
        }

        input.Position = globalsOffset;
        for (int i = 0; i < Globals.Capacity; i++)
        {
            Globals.Add(input.ReadInt16BigEndian());
        }

        input.Position = functionsOffset;
        for (int i = 0; i < Functions.Capacity; i++)
        {
            Functions.Add(new LingoFunction(ref input));
        }

        input.Position = literalsOffset;
        var literalEntries = new (VariantKind Kind, int Offset)[Literals.Capacity]; // TODO: Stackalloc/ArrayPool
        for (int i = 0; i < Literals.Capacity; i++)
        {
            literalEntries[i].Kind = (VariantKind)input.ReadInt32BigEndian();
            literalEntries[i].Offset = input.ReadInt32BigEndian();
        }

        input.Position = literalDataOffset;
        for (int i = 0; i < Literals.Capacity; i++)
        {
            (VariantKind kind, int offset) = literalEntries[i];

            Literals.Add(LingoLiteral.Read(ref input, kind, literalDataOffset + offset));

            Debug.Assert(literalDataOffset + literalDataLength >= input.Position);
        }

        input.Position = eventHandlerIndexOffset;
        for (int i = 0; i < EventHandlerIndices.Capacity; i++)
        {
            EventHandlerIndices.Add(input.ReadInt16BigEndian());
        }
    }

    public static int GetHeaderSize()
    {
        int size = 0;
        size += sizeof(int);
        size += sizeof(int);

        size += sizeof(int);
        size += sizeof(int);

        size += sizeof(short);
        size += sizeof(short);
        size += sizeof(short);
        size += sizeof(short);
        size += sizeof(short);

        size += sizeof(int);
        size += sizeof(int);
        size += sizeof(int);

        size += sizeof(int);

        size += sizeof(int);
        size += sizeof(short);

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
        size += sizeof(int);
        return size;
    }

    public int GetBodySize(WriterOptions options)
    {
        int size = 0;
        size += GetHeaderSize();
        return size;
    }

    public void WriteTo(ShockwaveWriter output, WriterOptions options)
    {
        output.WriteInt32BigEndian(0);
        output.WriteInt32BigEndian(0);

        int bodySize = GetBodySize(options);
        output.WriteInt32BigEndian(bodySize);
        output.WriteInt32BigEndian(bodySize);

        output.WriteInt32BigEndian(92); //TODO:

        output.WriteInt16BigEndian(ContextIndex);
        output.WriteInt16BigEndian(EnvironmentIndex);
        output.WriteInt16BigEndian(ParentContextIndex);
        output.WriteInt16BigEndian((short)-1);

        output.WriteInt32BigEndian(0);
        output.WriteInt32BigEndian(0);
        output.WriteInt32BigEndian(0);

        output.WriteInt32BigEndian((int)Flags);

        output.WriteInt16BigEndian((short)0);
        output.WriteInt16BigEndian(CastMemberId);

        output.WriteInt16BigEndian(FactoryNameIndex);
    }
}