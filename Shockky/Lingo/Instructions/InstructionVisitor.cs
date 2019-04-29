namespace Shockky.Lingo.Bytecode.Instructions
{
    public abstract class InstructionVisitor
    {
        protected abstract void Default(Instruction ins);
        
        public virtual void VisitCallInstruction(Call call) => Default(call);
        public virtual void VisitUnaryInstruction(Unary unary) => Default(unary);
        public virtual void VisitNewListInstruction(NewListIns newList) => Default(newList);
        public virtual void VisitPrimitiveInstruction(Primitive primitive) => Default(primitive);
        public virtual void VisitComputationInstruction(Computation computation) => Default(computation);
        public virtual void VisitVariableReferenceInstruction(VariableReference variableReference) => Default(variableReference);
        public virtual void VisitVariableAssignmentInstruction(VariableAssignment variableAssignment) => Default(variableAssignment);

        public virtual void VisitIfTrueInstruction(IfTrueIns ifTrue) => Default(ifTrue);
        public virtual void VisitWrapListInstrution(WrapListIns wrapList) => Default(wrapList);
    }

    public abstract class InstructionVisitor<TContext>
    {
        protected abstract void Default(Instruction ins, TContext context);

        public virtual void VisitCallInstruction(Call call, TContext context) => Default(call, context);
        public virtual void VisitUnaryInstruction(Unary unary, TContext context) => Default(unary, context);
        public virtual void VisitNewListInstruction(NewListIns newList, TContext context) => Default(newList, context);
        public virtual void VisitPrimitiveInstruction(Primitive primitive, TContext context) => Default(primitive, context);
        public virtual void VisitComputationInstruction(Computation computation, TContext context) => Default(computation, context);
        public virtual void VisitVariableReferenceInstruction(VariableReference variableReference, TContext context) => Default(variableReference, context);
        public virtual void VisitVariableAssignmentInstruction(VariableAssignment variableAssignment, TContext context) => Default(variableAssignment, context);

        public virtual void VisitIfTrueInstruction(IfTrueIns ifTrue, TContext context) => Default(ifTrue, context);
        public virtual void VisitWrapListInstrution(WrapListIns wrapList, TContext context) => Default(wrapList, context);
    }

    public abstract class InstructionVisitor<TContext, T>
    {
        protected abstract T Default(Instruction ins, TContext context);

        public virtual T VisitCallInstruction(Call call, TContext context) => Default(call, context);
        public virtual T VisitUnaryInstruction(Unary unary, TContext context) => Default(unary, context);
        public virtual T VisitNewListInstruction(NewListIns newList, TContext context) => Default(newList, context);
        public virtual T VisitPrimitiveInstruction(Primitive primitive, TContext context) => Default(primitive, context);
        public virtual T VisitComputationInstruction(Computation computation, TContext context) => Default(computation, context);
        public virtual T VisitVariableReferenceInstruction(VariableReference variableReference, TContext context) => Default(variableReference, context);
        public virtual T VisitVariableAssignmentInstruction(VariableAssignment variableAssignment, TContext context) => Default(variableAssignment, context);

        public virtual T VisitIfTrueInstruction(IfTrueIns ifTrue, TContext context) => Default(ifTrue, context);
        public virtual T VisitWrapListInstrution(WrapListIns wrapList, TContext context) => Default(wrapList, context);
    }
}
