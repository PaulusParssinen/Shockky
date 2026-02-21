using System.Text;

using Shockky.IO;

namespace Shockky.Resources;

public sealed class ScoreLabels : IShockwaveItem, IResource
{
    public OsType Kind => OsType.VWLB;

    public Dictionary<short, string> Labels { get; set; }

    public ScoreLabels(ref ShockwaveReader input, ReaderContext context)
    {
        input.ReverseEndianness = false;

        var offsetMap = new (short frame, int offset)[input.ReadInt16BigEndian()];
        Labels = new Dictionary<short, string>(offsetMap.Length);

        for (int i = 0; i < offsetMap.Length; i++)
        {
            offsetMap[i] = (input.ReadInt16BigEndian(), input.ReadInt16BigEndian());
        }

        int length = input.ReadInt32BigEndian();

        string labels = input.ReadPString();

        for (int i = 0; i < offsetMap.Length; i++)
        {
            var (frame, offset) = offsetMap[i];

            if (i == offsetMap.Length - 1)
                Labels[frame] = labels[offset..];
            else
                Labels[frame] = labels[offset..offsetMap[i + 1].offset];
        }
    }

    public int GetBodySize(WriterOptions options)
    {
        int size = 0;
        size += sizeof(short);
        size += Labels.Count * (2 * sizeof(short));
        size += sizeof(int);
        size += Labels.Values.Sum(l => l.Length);
        return size;
    }

    public void WriteTo(ShockwaveWriter output, WriterOptions options)
    {
        int offset = 0;
        var builder = new StringBuilder();

        output.WriteInt32BigEndian(Labels.Count);
        foreach ((short frame, string label) in Labels)
        {
            output.WriteInt16BigEndian(frame);
            output.WriteInt16BigEndian((short)offset);

            offset += label.Length;
            builder.Append(label);
        }

        output.WriteInt32BigEndian(builder.Length);
        output.WriteBytes(Encoding.UTF8.GetBytes(builder.ToString()));
    }
}