using Shockky.IO;

namespace Shockky.Resources;

public class ScoreResource : IShockwaveItem, IResource
{
    public OsType Kind => OsType.VWSC;

    public ScoreResource()
    { }
    public ScoreResource(ref ShockwaveReader input)
    {
        input.ReverseEndianness = false;

        int totalLength = input.ReadInt32BigEndian();
        int headerType = input.ReadInt32BigEndian(); //-3

        throw new NotImplementedException();
    }

    public int GetBodySize(WriterOptions options)
    {
        throw new NotImplementedException();
        int size = 0;
        size += sizeof(int);
        size += sizeof(int);

        size += sizeof(int);
        size += sizeof(int);

        size += sizeof(int);
        size += sizeof(int);
        return size;
    }

    public void WriteTo(ShockwaveWriter output, WriterOptions options)
    {
        throw new NotImplementedException();
    }
}