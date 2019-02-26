namespace Shockky.Lingo.Bytecode.Instructions
{
    public class GetLocalIns : Instruction
    {
        public int NameIndex => Value;
        public string Name => Pool.NameList[Handler.Locals[NameIndex]];
        
        public GetLocalIns(LingoHandler handler)
            : base(OPCode.GetLocal, handler)
        { }

        public GetLocalIns(LingoHandler handler, int localIndex)
            : this(handler)
        {
            Value = localIndex;
            //TODO: Implement this shit
            //Handler.Locals.Add, also adjust its index to be under int16 in namelist
        }

        public override int GetPopCount() => 1;
    }
}