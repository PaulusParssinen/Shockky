namespace Shockky.Lingo.Bytecode.Instructions
{
    public class NewListIns : Instruction
    {
        public int ItemCount => Value;
        public bool IsArgumentList { get; }

        public NewListIns(bool argList)
            : base(argList ? OPCode.NewArgList : OPCode.NewList)
        {
            IsArgumentList = argList;
        }
        public NewListIns(LingoHandler handler, bool argList)
            : base(argList ? OPCode.NewArgList : OPCode.NewList, handler)
        {
            IsArgumentList = argList;
        }
        public NewListIns(LingoHandler handler, int itemCount, bool argList)
            : this(handler, argList)
        {
            Value = itemCount;
        }

        public override int GetPopCount() => ItemCount;
        public override int GetPushCount() => 1;

        public override void AcceptVisitor(InstructionVisitor visitor)
        {
            visitor.VisitNewListInstruction(this);
        }
        public override void AcceptVisitor<TContext>(InstructionVisitor<TContext> visitor, TContext context)
        {
            visitor.VisitNewListInstruction(this, context);
        }
        public override T AcceptVisitor<TContext, T>(InstructionVisitor<TContext, T> visitor, TContext context)
        {
            return visitor.VisitNewListInstruction(this, context);
        }
    }
}