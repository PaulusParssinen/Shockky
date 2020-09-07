namespace Shockky.Lingo.Instructions
{
    public abstract class InstructionVisitor
    {
        protected abstract void Default(Instruction ins);

        public virtual void VisitReturnInstruction(ReturnIns @return) => Default(@return);
        
        public virtual void VisitCallInstruction(Call call) => Default(call);
        public virtual void VisitUnaryInstruction(Unary unary) => Default(unary);
        public virtual void VisitNewListInstruction(NewListIns newList) => Default(newList);
        public virtual void VisitPrimitiveInstruction(Primitive primitive) => Default(primitive);
        public virtual void VisitComputationInstruction(Computation computation) => Default(computation);
        public virtual void VisitVariableReferenceInstruction(VariableReference variableReference) => Default(variableReference);
        public virtual void VisitVariableAssignmentInstruction(VariableAssignment variableAssignment) => Default(variableAssignment);

        public virtual void VisitIfTrueInstruction(IfFalseIns ifFalse) => Default(ifFalse);
        public virtual void VisitSymbolInstruction(PushSymbolIns symbol) => Default(symbol);
        public virtual void VisitWrapListInstrution(WrapListIns wrapList) => Default(wrapList);
        public virtual void VisitNewPropertyListInstruction(NewPropListIns newPropList) => Default(newPropList);

        public virtual void VisitInsertInstruction(InsertIns insert) => Default(insert);
        public virtual void VisitInsertStringInstruction(InsertStringIns insertString) => Default(insertString);

        public virtual void VisitSplitStringInstruction(SplitStringIns splitString) => Default(splitString);
        public virtual void VisitLightStringInstruction(HiliteStringIns lightString) => Default(lightString);

        public virtual void VisitPopInstruction(PopIns pop) => Default(pop);
        public virtual void VisitSwapInstruction(SwapIns swap) => Default(swap);
        public virtual void VisitDuplicateInstruction(DupIns dup) => Default(dup);
    }

    public abstract class InstructionVisitor<TContext>
    {
        protected abstract void Default(Instruction ins, TContext context);

        public virtual void VisitReturnInstruction(ReturnIns @return, TContext context) => Default(@return, context);

        public virtual void VisitCallInstruction(Call call, TContext context) => Default(call, context);
        public virtual void VisitUnaryInstruction(Unary unary, TContext context) => Default(unary, context);
        public virtual void VisitNewListInstruction(NewListIns newList, TContext context) => Default(newList, context);
        public virtual void VisitPrimitiveInstruction(Primitive primitive, TContext context) => Default(primitive, context);
        public virtual void VisitComputationInstruction(Computation computation, TContext context) => Default(computation, context);
        public virtual void VisitVariableReferenceInstruction(VariableReference variableReference, TContext context) => Default(variableReference, context);
        public virtual void VisitVariableAssignmentInstruction(VariableAssignment variableAssignment, TContext context) => Default(variableAssignment, context);

        public virtual void VisitIfFalseInstruction(IfFalseIns ifFalse, TContext context) => Default(ifFalse, context);
        public virtual void VisitSymbolInstruction(PushSymbolIns symbol, TContext context) => Default(symbol, context);
        public virtual void VisitWrapListInstrution(WrapListIns wrapList, TContext context) => Default(wrapList, context);
        public virtual void VisitNewPropertyListInstruction(NewPropListIns newPropList, TContext context) => Default(newPropList, context);

        public virtual void VisitInsertInstruction(InsertIns insert, TContext context) => Default(insert, context);
        public virtual void VisitInsertStringInstruction(InsertStringIns insertString, TContext context) => Default(insertString, context);

        public virtual void VisitSplitStringInstruction(SplitStringIns splitString, TContext context) => Default(splitString, context);
        public virtual void VisitLightStringInstruction(HiliteStringIns lightString, TContext context) => Default(lightString, context);

        public virtual void VisitPopInstruction(PopIns pop, TContext context) => Default(pop, context);
        public virtual void VisitSwapInstruction(SwapIns swap, TContext context) => Default(swap, context);
        public virtual void VisitDuplicateInstruction(DupIns dup, TContext context) => Default(dup, context);
    }

    public abstract class InstructionVisitor<TContext, T>
    {
        protected abstract T Default(Instruction ins, TContext context);

        public virtual T VisitReturnInstruction(ReturnIns @return, TContext context) => Default(@return, context);

        public virtual T VisitCallInstruction(Call call, TContext context) => Default(call, context);
        public virtual T VisitUnaryInstruction(Unary unary, TContext context) => Default(unary, context);
        public virtual T VisitNewListInstruction(NewListIns newList, TContext context) => Default(newList, context);
        public virtual T VisitPrimitiveInstruction(Primitive primitive, TContext context) => Default(primitive, context);
        public virtual T VisitComputationInstruction(Computation computation, TContext context) => Default(computation, context);
        public virtual T VisitVariableReferenceInstruction(VariableReference variableReference, TContext context) => Default(variableReference, context);
        public virtual T VisitVariableAssignmentInstruction(VariableAssignment variableAssignment, TContext context) => Default(variableAssignment, context);

        public virtual T VisitIfFalseInstruction(IfFalseIns ifFalse, TContext context) => Default(ifFalse, context);
        public virtual T VisitSymbolInstruction(PushSymbolIns symbol, TContext context) => Default(symbol, context);
        public virtual T VisitWrapListInstrution(WrapListIns wrapList, TContext context) => Default(wrapList, context);
        public virtual T VisitNewPropertyListInstruction(NewPropListIns newPropList, TContext context) => Default(newPropList, context);

        public virtual T VisitInsertInstruction(InsertIns insert, TContext context) => Default(insert, context);
        public virtual T VisitInsertStringInstruction(InsertStringIns insertString, TContext context) => Default(insertString, context);

        public virtual T VisitSplitStringInstruction(SplitStringIns splitString, TContext context) => Default(splitString, context);
        public virtual T VisitLightStringInstruction(HiliteStringIns lightString, TContext context) => Default(lightString, context);

        public virtual T VisitPopInstruction(PopIns pop, TContext context) => Default(pop, context);
        public virtual T VisitSwapInstruction(SwapIns swap, TContext context) => Default(swap, context);
        public virtual T VisitDuplicateInstruction(DupIns dup, TContext context) => Default(dup, context);
    }
}
