namespace Shockky.Resources;

[Flags]
public enum CastMemberInfoFlags : int
{
    None,
    /// <summary>
    /// The cast member resource data is in a linked (external) file
    /// identified by the file path given in the metadata.
    /// </summary>
    LinkedFile = 1 << 0,
    /// <summary>
    /// A bitmap cast member should be automatically hilited when clicked.
    /// </summary>
    AutoHilite = 1 << 1,
    /// <summary>
    /// In a low memory condition, never purge the cast member from memory.
    /// </summary>
    PurgeNever = 1 << 2,
    /// <summary>
    /// In a low memory condition, purge the cast member from memory only
    /// after all possible “purge next” cast members have already been
    /// purged and there is still not enough memory.
    /// </summary>
    PurgeLast = 1 << 3,
    /// <summary>
    /// In a low memory condition, purge the cast member from memory only
    /// after all possible “purge normal” cast members have already been
    /// purged and there is still not enough memory.
    /// </summary>
    PurgeNext = PurgeNever | PurgeLast,
    /// <summary>
    /// The cast member is not muted.
    /// </summary>
    SoundOn = 1 << 4,

    Property42 = 1 << 5,
    Property40 = 1 << 6,
}