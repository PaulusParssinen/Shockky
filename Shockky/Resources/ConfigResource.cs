using System.Diagnostics;
using System.Drawing;

using Shockky.IO;
using Shockky.Resources.Cast;

namespace Shockky.Resources;

public sealed class ConfigResource : IShockwaveItem, IResource
{
    public OsType Kind => OsType.DRCF;

    public const short LENGTH = 100;

    public DirectorVersion Version { get; set; }

    public Rectangle Rect { get; set; }

    public short MinMemberNum { get; set; }
    public short MaxMemberNum { get; set; }

    public short Field12 { get; set; }
    public short CommentFont { get; set; }
    public short CommentSize { get; set; }
    public short CommentStyle { get; set; }

    public byte LegacyTempo { get; set; }
    public bool LightSwitch { get; set; }

    public byte Field1E { get; set; }
    public byte Field1F { get; set; }
    public int DataSize { get; set; }

    public short StagePalette { get; set; }
    public short DefaultColorDepth { get; set; }
    public DirectorVersion OriginalVersion { get; set; }
    public short MaxCastColorDepth { get; set; }
    public ConfigFlags Flags { get; set; }

    public ulong ScoreUsedChannelsMask { get; set; }
    public bool Trial { get; set; }
    public byte Field34 { get; set; }

    public short Tempo { get; set; }
    public Platform Platform { get; set; }

    public short SaveSeed { get; set; }
    public int Field3C { get; set; }
    public uint Checksum { get; set; }

    public short OldDefaultPalette { get; set; }
    public short Field46 { get; set; }
    public int MaxCastResourceNum { get; set; }
    public CastMemberId DefaultPalette { get; set; }
    public byte Field50 { get; set; }
    public byte Field51 { get; set; }
    public short Field52 { get; set; }

    public int DownloadFramesBeforePlaying { get; set; }
    public short Field58 { get; set; }
    public short Field5A { get; set; }
    public short Field5C { get; set; }
    public short Field5E { get; set; }
    public short Field60 { get; set; }
    public short Field62 { get; set; }

    public ConfigResource(ref ShockwaveReader input, ReaderContext context)
    {
        input.ReverseEndianness = false;

        input.ReadInt16BigEndian();
        Version = (DirectorVersion)input.ReadInt16BigEndian();

        Rect = input.ReadRectLittleEndian();

        MinMemberNum = input.ReadInt16BigEndian();
        MaxMemberNum = input.ReadInt16BigEndian();

        LegacyTempo = input.ReadByte();
        LightSwitch = input.ReadBoolean();

        Field12 = input.ReadInt16BigEndian();

        CommentFont = input.ReadInt16BigEndian();
        CommentSize = input.ReadInt16BigEndian();
        CommentStyle = input.ReadInt16BigEndian();
        // v >= 1025
        StagePalette = input.ReadInt16BigEndian();
        DefaultColorDepth = input.ReadInt16BigEndian();

        Field1E = input.ReadByte();
        Field1F = input.ReadByte();
        DataSize = input.ReadInt32BigEndian();

        OriginalVersion = (DirectorVersion)input.ReadInt16BigEndian();
        MaxCastColorDepth = input.ReadInt16BigEndian();
        Flags = (ConfigFlags)input.ReadInt32BigEndian();
        ScoreUsedChannelsMask = input.ReadUInt64BigEndian();

        Trial = input.ReadBoolean();
        Field34 = input.ReadByte();

        Tempo = input.ReadInt16BigEndian();
        Platform = (Platform)input.ReadInt16BigEndian();
        // v >= 1113
        SaveSeed = input.ReadInt16BigEndian();
        Field3C = input.ReadInt32BigEndian();
        Checksum = input.ReadUInt32BigEndian();
        // v >= 1114
        OldDefaultPalette = input.ReadInt16BigEndian();
        // v >= 1115
        Field46 = input.ReadInt16BigEndian();

        MaxCastResourceNum = input.ReadInt32BigEndian();
        DefaultPalette = new CastMemberId(input.ReadInt16BigEndian(), input.ReadInt16BigEndian());

        Field50 = input.ReadByte();
        Field51 = input.ReadByte();
        Field52 = input.ReadInt16BigEndian();

        if (!input.IsDataAvailable) return;

        DownloadFramesBeforePlaying = input.ReadInt32BigEndian();
        Field58 = input.ReadInt16BigEndian();
        Field5A = input.ReadInt16BigEndian();
        Field5C = input.ReadInt16BigEndian();
        Field5E = input.ReadInt16BigEndian();
        Field60 = input.ReadInt16BigEndian();
        Field62 = input.ReadInt16BigEndian();

        Debug.Assert(Checksum == CalculateChecksum(), "Config checksum mismatch!");
    }

    public uint CalculateChecksum()
    {
        uint checksum = LENGTH + 1;
        unchecked
        {
            checksum *= ((uint)Version + 2);
            checksum /= (uint)Rect.Top + 3;
            checksum *= (uint)Rect.Left + 4;
            checksum /= (uint)Rect.Bottom + 5;
            checksum *= (uint)Rect.Right + 6;
            checksum -= (uint)MinMemberNum + 7;
            checksum *= (uint)MaxMemberNum + 8;
            checksum -= (uint)LegacyTempo + 9;
            checksum -= (uint)(LightSwitch ? 1 : 0) + 10;
            checksum += (uint)Field12 + 11;
            checksum *= (uint)CommentFont + 12;
            checksum += (uint)CommentSize + 13;
            checksum *= (uint)CommentStyle + 14;
            checksum += (uint)StagePalette + 15;
            checksum += (uint)DefaultColorDepth + 16;
            checksum += (uint)Field1E + 17;
            checksum *= (uint)Field1F + 18;
            checksum += (uint)DataSize + 19;
            checksum *= (uint)OriginalVersion + 20;
            checksum += (uint)MaxCastColorDepth + 21;
            checksum += (uint)Flags + 22;
            checksum += (uint)(ScoreUsedChannelsMask >> 32) + 23;
            checksum += (uint)(ScoreUsedChannelsMask & 0xFFFFFFFF) + 24;
            checksum *= (uint)Field34 + 25;
            checksum += (uint)Tempo + 26;
            checksum *= (uint)Platform + 27;
            checksum *= (uint)((SaveSeed * 0xE06) + 0xFFF450000); // - 0xBB000
            checksum ^= 0x72616C66;
        }
        return checksum;
    }

    public int GetBodySize(WriterOptions options) => LENGTH;

    public void WriteTo(ShockwaveWriter output, WriterOptions options)
    {
        output.WriteInt16BigEndian(LENGTH);
        output.WriteInt16BigEndian((short)Version);
        output.WriteRect(Rect);
        output.WriteInt16BigEndian(MinMemberNum);
        output.WriteInt16BigEndian(MaxMemberNum);
        output.WriteByte(LegacyTempo);
        output.WriteBoolean(LightSwitch);

        output.WriteInt16BigEndian(Field12);
        output.WriteInt16BigEndian(CommentFont);
        output.WriteInt16BigEndian(CommentSize);
        output.WriteInt16BigEndian(CommentStyle);

        output.WriteInt16BigEndian(StagePalette);
        output.WriteInt16BigEndian(DefaultColorDepth);

        output.WriteByte(Field1E);
        output.WriteByte(Field1F);
        output.WriteInt32BigEndian(DataSize);

        output.WriteInt16BigEndian((short)OriginalVersion);
        output.WriteInt16BigEndian(MaxCastColorDepth);
        output.WriteInt32BigEndian((int)Flags);
        output.WriteUInt64BigEndian(ScoreUsedChannelsMask);
        output.WriteBoolean(Trial);
        output.WriteByte(Field34);
        output.WriteInt16BigEndian(Tempo);
        output.WriteInt16BigEndian((short)Platform);
        output.WriteInt16BigEndian(SaveSeed);
        output.WriteInt32BigEndian(Field3C);
        output.WriteUInt32BigEndian(Checksum); // TODO: CalculateChecksum()
        output.WriteInt16BigEndian(OldDefaultPalette);

        output.WriteInt16BigEndian(Field46);
        output.WriteInt32BigEndian(MaxCastResourceNum);
        output.WriteMemberIdBigEndian(DefaultPalette);
        output.WriteByte(Field50);
        output.WriteByte(Field51);
        output.WriteInt16BigEndian(Field52);

        output.WriteInt32BigEndian(DownloadFramesBeforePlaying);
        output.WriteInt16BigEndian(Field58);
        output.WriteInt16BigEndian(Field5A);
        output.WriteInt16BigEndian(Field5C);
        output.WriteInt16BigEndian(Field5E);
        output.WriteInt16BigEndian(Field60);
        output.WriteInt16BigEndian(Field62);
    }
}