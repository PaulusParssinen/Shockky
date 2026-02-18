namespace Shockky.Resources;

[Flags]
public enum LingoContextFlags : short
{
    None = 0,
    NotEmptyMaybe = 1 << 0,
    RemoveFromEnvironment = 1 << 1,
    RiffChunkIndicesChangedMaybe = 1 << 2,
    EventHandlersChangedMaybe = 1 << 3,
    AllowOutdatedLingo = 1 << 5,
    UpdateMovieEnabled = 1 << 6
}