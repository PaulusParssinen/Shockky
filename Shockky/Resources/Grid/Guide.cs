using Shockky.IO;

namespace Shockky.Resources;

public readonly record struct Guide(Axis Axis, short Position) : IShockwaveItem
{
    public static Guide Read(ref ShockwaveReader reader, ReaderContext context)
        => new((Axis)reader.ReadInt16LittleEndian(), reader.ReadInt16LittleEndian());

    public int GetBodySize(WriterOptions options)
    {
        int size = 0;
        size += sizeof(short);
        size += sizeof(short);
        return size;
    }
    public void WriteTo(ShockwaveWriter output, WriterOptions options)
    {
        output.WriteInt16LittleEndian((short)Axis);
        output.WriteInt16LittleEndian(Position);
    }
}