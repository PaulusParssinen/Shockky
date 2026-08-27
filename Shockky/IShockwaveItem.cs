using Shockky.IO;

namespace Shockky;

public interface IShockwaveItem
{
    int GetBodySize(WriterOptions options);
    void WriteTo(ref ShockwaveWriter output, WriterOptions options);
}

public interface IShockwaveItem<TSelf> : IShockwaveItem
    where TSelf : IShockwaveItem<TSelf>
{
    static abstract TSelf Read(ref ShockwaveReader input, ReaderContext context);
}