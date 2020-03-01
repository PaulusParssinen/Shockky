using System;
using System.Collections.Generic;

using Shockky.Lingo.Instructions;

namespace Shockky.Lingo.ControlFlow
{
    public class ControlFlowGraph
    {
        //public BasicBlock Entry => Blocks[0];
        public IList<BasicBlock> Blocks { get; set; }

        //Graph G = (B, E) where B = Blocks
        //and where E = Edges =>
        //(BasicBlock predecessor, BasicBlock succcessor)[] Edges = (b_i, b_j)

        public ControlFlowGraph()
        { }

        public static ControlFlowGraph Create(LingoCode code)
        {
            //TODO: Remove temp comment rambling
            //TODO: For later tranforms, check BBs which are "pure" and only exist as a exit and as "a jumper". BB.Body.Count == 1
            //TODO: Number the blocks topologically or nah? BB.Ordinal?

            var builder = new ControlFlowGraphBuilder();

            var forwardBranches = new Dictionary<Instruction, BasicBlock>(code.JumpExits.Count);
            var backEdges = new Dictionary<Jumper, BasicBlock>();

            Range currentBlockRange = default; //0..0
            for (int i = 0; i < code.Count; i++)
            {
                Instruction instruction = code[i];

                //We are hunting here for those dangling JumpExits. If we find one which is a back-edge exit, we break and leave that to be targeted for future.
                if (code.JumpExits.ContainsValue(instruction)) //TODO: if the jumpExit OP = Return -> BBKind.Exit, or have another round for transformations like that? //TODO: Check how much ContainsValue slows this down
                {
                    currentBlockRange = currentBlockRange.End..i;

                    if (!currentBlockRange.Equals(i..i))
                    {
                        builder.CurrentBlock.Body = code[currentBlockRange];
                    }

                    if (forwardBranches.TryGetValue(instruction, out var successor))
                    {
                        //Resolving the forward-edge
                        if (builder.CurrentBlock.FallThrough == null) //TODO: Doesn't feel right? Maybe abuse the shit out of CurrentBlock getter-setter?
                        {
                            builder.AppendBlock(successor); //If
                        }
                        else builder.AddBlock(successor); //usually If-Else
                    }
                    else
                    {
                        //Back-edge target
                        var nextBlock = new BasicBlock();

                        backEdges.Add(code.GetJumperEntry(instruction), nextBlock);
                        builder.AppendBlock(nextBlock);
                    }
                }

                if (Jumper.IsValid(instruction.OP))
                {
                    Jumper jumper = (Jumper)instruction;

                    currentBlockRange = currentBlockRange.End..(i + 1);
                    builder.CurrentBlock.Body = code[currentBlockRange];

                    if (code.IsBackwardsJump(jumper))
                    {
                        //Fallthrough back-edge jumper
                        builder.LinkBlocks(builder.CurrentBlock, backEdges[jumper]);
                    }
                    else
                    {
                        //Forward jump to successor
                        if (!forwardBranches.TryGetValue(code.JumpExits[jumper], out BasicBlock successor))
                            forwardBranches.Add(code.JumpExits[jumper], successor = new BasicBlock());

                        builder.LinkBlocks(builder.CurrentBlock, successor);

                        if (Jumper.IsConditional(instruction.OP))
                        {
                            builder.AppendBlock(new BasicBlock(), isConditionalBranch: true);
                        }
                    }
                }
            }

            currentBlockRange = currentBlockRange.End..code.Count;
            builder.CurrentBlock.Body = code[currentBlockRange];

            builder.AppendBlock(new BasicBlock(BasicBlockKind.Exit)); //TODO:

            return new ControlFlowGraph()
            {
                Blocks = builder.Blocks
            };
        }
    }
}
