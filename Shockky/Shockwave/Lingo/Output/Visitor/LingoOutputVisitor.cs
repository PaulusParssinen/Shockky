using System;
using System.Collections.Generic;
using System.Text;
using Shockky.Shockwave.Lingo.Bytecode.AST.Expressions;
using Shockky.Shockwave.Lingo.Bytecode.AST.Statements;

namespace Shockky.Shockwave.Lingo.Output.Visitor
{
    public class LingoOutputVisitor : IAstVisitor
    {
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
