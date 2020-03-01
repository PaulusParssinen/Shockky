using System.Linq;
using System.Diagnostics;
using System.Collections.Generic;

namespace Shockky.Lingo.ControlFlow
{
    public class ControlFlowGraphBuilder
    {
        //TODO: ?????????????no thoughts, just a stateful mess

        private readonly BasicBlock _entry;
        private readonly BasicBlock _exit;

        private BasicBlock _currentBlock;
        public BasicBlock CurrentBlock
        {
            get
            {
                if (_currentBlock == null)
                    AppendBlock(new BasicBlock());

                return _currentBlock;
            }
        }

        public IList<BasicBlock> Blocks { get; }

        public ControlFlowGraphBuilder()
        {
            _entry = new BasicBlock(BasicBlockKind.Entry);
            _exit = new BasicBlock(BasicBlockKind.Exit);

            Blocks = new List<BasicBlock>();
            
            AppendBlock(_entry);
        }

        //TODO: Meh, redo this block adding logic, it's incomplete anyways
        public void AddBlock(BasicBlock block)
        {
            Blocks.Add(block);
            _currentBlock = block;
        }
        public void LinkBlocks(BasicBlock previous, BasicBlock next, bool isConditionalBranch = false)
        {
            //TODO: Except for BasiBlockKind.Exit, there is no block without FallThrough branch set, make something to ensure this or check in post-process?
            if (isConditionalBranch)
            {
                Debug.Assert(previous.Conditional == null);
                previous.Conditional = next;
            }
            else
            {
                Debug.Assert(previous.FallThrough == null);
                previous.FallThrough = next;
            }
            next.Predecessors.Add(previous);
        }

        /// <summary>
        /// Appends a basic block. If <see cref="Blocks"/> is not empty, the block will be linked to the last block either as a fallthrough or a conditional branch.
        /// </summary>
        /// <param name="isConditionalBranch"></param>
        public void AppendBlock(BasicBlock block, bool isConditionalBranch = false)
        {
            if (Blocks.Count > 0)
            {
                var previousBlock = Blocks.Last();

                LinkBlocks(previousBlock, block, isConditionalBranch);
            }

            AddBlock(block);
        }
    }
}
