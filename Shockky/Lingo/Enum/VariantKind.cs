namespace Shockky.Lingo;

public enum VariantKind : int
{
    Null = 0,
    String = 1,
    Void = 2,
    XObject = 3,
    Integer = 4,
    Picture = 5,
    Object = 6,
    Symbol = 8,
    Float = 9,
    CompiledJavascript = 11

    //TODO: Lval
    // CastMemberScript = 101
    // ScriptInstance = 102
    // List = 103        |
    // SortedList = 104  |-len prefixed arrays
    // Point = 105       |
    // Rect = 106        |
    // Proplist = 107
    // SortedProplit = 108
    // "109-111 stubbed on windows, probably xlib" -csnover
}