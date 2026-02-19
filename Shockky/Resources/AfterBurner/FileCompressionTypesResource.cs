using Shockky.IO;

namespace Shockky.Resources;

public sealed class FileCompressionTypesResource : IShockwaveItem, IResource
{
    public OsType Kind => OsType.Fcdr;

    public (Guid Id, string Description)[] CompressionTypes { get; set; } = [];

    public FileCompressionTypesResource()
    { }
    public FileCompressionTypesResource(ref ShockwaveReader input, ReaderContext context)
    {
        using ZLibShockwaveReader decompressedInput = ZLib.CreateDeflateReaderUnsafe(ref input);

        int compressionTypeCount = decompressedInput.ReadInt16BigEndian();

        Guid[] ids = new Guid[compressionTypeCount];
        for (int i = 0; i < ids.Length; i++)
        {
            ids[i] = new Guid(decompressedInput.ReadBytes(16));
        }

        CompressionTypes = new (Guid Id, string Description)[compressionTypeCount];
        for (int i = 0; i < CompressionTypes.Length; i++)
        {
            CompressionTypes[i] = (ids[i], decompressedInput.ReadCString());
        }
    }

    public int GetBodySize(WriterOptions options) => throw new NotImplementedException();
    public void WriteTo(ShockwaveWriter output, WriterOptions options) => throw new NotImplementedException();
}