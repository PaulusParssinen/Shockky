using System.Diagnostics;
using System.Collections;

using Shockky.Lingo.Instructions;

namespace Shockky.Lingo.ControlFlow;

/// <summary>
/// Represents a Lingo VM instruction sequence in the <see cref="ControlFlowGraph"/>
/// </summary>
[DebuggerDisplay("Kind: {Kind}")]
public class BasicBlock : IEnumerable<IInstruction>
{
    public IList<IInstruction> Body { get; set; }
    public IList<BasicBlock> Predecessors { get; set; }

    public BasicBlock Conditional { get; set; }
    public BasicBlock FallThrough { get; set; }

    //public int Ordinal { get; set; }
    public BasicBlockKind Kind { get; }

    public BasicBlock(BasicBlockKind kind = BasicBlockKind.Block)
    {
        Kind = kind;

        Predecessors = new List<BasicBlock>();
    }

    public IEnumerator<IInstruction> GetEnumerator() => Body.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
