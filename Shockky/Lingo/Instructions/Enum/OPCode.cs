namespace Shockky.Lingo.Instructions
{
    public enum OPCode : byte
    {
        Return = 0x01,
        PushInt0 = 0x03,
        Multiple,
        Add,
        Substract,
        Divide,
        Modulo,
        Inverse,
        JoinString,
        JoinPadString,
        LessThan,
        LessThanEquals,
        NotEqual,
        Equals,
        GreaterThan,
        GreaterEquals,
        And,
        Or,
        Not,
        ContainsString,
        StartsWith,
        SplitString, //ChunkExpression
        Hilite,
        OntoSprite,
        IntoSprite,
        CastString,
        StartObject, //tell, startscope
        StopObject,
        WrapList,
        NewPropList,

        Swap = 0x21,
        ExecuteJavascript = 0x26,

        //Multi

        PushInt = 0x41,
        NewArgList,
        NewList,
        PushConstant,
        PushSymbol,
        PushObject,
        Op_47,
        Op_48,
        GetGlobal,
        GetProperty,
        GetParameter,
        GetLocal,
        SetGlobal = 0x4f,
        SetProperty,
        SetParameter,
        SetLocal,
        Jump,
        EndRepeat,
        IfTrue, //AVM2 spec would call this IfFalse
        CallLocal,
        CallExternal,
        CallObjOld, //?
        InsertString, //TODO: ChunkExpressions
        Insert,
        DeleteString,
        Get,
        Set,
        GetMovieProp = 0x5f,
        SetMovieProp,
        GetObjProp,
        SetObjProp,
        Op_63,
        Dup,
        Pop,
        GetMovieInfo,
        CallObj,
        Op_6d = 0x6d,
        PushInt2,
        PushInt3,
        GetSpecial, //TODO:
        PushFloat,
        Op_72,
        Op_7d = 0x7d
    }
}