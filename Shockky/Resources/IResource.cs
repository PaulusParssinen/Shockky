using Shockky.IO;

namespace Shockky.Resources;

public interface IResource
{
    OsType Kind { get; }

    public static IResource Read(scoped ref ShockwaveReader input, ReaderContext context)
    {
        var header = new ResourceHeader(ref input);
        return Read(ref input, context, header.Kind, header.Length);
    }
    public static IResource Read(scoped ref ShockwaveReader input, ReaderContext context, OsType kind, int length)
    {
        ReadOnlySpan<byte> chunkSpan = input.ReadBytes(length);
        var bodyInput = new ShockwaveReader(chunkSpan, input.ReverseEndianness);

        return kind switch
        {
            OsType.Fver => new FileVersion(ref bodyInput, context),
            OsType.Fcdr => new FileCompressionTypes(ref bodyInput, context),
            OsType.ABMP => new AfterburnerMap(ref bodyInput, context),

            OsType.imap => new IndexMap(ref bodyInput),
            OsType.mmap => new MemoryMap(ref bodyInput),
            OsType.KEYPtr => new KeyMap(ref bodyInput, context),
            OsType.VWCF or OsType.DRCF => new Config(ref bodyInput, context),

            // TODO: handle V1850
            //OsType.VWLB => new ScoreLabels(ref chunkInput, context),
            OsType.VWFI => new FileInfo(ref bodyInput, context),

            OsType.Lnam => new LingoNames(ref bodyInput, context),
            OsType.Lscr => new LingoScript(ref bodyInput),
            OsType.Lctx or OsType.LctX => new LingoContext(ref bodyInput, context),

            OsType.CASPtr => new CastMap(ref bodyInput, context),
            OsType.CASt => new CastMemberProperties(ref bodyInput, context),

            OsType.SCRF => new ScoreReference(ref bodyInput, context),
            OsType.Sord => new ScoreOrder(ref bodyInput, context),
            OsType.CLUT => new Palette(ref bodyInput, context),
            OsType.STXT => new StyledText(ref bodyInput, context),

            OsType.snd => new SoundData(ref bodyInput),

            OsType.Fmap => new FontMap(ref bodyInput, context),

            OsType.GRID => new Grid(ref bodyInput, context),
            OsType.FCOL => FavoriteColors.Read(ref bodyInput, context),

            OsType.BITD => new BitmapData(ref bodyInput, context),

            _ => new UnknownResource(ref bodyInput, context, kind)
        };
    }
}
