using System.Diagnostics;
using System.Drawing;

using Shockky.IO;
using Shockky.Resources.Cast;

namespace Shockky.Resources;

public sealed class ConfigResource : IShockwaveItem, IResource
{
    public OsType Kind => OsType.DRCF;

    public const short Length = 100;

    /// <summary>
    /// The movie version.
    /// </summary>
    /// <remarks>When equal to <see cref="DirectorVersion.Protected"/> magic value, the actual version is stored in the <see cref="OriginalVersion"/> field.</remarks>
    /// <seealso cref="OriginalVersion"/>
    public DirectorVersion Version { get; set; }

    /// <summary>
    /// The movie stage dimensions.
    /// </summary>
    public Rectangle Rect { get; set; }

    /// <summary>
    /// The minimum cast member number of the movie cast.
    /// </summary>
    public short MinMemberNum { get; set; }

    /// <summary>
    /// The maximum cast member number of the movie cast.
    /// </summary>
    public short MaxMemberNum { get; set; }

    /// <summary>
    /// The movie tempo in ticks per frame. Only used in Director 2 and below.
    /// </summary>
    public byte LegacyTempo { get; set; }

    /// <summary>
    /// Legacy switch to set the stage background to black color instead of white. Only used in Director 2 and below.
    /// </summary>
    public bool LightSwitch { get; set; }

    public short CommentFont { get; set; }
    public short CommentSize { get; set; }
    public short CommentStyle { get; set; }

    /// <summary>
    /// The stage background color.
    /// </summary>
    /// <remarks>Used for Director 7 and above.</remarks>
    /// <seealso cref="StagePalette"/>
    public Color StageColor { get; set; }

    /// <summary>
    /// The stage background color palette index.
    /// </summary>
    /// <seealso cref="StageColor"/>
    public short StagePalette { get; set; }

    /// <summary>
    /// The default color depth of the movie.
    /// </summary>
    public short DefaultColorDepth { get; set; }

    /// <summary>
    /// If <see langword="true"/>, the system must support color.
    /// </summary>
    public bool ColorRequired { get; set; }

    public byte Field1F { get; set; }

    /// <summary>
    /// The calculated size of all cast member metadata, cast member data, score 
    /// data, timecode data, and frame label data for the movie.
    /// </summary>
    public int DataSize { get; set; }

    /// <summary>
    /// The original movie version.
    /// </summary>
    /// <remarks>For protected movies, this contains the actual version.</remarks>
    public DirectorVersion OriginalVersion { get; set; }

    /// <summary>
    /// The maximum color depth of any cast member in the movie cast.
    /// </summary>
    public short MaxCastColorDepth { get; set; }

    /// <summary>
    /// The movie configuration flags.
    /// </summary>
    public ConfigFlags Flags { get; set; }

    /// <summary>
    /// A bitmask of used score channels.
    /// </summary>
    /// <remarks>Not always populated even if there are used score channels.</remarks>
    public ulong ScoreUsedChannelsMask { get; set; }

    /// <summary>
    /// If <see langword="true"/>, the movie was authored using Director Trial.
    /// </summary>
    public bool Trial { get; set; }

    public byte Field34 { get; set; }

    /// <summary>
    /// The initial score tempo.
    /// </summary>
    public short Tempo { get; set; }

    /// <summary>
    /// The original platform the movie was authored on.
    /// </summary>
    public Platform Platform { get; set; }

    /// <summary>
    /// A pseudo-random value that is regenerated every time a movie is saved
    /// by the authoring tool.
    /// </summary>
    /// <remarks>When equal to <c>23</c>, the movie is protected.</remarks>
    /// <seealso cref="IsProtected"/>
    public short SaveSeed { get; set; }

    public int Field3C { get; set; }

    /// <summary>
    /// A checksum of all preceeding fields.
    /// </summary>
    /// <seealso cref="CalculateChecksum"/>
    public uint Checksum { get; set; }

    /// <summary>
    /// The number of cast members in the movie.
    /// </summary>
    public short CastMemberCount { get; set; }

    public short Field46 { get; set; }

    /// <summary>
    /// The highest resource number used for a cast library.
    /// </summary>
    public int MaxCastResourceNum { get; set; }

    /// <summary>
    /// The default movie palette.
    /// </summary>
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

    public bool IsProtected => SaveSeed % 23 == 0;

    public ConfigResource(ref ShockwaveReader input, ReaderContext context)
    {
        input.ReverseEndianness = false;

        input.ReadInt16BigEndian();
        Version = (DirectorVersion)input.ReadInt16BigEndian();

        Rect = input.ReadRectBigEndian();

        MinMemberNum = input.ReadInt16BigEndian();
        MaxMemberNum = input.ReadInt16BigEndian();

        LegacyTempo = input.ReadByte();
        LightSwitch = input.ReadBoolean();

        // TODO: Forgot what's up with this, i.e. where's red.
        StageColor = Color.FromArgb(0, input.ReadByte(), input.ReadByte());

        CommentFont = input.ReadInt16BigEndian();
        CommentSize = input.ReadInt16BigEndian();
        CommentStyle = input.ReadInt16BigEndian();

        if (Version < DirectorVersion.V1025) return;

        StagePalette = input.ReadInt16LittleEndian();

        DefaultColorDepth = input.ReadInt16BigEndian();

        // D2 -> D3 did not update the version apparently
        if (!input.IsDataAvailable) return;

        ColorRequired = input.ReadBoolean();
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

        if (Version < DirectorVersion.V1113) return;
        SaveSeed = input.ReadInt16BigEndian();
        Field3C = input.ReadInt32BigEndian();
        Checksum = input.ReadUInt32BigEndian();

        if (Version < DirectorVersion.V1114) return;
        CastMemberCount = input.ReadInt16BigEndian();

        if (Version < DirectorVersion.V1115) return;
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

        // var calculatedChecksum = CalculateChecksum();
        // TODO: Debug.Assert(Checksum == calculatedChecksum, "Config checksum mismatch!");
    }

    public uint CalculateChecksum()
    {
        uint checksum = Length + 1;
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
            checksum += (uint)StageColor.G + 11;
            checksum *= (uint)CommentFont + 12;
            checksum += (uint)CommentSize + 13;
            checksum *= (uint)CommentStyle + 14;
            checksum += (uint)StagePalette + 15;
            checksum += (uint)DefaultColorDepth + 16;
            checksum += (uint)(ColorRequired ? 1 : 0) + 17;
            checksum *= (uint)Field1F + 18;
            checksum += (uint)DataSize + 19;
            checksum *= (uint)OriginalVersion + 20;
            checksum += (uint)MaxCastColorDepth + 21;
            checksum += (uint)Flags + 22;
            checksum += (uint)(ScoreUsedChannelsMask >> 32) + 23;
            checksum += (uint)(ScoreUsedChannelsMask & 0xFFFFFFFF) + 24;
            checksum *= (uint)(Trial ? 1 : 0) + 25;
            checksum += (uint)Tempo + 26;
            checksum *= (uint)Platform + 27;
            checksum *= (uint)((SaveSeed * 0xE06) + 0xFFF450000); // - 0xBB000
            checksum ^= 0x72616C66;
        }
        return checksum;
    }

    public int GetBodySize(WriterOptions options) => Length;

    public void WriteTo(ShockwaveWriter output, WriterOptions options)
    {
        output.WriteInt16BigEndian(Length);
        output.WriteInt16BigEndian((short)Version);
        output.WriteRect(Rect);
        output.WriteInt16BigEndian(MinMemberNum);
        output.WriteInt16BigEndian(MaxMemberNum);
        output.WriteByte(LegacyTempo);
        output.WriteBoolean(LightSwitch);

        output.WriteByte(StageColor.G);
        output.WriteByte(StageColor.B);
        output.WriteInt16BigEndian(CommentFont);
        output.WriteInt16BigEndian(CommentSize);
        output.WriteInt16BigEndian(CommentStyle);

        output.WriteInt16BigEndian(StagePalette);
        output.WriteInt16BigEndian(DefaultColorDepth);

        output.WriteBoolean(ColorRequired);
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
        output.WriteInt16BigEndian(CastMemberCount);

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