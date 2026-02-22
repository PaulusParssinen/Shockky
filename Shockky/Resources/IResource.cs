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
        ReadOnlySpan<byte> resourceSpan = input.ReadBytes(length);
        var bodyInput = new ShockwaveReader(resourceSpan, input.ReverseEndianness);

        return kind switch
        {
            OsType.Fver => new FileVersionResource(ref bodyInput, context),
            OsType.Fcdr => new FileCompressionTypesResource(ref bodyInput, context),
            OsType.ABMP => new AfterburnerMapResource(ref bodyInput, context),

            OsType.imap => new IndexMapResource(ref bodyInput),
            OsType.mmap => new MemoryMapResource(ref bodyInput),
            OsType.KEYPtr => new KeyMapResource(ref bodyInput, context),
            OsType.VWCF or OsType.DRCF => new ConfigResource(ref bodyInput, context),

            // TODO: handle D9
            //OsType.VWLB => new ScoreLabels(ref bodyInput, context),
            OsType.VWFI => FileInfoResource.Read(ref bodyInput, context),

            OsType.Lnam => new LingoNamesResource(ref bodyInput, context),
            OsType.Lscr => new LingoScriptResource(ref bodyInput),
            OsType.Lctx or OsType.LctX => new LingoContextResource(ref bodyInput, context),

            OsType.MCsL => new MovieCastListResource(ref bodyInput, context),

            OsType.CASPtr => new CastMapResource(ref bodyInput, context),
            OsType.CASt => new CastMemberPropertiesResource(ref bodyInput, context),

            OsType.SCRF => new ScoreReferenceResource(ref bodyInput, context),
            OsType.Sord => new ScoreOrderResource(ref bodyInput, context),
            OsType.CLUT => new PaletteResource(ref bodyInput, context),
            OsType.STXT => new StyledTextResource(ref bodyInput, context),

            OsType.snd => new SoundDataResource(ref bodyInput),

            OsType.Fmap => new FontMapResource(ref bodyInput, context),

            OsType.GRID => new GridResource(ref bodyInput, context),
            OsType.FCOL => FavoriteColorsResource.Read(ref bodyInput, context),

            OsType.BITD => new BitmapDataResource(ref bodyInput, context),

            _ => new UnknownResource(ref bodyInput, context, kind)
        };
    }
}