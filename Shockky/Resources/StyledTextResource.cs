using Shockky.IO;

namespace Shockky.Resources;

public class StyledTextResource : IShockwaveItem, IResource
{
    public OsType Kind => OsType.STXT;

    public string Text { get; set; }
    public TextFormat[] Formats { get; set; } = Array.Empty<TextFormat>();

    public StyledTextResource(ref ShockwaveReader input, ReaderContext context)
    {
        input.ReverseEndianness = true;

        input.ReadInt32LittleEndian();
        int textLength = input.ReadInt32LittleEndian();
        input.ReadInt32LittleEndian();

        Text = input.ReadString(textLength);

        Formats = new TextFormat[input.ReadInt16LittleEndian()];
        for (int i = 0; i < Formats.Length; i++)
        {
            Formats[i] = new TextFormat(ref input, context);
        }
    }

    public void WriteTo(ShockwaveWriter output, WriterOptions options)
    {
        const int TEXT_OFFSET = 12;
        const int TEXT_FORMAT_SIZE = 20;

        output.WriteInt32LittleEndian(TEXT_OFFSET);
        output.WriteInt32LittleEndian(Text.Length);
        output.WriteInt32LittleEndian(sizeof(short) + (Formats.Length * TEXT_FORMAT_SIZE));

        output.WriteString(Text); //TODO: 

        output.WriteInt16LittleEndian((short)Formats.Length);
        for (int i = 0; i < Formats.Length; i++)
        {
            Formats[i].WriteTo(output, options);
        }
    }

    public int GetBodySize(WriterOptions options)
    {
        const int TEXT_FORMAT_SIZE = 20;

        int size = 0;
        size += sizeof(int);
        size += sizeof(int);
        size += sizeof(int);
        size += Text.Length;
        size += sizeof(short);
        size += Formats.Length * TEXT_FORMAT_SIZE;
        return size;
    }
}