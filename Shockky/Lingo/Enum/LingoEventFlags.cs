namespace Shockky.Lingo;

[Flags]
public enum LingoEventFlags : int
{
    None = 0,
    MouseDown = 1 << 0,
    MouseUp = 1 << 1,
    KeyDown = 1 << 2,
    KeyUp = 1 << 3,
    Timeout = 1 << 4,
    MouseDoubleClick = 1 << 6,
    MouseStillDown = 1 << 7,
    MouseEnter = 1 << 8,
    MouseLeave = 1 << 9,
    MouseWithin = 1 << 10,
    Idle = 1 << 11,
    StartMovie = 1 << 12,
    StopMovie = 1 << 13,
    StepMovie = 1 << 14,
    EnterFrame = 1 << 15,
    ExitFrame = 1 << 16,
    ActivateWindow = 1 << 17,
    DeactivateWindow = 1 << 18,
    CloseWindow = 1 << 19,
    OpenWindow = 1 << 20,
    MoveWindow = 1 << 21,
    ZoomWindow = 1 << 22,
    ResizeWindow = 1 << 23,
    RightMouseDown = 1 << 24,
    RightMouseUp = 1 << 25,
}