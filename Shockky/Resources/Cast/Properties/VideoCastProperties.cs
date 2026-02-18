using System.Drawing;

using Shockky.IO;

namespace Shockky.Resources.Cast;

public class VideoCastProperties : IMemberProperties
{
    public string Type { get; set; }

    public bool Streaming { get; set; }
    public bool HasSound { get; set; }
    public bool PausedAtStart { get; set; }

    public VideoCastFlags Flags { get; set; }
    public byte Framerate { get; set; }
    public Rectangle Rectangle { get; set; }

    public VideoCastProperties(ref ShockwaveReader input, ReaderContext context)
    {
        Type = input.ReadString((int)input.ReadUInt32LittleEndian());
        input.Advance(10);

        //TODO:
        byte videoFlags = input.ReadByte();
        Streaming = ((videoFlags & 1) == 1);

        videoFlags = input.ReadByte();
        HasSound = ((videoFlags & 1) == 1);
        PausedAtStart = ((videoFlags & 2) == 2);

        Flags = (VideoCastFlags)input.ReadByte();
        input.Advance(3);
        Framerate = input.ReadByte();
        input.Advance(32);
        Rectangle = input.ReadRectLittleEndian();
    }

    public int GetBodySize(WriterOptions options)
    {
        int size = 0;
        size += sizeof(short);
        size += 10;
        size += sizeof(byte);
        size += sizeof(byte);
        size += sizeof(byte);
        size += 3;
        size += sizeof(byte);
        size += 32;
        size += sizeof(short) * 4;
        return size;
    }

    public void WriteTo(ShockwaveWriter output, WriterOptions options)
    {
        throw new NotImplementedException(nameof(VideoCastProperties));
    }
}