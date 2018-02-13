using Shockky.Shockwave.Lingo.Bytecode.Roles;

namespace Shockky.Shockwave.Lingo.AST.Interface
{
    public interface INode
    {
	    Role Role { get; }
	    INode FirstChild { get; }
	    INode NextSibling { get; }
	    bool IsNull { get; }
	}
}
