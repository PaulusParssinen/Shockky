using System;
using Shockky.Shockwave.Lingo.AST.Expressions;
using Shockky.Shockwave.Lingo.AST.Statements;

namespace Shockky.Shockwave.Lingo.Bytecode.Output.Visitor
{
    public class LingoBytecodeOutputVisitor : IAstVisitor
    {
	    public LingoBytecodeOutputVisitor()
	    {
		    
	    }

	    public void VisitExitStatement(ExitStatement exitStatement)
	    {
		    throw new NotImplementedException();
	    }

	    public void VisitBlockStatement(BlockStatement blockStatement)
	    {
		    throw new NotImplementedException();
	    }

	    public void VisitAssignmentStatement(AssignmentStatement assignmentStatement)
	    {
		    throw new NotImplementedException();
	    }

	    public void VisitBinaryOperationExpression(BinaryOperatorExpression binaryOperatorExpression)
	    {
		    throw new NotImplementedException();
	    }
    }
}
