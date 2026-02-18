using Shockky.IO;
using Shockky.Resources.Cast;

namespace Shockky.Resources;

/// <summary>
/// Represents list of all cast members in the movie, sorted by the order they appear in the Score.
/// </summary>
public sealed class ScoreOrder : IShockwaveItem, IResource
{
    public OsType Kind => OsType.Sord;

    public CastMemberId[] Entries { get; set; }

    public ScoreOrder()
    {
        Entries = [];
    }
    public ScoreOrder(ref ShockwaveReader input, ReaderContext context)
    {
        input.ReverseEndianness = false;

        input.ReadInt32BigEndian();
        input.ReadInt32BigEndian();

        Entries = new CastMemberId[input.ReadInt32BigEndian()];
        input.ReadInt32BigEndian();

        input.ReadInt16BigEndian();
        input.ReadInt16BigEndian(); //TODO: dir <= 0x500 ? sizeof(short) : sizeof(short) * 2.

        for (int i = 0; i < Entries.Length; i++)
        {
            Entries[i] = new(input.ReadInt16BigEndian(), input.ReadInt16BigEndian());
        }
    }

    public int GetBodySize(WriterOptions options)
    {
        int size = 0;
        size += sizeof(int);

        size += sizeof(int);

        size += sizeof(int);

        size += sizeof(short);
        size += sizeof(short);

        size += sizeof(short) * 2 * Entries.Length;
        return size;
    }
    public void WriteTo(ShockwaveWriter output, WriterOptions options)
    {
        const short ENTRIES_OFFSET = 20;
        const short ENTRY_SIZE = sizeof(short) + sizeof(short);

        output.WriteInt32BigEndian(0);
        output.WriteInt32BigEndian(0);

        output.WriteInt32BigEndian(Entries.Length);
        output.WriteInt32BigEndian(Entries.Length);

        output.WriteInt16BigEndian(ENTRIES_OFFSET);
        output.WriteInt16BigEndian(ENTRY_SIZE);

        foreach (CastMemberId memberId in Entries)
        {
            output.WriteMemberIdBigEndian(memberId);
        }
    }
}