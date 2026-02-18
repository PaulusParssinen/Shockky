namespace Shockky.Lingo.Instructions;

public enum OPCode : byte
{
    [OP] Return = 0x01,
    [OP] ReturnFactory = 0x02,
    [OP] PushZero = 0x03,
    [OP] Multiple = 0x04,
    [OP] Add = 0x05,
    [OP] Substract = 0x06,
    [OP] Divide = 0x07,
    [OP] Modulo = 0x08,
    [OP] Inverse = 0x09,
    [OP] JoinString = 0x0A,
    [OP] JoinPadString = 0x0B,
    [OP] LessThan = 0x0C,
    [OP] LessThanEquals = 0x0D,
    [OP] NotEqual = 0x0E,
    [OP] Equals = 0x0F,
    [OP] GreaterThan = 0x10,
    [OP] GreaterThanEquals = 0x11,
    [OP] And = 0x12,
    [OP] Or = 0x13,
    [OP] Not = 0x14,
    [OP] ContainsString = 0x15,
    [OP] StartsWith = 0x16,
    [OP] ChunkExpression = 0x17,
    [OP] Hilite = 0x18,
    [OP] OntoSprite = 0x19,
    [OP] IntoSprite = 0x1A,
    [OP] CastString = 0x1B,
    [OP] StartTell = 0x1C,
    [OP] EndTell = 0x1D,
    [OP] WrapList = 0x1E,
    [OP] NewPropList = 0x1F,
    [OP] Op_20 = 0x20,
    [OP] Swap = 0x21,
    [OP] Op_22 = 0x22, //OD: chunk_expr_related
    [OP] Op_23 = 0x23, //OD: chunk_expr_related - 
    [OP] Op_25 = 0x25,
    [OP] ExecuteJavascript = 0x26,

    // Multi
    [OP(ImmediateKind.Integer)] PushInt = 0x41,
    [OP(ImmediateKind.Integer)] PushUInt = 0x42,
    [OP(ImmediateKind.Integer)] NewList = 0x43,
    [OP(ImmediateKind.Literal)] PushLiteral = 0x44,
    [OP(ImmediateKind.NameIndex)] PushSymbol = 0x45,
    [OP(ImmediateKind.Integer)] PushObject = 0x46,

    [OP(ImmediateKind.NameIndex)] GetVar = 0x47,
    [OP(ImmediateKind.NameIndex)] GetGlobalFactory = 0x48,
    [OP(ImmediateKind.NameIndex)] GetGlobal = 0x49,
    [OP(ImmediateKind.NameIndex)] GetProperty = 0x4A,
    [OP(ImmediateKind.ArgumentIndex)] GetParameter = 0x4B,
    [OP(ImmediateKind.LocalIndex)] GetLocal = 0x4C,
    [OP(ImmediateKind.NameIndex)] SetVar = 0x4D,
    [OP(ImmediateKind.NameIndex)] SetGlobalFactory = 0x4E,
    [OP(ImmediateKind.NameIndex)] SetGlobal = 0x4F,
    [OP(ImmediateKind.NameIndex)] SetProperty = 0x50,
    [OP(ImmediateKind.ArgumentIndex)] SetParameter = 0x51,
    [OP(ImmediateKind.LocalIndex)] SetLocal = 0x52,

    [OP(ImmediateKind.Offset)] Jump = 0x53,
    [OP(ImmediateKind.NegativeOffset)] EndRepeat = 0x54, // conditional jump backwards
    [OP(ImmediateKind.Offset)] IfFalse = 0x55, // "skip if" -csnover
    [OP(ImmediateKind.ScriptIndex)] CallLocal = 0x56,
    [OP(ImmediateKind.NameIndex)] CallExternal = 0x57,
    [OP(ImmediateKind.Integer)] CallObjOld = 0x58,
    [OP(ImmediateKind.Integer)] InsertString = 0x59, //TODO: ChunkExpressions
    [OP(ImmediateKind.Integer)] Insert = 0x5A,
    [OP(ImmediateKind.Integer)] DeleteString = 0x5B,
    [OP(ImmediateKind.Integer)] Get = 0x5C,
    [OP(ImmediateKind.Integer)] Set = 0x5D,
    // [OP(ImmediateKind.Integer)] Op_5E = 0x5E,
    [OP(ImmediateKind.NameIndex)] GetMovieProp = 0x5F,
    [OP(ImmediateKind.NameIndex)] SetMovieProp = 0x60,
    [OP(ImmediateKind.NameIndex)] GetObjProp = 0x61,
    [OP(ImmediateKind.NameIndex)] SetObjProp = 0x62,
    [OP(ImmediateKind.NameIndex)] TellCall = 0x63,
    [OP(ImmediateKind.Integer)] Dup = 0x64,
    [OP(ImmediateKind.Integer)] Pop = 0x65,
    [OP(ImmediateKind.NameIndex)] GetMovieInfo = 0x66, //TODO: builtins
    [OP(ImmediateKind.Integer)] CallObj = 0x67,
    [OP(ImmediateKind.Integer)] Op_68 = 0x68, //OD: ChunkExpr
    // [OP(ImmediateKind.Integer)] Op_69, //OD
    // [OP(ImmediateKind.Integer)] Op_6A, //OD
    // [OP(ImmediateKind.Integer)] Op_6B, //OD
    // [OP(ImmediateKind.Integer)] Op_6C, //OD
    [OP(ImmediateKind.Integer)] Op_6D = 0x6D, //OD: chunk_expr(imm) => get_var_by_kind(imm)
    [OP(ImmediateKind.Integer)] PushInt16 = 0x6E,
    [OP(ImmediateKind.Integer)] PushInt32 = 0x6F,
    [OP(ImmediateKind.Integer)] GetChainedProp = 0x70,
    [OP(ImmediateKind.Integer)] PushFloat = 0x71,
    [OP(ImmediateKind.Integer)] GetTopLevelProp = 0x72,
    [OP(ImmediateKind.Integer)] Op_73 = 0x73, //OD: immediate is builtin function symbol?
    [OP(ImmediateKind.Integer)] Op_7d = 0x7d //TODO: does this exist
}