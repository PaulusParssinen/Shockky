namespace Shockky.Lingo.Syntax
{
    public interface IAstVisitor
    {
        void VisitSyntaxTree(SyntaxTree syntaxTree);

        void VisitExitStatement(ExitStatement exitStatement);

        void VisitIfStatement(IfStatement ifStatement);
        void VisitBlockStatement(BlockStatement blockStatement);
        void VisitSwitchStatement(SwitchStatement switchStatement);
        void VisitRepeatStatement(RepeatStatement repeatStatement);
        void VisitAssigmentStatement(AssignmentStatement assignmentStatement);
        void VisitExpressionStatement(ExpressionStatement expressionStatement);

        void VisitListExpression(ListExpression listExpression);
        void VisitCallExpression(CallExpression callExpression);
        void VisitPrimitiveExpression(PrimitiveExpression primitiveExpression);
        void VisitIdentifierExpression(IdentifierExpression identifierExpression);
        void VisitArgumentListExpression(ArgumentListExpression argumentListExpression);
        void VisitUnaryOperatorExpression(UnaryOperatorExpression unaryOperatorExpression);
        void VisitBinaryOperatorExpression(BinaryOperatorExpression binaryOperatorExpression);

        void VisitMovieReferenceExpression(MovieReferenceExpression movieReferenceExpression);
        void VisitMemberReferenceExpression(MemberReferenceExpression memberReferenceExpression);
    }
}
