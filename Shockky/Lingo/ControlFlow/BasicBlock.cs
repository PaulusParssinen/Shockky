using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;

using Shockky.Lingo.Instructions;

namespace Shockky.Lingo.ControlFlow
{
    /// <summary>
    /// Represents a Lingo VM instruction sequence in the <see cref="ControlFlowGraph"/>
    /// </summary>
    [DebuggerDisplay("Kind: {Kind}")]
    public class BasicBlock : IEnumerable<Instruction> //TODO: BB<T>?
    {
        public IList<Instruction> Body { get; set; } //SEQ //Not aware of it's location in dissassembly
        public IList<BasicBlock> Predecessors { get; set; }
        
        //Successors
        public BasicBlock Conditional { get; set; }
        public BasicBlock FallThrough { get; set; }

        //public int Ordinal { get; set; }
        public BasicBlockKind Kind { get; }

        public BasicBlock(BasicBlockKind kind = BasicBlockKind.Block)
        {
            Kind = kind;

            Predecessors = new List<BasicBlock>();
        }

        public IEnumerator<Instruction> GetEnumerator() => Body.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
