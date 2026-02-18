using Shockky.IO;

namespace Shockky;

public interface IShockwaveItem
{
    int GetBodySize(WriterOptions options);
    void WriteTo(ShockwaveWriter output, WriterOptions options);
}