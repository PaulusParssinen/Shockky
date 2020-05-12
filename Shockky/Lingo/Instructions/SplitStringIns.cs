namespace Shockky.Lingo.Instructions
{
    public class SplitStringIns : Instruction
    {
        public string Body { get; private set; }
        public string Type { get; private set; }

        public SplitStringIns()
            : base(OPCode.SplitString)
        { }

        public override int GetPopCount() => 9;
        public override int GetPushCount() => 1;

        public override void AcceptVisitor(InstructionVisitor visitor)
        {
            visitor.VisitSplitStringInstruction(this);
        }
        public override void AcceptVisitor<TContext>(InstructionVisitor<TContext> visitor, TContext context)
        {
            visitor.VisitSplitStringInstruction(this, context);
        }
        public override T AcceptVisitor<TContext, T>(InstructionVisitor<TContext, T> visitor, TContext context)
        {
            return visitor.VisitSplitStringInstruction(this, context);
        }
        /* public override void Translate()
         {
             Body = Handler.Expressions.Pop().ToString();
             var lastLine = Handler.Expressions.Pop();
             var firstLine = Handler.Expressions.Pop();
             var lastItem = Handler.Expressions.Pop();
             var firstItem = Handler.Expressions.Pop();
             var lastWord = Handler.Expressions.Pop();
             var firstWord = Handler.Expressions.Pop();
             var lastChar = Handler.Expressions.Pop();
             var firstChar = Handler.Expressions.Pop();

             if (!(firstChar is PushZeroIns))
             {
                 Type = "char";
                 First = firstChar;
                 Last = lastChar;
             }
             else if (!(firstWord is PushZeroIns))
             {
                 Type = "word";
                 First = firstWord;
                 Last = lastWord;
             }
             else if (!(firstItem is PushZeroIns))
             {
                 Type = "item";
                 First = firstItem;
                 Last = lastItem;
             }
             else if (!(firstLine is PushZeroIns))
             {
                 Type = "line";
                 First = firstLine;
                 Last = lastLine;
             }else throw new NotImplementedException("ze fuck is going on over here");
         }*/
    }
}