namespace Shockky.Resources;

[Flags]
public enum LingoScriptFlags : int
{
    ScriptTextIsOwned = 1 << 0,
    EventHandler = 1 << 1,
    VarsGlobal = 1 << 2, // scumm: no local vars? - csnover: Score and eval related
    King6Related = 1 << 3,
    IsFactory = 1 << 4,
    Unk20 = 1 << 5,
    NumericExpression = 1 << 6,
    Deleted = 1 << 7,
    HasFactory = 1 << 8,
    Flag200_SCORE_RELATED = 1 << 9,
    EventScript2 = 1 << 10, //RESERVED_FIRST_SCRIPT_FN_MAYBE 
}