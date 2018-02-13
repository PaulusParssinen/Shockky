using Shockky.Shockwave.Lingo.AST.Expressions;
using Shockky.Shockwave.Lingo.AST.Statements;

namespace Shockky.Shockwave.Lingo
{
	public interface IAstVisitor : IVisitor
	{
		//Statements
		void VisitExitStatement(ExitStatement exitStatement);
		void VisitBlockStatement(BlockStatement blockStatement);
		void VisitAssignmentStatement(AssignmentStatement assignmentStatement);

		//Expressions
		void VisitBinaryOperationExpression(BinaryOperatorExpression binaryOperatorExpression);
	}
}
