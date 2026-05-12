using Shockky.IO;

namespace Shockky.Lingo;

public class LingoFunction : IShockwaveItem
{
    public short EnvironmentIndex { get; set; }
    public LingoEventFlags EventFlags { get; set; }

    public byte[] Bytecode { get; set; }
    public List<short> Arguments { get; set; }
    public List<short> Locals { get; set; }
    public List<short> Globals { get; set; }
    public byte[] BytesPerLine { get; set; }

    public int ParserBytesRead { get; set; }
    public int BodyLineNumber { get; set; }

    public int StackHeight { get; set; }

    public LingoFunction()
    {
        Bytecode = [];
        Arguments = [];
        Locals = [];
        Globals = [];
        BytesPerLine = [];
    }
    public LingoFunction(ref ShockwaveReader input)
    {
        EnvironmentIndex = input.ReadInt16BigEndian();
        EventFlags = (LingoEventFlags)input.ReadInt16BigEndian();

        Bytecode = new byte[input.ReadInt32BigEndian()];
        int bytecodeOffset = input.ReadInt32BigEndian();

        Arguments = new List<short>(input.ReadInt16BigEndian());
        int argumentsOffset = input.ReadInt32BigEndian();

        Locals = new List<short>(input.ReadInt16BigEndian());
        int localsOffset = input.ReadInt32BigEndian();

        Globals = new List<short>(input.ReadInt16BigEndian()); //v5
        int globalsOffset = input.ReadInt32BigEndian(); //v5

        ParserBytesRead = input.ReadInt32BigEndian();
        BodyLineNumber = input.ReadInt16BigEndian();

        BytesPerLine = new byte[input.ReadInt16BigEndian()];
        int lineOffset = input.ReadInt32BigEndian();

        //if version > 0x800
        StackHeight = input.ReadInt32BigEndian();

        // TODO: Seperate "data" reading from handler body
        int bodyEndOffset = input.Position;

        input.Position = bytecodeOffset;
        input.ReadBytes(Bytecode);

        input.Position = argumentsOffset;
        for (int i = 0; i < Arguments.Capacity; i++)
        {
            Arguments.Add(input.ReadInt16BigEndian());
        }

        input.Position = localsOffset;
        for (int i = 0; i < Locals.Capacity; i++)
        {
            Locals.Add(input.ReadInt16BigEndian());
        }

        input.Position = globalsOffset;
        for (int i = 0; i < Globals.Capacity; i++)
        {
            Globals.Add(input.ReadInt16BigEndian());
        }

        input.Position = lineOffset;
        for (int i = 0; i < BytesPerLine.Length; i++)
        {
            BytesPerLine[i] = input.ReadByte();
        }

        input.Position = bodyEndOffset;
    }

    public int GetBodySize(WriterOptions options)
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

    public void WriteTo(ShockwaveWriter output, WriterOptions options)
    {
        output.WriteInt16BigEndian(EnvironmentIndex);
        output.WriteUInt16BigEndian((ushort)EventFlags);
        output.WriteUInt16BigEndian((ushort)Bytecode.Length);
        output.WriteUInt16BigEndian(0); // Reserve
        output.WriteUInt16BigEndian((ushort)Arguments.Count);
        output.WriteInt32BigEndian(0); // Reserve
        output.WriteUInt16BigEndian((ushort)Locals.Count);
        output.WriteInt32BigEndian(0); // Reserve
        output.WriteUInt16BigEndian((ushort)Globals.Count);
        output.WriteInt32BigEndian(0); // Reserve
        output.WriteUInt16BigEndian((ushort)BytesPerLine.Length);
    }
}