using Shockky.IO;

namespace Shockky.Resources;

public class Channel : IShockwaveItem
{
    public byte ForeColor { get; set; }
    public byte BackColor { get; set; }

    public short LocV { get; set; } //x
    public short LocH { get; set; } //Y

    public short Height { get; set; }
    public short Width { get; set; }

    public bool FlipV { get; set; }
    public bool FlipH { get; set; }

    public Channel(ref ShockwaveReader input)
    { }

    public int GetBodySize(WriterOptions options)
    {
        throw new NotImplementedException();
    }

    public void WriteTo(ShockwaveWriter output, WriterOptions options)
    {
        throw new NotImplementedException();
    }
}