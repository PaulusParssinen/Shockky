using Shockky.IO;

#nullable enable
namespace Shockky.Resources.Cast.Properties;

public class TransitionCastProperties : IMemberProperties
{
    public byte LegacyDuration { get; set; }
    public byte ChunkSize { get; set; }
    public TransitionType Type { get; set; }
    public TransitionFlags Flags { get; set; }
    public short DurationInMilliseconds { get; set; }

    public XtraCastProperties? Xtra { get; set; }

    public TransitionCastProperties(ref ShockwaveReader input, ReaderContext context)
    {
        //TODO: Version differences.
        LegacyDuration = input.ReadByte();
        ChunkSize = input.ReadByte();
        Type = (TransitionType)input.ReadByte();
        Flags = (TransitionFlags)input.ReadByte();
        DurationInMilliseconds = input.ReadInt16BigEndian(); //TODO: Not in < D5

        if (!Flags.HasFlag(TransitionFlags.Standard))
            Xtra = new XtraCastProperties(ref input, context);
    }

    public int GetBodySize(WriterOptions options)
    {
        int size = 0;
        size += sizeof(byte);
        size += sizeof(byte);
        size += sizeof(byte);
        size += sizeof(byte);
        size += sizeof(short);
        if (!Flags.HasFlag(TransitionFlags.Standard))
            size += Xtra!.GetBodySize(options);
        return size;
    }

    public void WriteTo(ShockwaveWriter output, WriterOptions options)
    {
        throw new NotImplementedException();
    }
}