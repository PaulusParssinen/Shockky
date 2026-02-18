using Shockky.IO;

namespace Shockky.Resources;

public class Score : IShockwaveItem, IResource
{
    public OsType Kind => OsType.VWSC;

    public Score()
    { }
    public Score(ref ShockwaveReader input)
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