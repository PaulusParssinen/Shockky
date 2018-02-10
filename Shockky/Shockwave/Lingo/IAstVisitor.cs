using Shockky.Shockwave.Lingo.Bytecode.AST.Expressions;
using Shockky.Shockwave.Lingo.Bytecode.AST.Statements;

namespace Shockky.Shockwave.Lingo
{
	public interface IAstVisitor
	{
		//Statements
		void VisitExitStatement(ExitStatement exitStatement);
		void VisitBlockStatement(BlockStatement blockStatement);
		void VisitAssignmentStatement(AssignmentStatement assignmentStatement);

		//Expressions
		void VisitBinaryOperationExpression(BinaryOperatorExpression binaryOperatorExpression);
	}
}
