using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Shockky.IO;
using Shockky.Shockwave.Lingo.Bytecode.AST.Statements;

namespace Shockky.Shockwave.Lingo
{
    [DebuggerDisplay("on {Name,nq}")]
    public class LingoHandler
    {
        private ShockwaveReader _input;

        public List<string> NameList { get; }

        public List<string> Arguments { get; }
        public List<string> Locals { get; }

        public string Name { get; }

        public int HandlerVectorPosition { get; }

        public int CodeLength { get; }
        public int CodeOffset { get; }

        public int LineCount { get; }
        public int LineOffset { get; }

        public int StackHeight { get; }

        public BlockStatement HandlerBody { get; private set; }

        public LingoScript Script { get; }

        public LingoHandler(LingoScript script, ShockwaveReader input)
        {
            _input = input;
            Script = script;

            NameList = script.Names;

            Name = NameList[input.ReadBigEndian<short>()];
            HandlerVectorPosition = input.ReadBigEndian<short>(); 

	        CodeLength = input.ReadBigEndian<int>();
            CodeOffset = input.ReadBigEndian<int>();

            int argumentsCount = input.ReadBigEndian<short>();
            int argumentsOffset = input.ReadBigEndian<int>();
			 
            int localsCount = input.ReadBigEndian<short>();
            int localsOffset = input.ReadBigEndian<int>();

            input.ReadBigEndian<short>(); //unk1Count
            input.ReadBigEndian<int>(); //unk1Offset

            input.ReadBigEndian<int>();
            input.ReadBigEndian<short>();

            LineCount = input.ReadBigEndian<short>();
            LineOffset = input.ReadBigEndian<int>();

            StackHeight = input.ReadBigEndian<int>();

            long ogPosition = input.Position;

            Arguments = input.ReadBigEndianList<short>(argumentsCount, argumentsOffset)
                .Select(i => NameList[i]).ToList();

            Locals = input.ReadBigEndianList<short>(localsCount, localsOffset)
                .Select(i => NameList[i]).ToList();

            input.Position = ogPosition;

        }
		/*
        public void LoadInstructions()
        {
            _input.Position = CodeOffset;

            while (_input.Position < CodeOffset + CodeLength)
            {
                var ins = Instruction.Create(this, ref _input);
                Instructions.Add(ins);
            }
        }
*/
    }
}
