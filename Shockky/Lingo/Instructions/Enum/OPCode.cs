namespace Shockky.Lingo.Instructions
{
    public enum OPCode : byte
    {
        Return = 0x01,
        //TODO: 0x02 has been spotted as another return instruction - ScummVM
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
        IfFalse,
        CallLocal,
        CallExternal,
        CallObjOld, //?
        InsertString, //TODO: ChunkExpressions
        Insert,
        DeleteString,
        Get,
        Set,
        Op_5d,
        GetMovieProp,
        SetMovieProp,
        GetObjProp,
        SetObjProp,
        Op_63, //seems to be scoped call
        Dup,
        Pop,
        GetMovieInfo, //"push path"
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