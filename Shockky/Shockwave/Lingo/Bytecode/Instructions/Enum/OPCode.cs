namespace Shockky.Shockwave.Lingo.Bytecode.Instructions.Enum
{
    public enum OPCode
    {
        Return = 0x01,
        PushInt0 = 0x03, //push int 0 literal thing
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
        Contains0String, //startswith i guess
        SplitString,
        LightString,
        OnToSprite,
        IntoSprite,
        CastString,
        StartObject,
        StopObject,
        WrapList,
        NewPropList,

        Swap = 0x21,

        //Multi

        PushInt = 0x41,
        NewArgList,
        NewList,
        PushConstant,
        PushSymbol,
        GetGlobal = 0x49,
        GetProperty,
        GetParameter,
        GetLocal,
        SetGlobal = 0x4f,
        SetProperty,
        SetParameter,
        SetLocal,
        Jump,
        EndRepeat,
        IfTrue,
        CallLocal,
        CallExternal,
        CallObjOld, //?
        Op_59,
        Op_5a = 0x5a,
        Op_5b,
        Get,
        Set,
        GetMovieProp = 0x5f,
        SetMovieProp,
        GetObjProp,
        SetObjProp,
        GetMovieInfo = 0x66,
        CallObj,
        PushInt2 = 0x6e
    }
}
