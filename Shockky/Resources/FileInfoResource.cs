using Shockky.IO;

namespace Shockky.Resources;

public sealed class FileInfoResource : IShockwaveItem, IResource
{
    public OsType Kind => OsType.VWFI;

    public string CreatedBy { get; set; }
    public string ModifiedBy { get; set; }
    public string FilePath { get; set; }

    public FileInfoResource(ref ShockwaveReader input, ReaderContext context)
    {
        input.ReverseEndianness = true;

        input.ReadBytes(input.ReadInt32LittleEndian());
        int offsets = input.ReadInt16LittleEndian();
        input.ReadByte();
        for (short i = 0; i < offsets; i++)
        {
            input.ReadInt32LittleEndian();
        }

        input.ReadByte();
        CreatedBy = input.ReadString();
        input.ReadByte();
        ModifiedBy = input.ReadString();
        input.ReadByte();
        FilePath = input.ReadString();
    }

    public int GetBodySize(WriterOptions options)
    {
        throw new NotImplementedException();
    }

    public void WriteTo(ShockwaveWriter output, WriterOptions options)
    {
        throw new NotImplementedException();
    }
}