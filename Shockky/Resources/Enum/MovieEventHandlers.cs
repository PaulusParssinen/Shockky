namespace Shockky.Resources;

/// <summary>
/// The event handlers registered in the movie script.
/// </summary>
[Flags]
public enum MovieEventHandlers : int
{
    None = 0,
    MouseUp = 1 << 0,
    MouseDown = 1 << 1,
    Idle = 1 << 2,
    StartMovie = 1 << 3,
    StopMovie = 1 << 4,
    StepMovie = 1 << 6
}