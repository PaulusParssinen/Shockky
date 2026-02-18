namespace Shockky.Resources;

public enum MemberKind
{
    /// <summary>
    /// Represents static bitmap image.
    /// </summary>
    Bitmap = 1,

    /// <summary>
    /// Represents a looped section of the Score.
    /// </summary>
    FilmLoop = 2,

    /// <summary>
    /// Represents Mac Styled text.
    /// </summary>
    Text = 3,

    /// <summary>
    /// Represents a color look-up table.
    /// </summary>
    Palette = 4,

    /// <summary>
    /// Represents Macintosh PICT object.
    /// </summary>
    Picture = 5,

    /// <summary>
    /// Represents a sound chunk.
    /// </summary>
    Sound = 6,

    /// <summary>
    /// Represents a button.
    /// </summary>
    Button = 7,

    /// <summary>
    /// Represents a shape.
    /// </summary>
    Shape = 8,

    /// <summary>
    /// Represents a externally linked Director movie.
    /// </summary>
    Movie = 9,

    /// <summary>
    /// Represents a QuickTime or AVI movie
    /// </summary>
    DigitalVideo = 10,

    /// <summary>
    /// Represents a Lingo script.
    /// </summary>
    Script = 11,

    /// <summary>
    /// Represents a block of Rich Text.
    /// </summary>
    RichText = 12,

    /// <summary>
    /// Represents a Microsoft OLE object.
    /// </summary>
    OLE = 13,

    /// <summary>
    /// Represents transition and its associated properties.
    /// </summary>
    Transition = 14,

    /// <summary>
    /// Represents a Xtra component.
    /// </summary>
    Xtra = 15
}