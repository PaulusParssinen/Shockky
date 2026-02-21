namespace Shockky.Resources;

public enum CastPreloadStrategy : ushort
{
    /// <summary>
    /// Do not preload cast member resource data.
    /// </summary>
    None = 0,

    /// <summary>
    /// Preload cast member resource data after the first frame is rendered.
    /// </summary>
    AfterFirstFrame = 1,

    /// <summary>
    /// Preload cast member resource data before the first frame is rendered.
    /// </summary>
    BeforeFirstFrame = 2,
    Always = 4,
}