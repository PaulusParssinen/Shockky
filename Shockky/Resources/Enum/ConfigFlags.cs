namespace Shockky.Resources;

[Flags]
public enum ConfigFlags : int
{
    None,
    MOVIE_FIELD_46 = 1 << 5,
    MapPalettes = 1 << 6,
    Legacy1 = 1 << 7,
    Legacy2 = 1 << 8,
    UpdateMovieEnabled = 1 << 9,
    PreloadEventAbort = 1 << 10
}