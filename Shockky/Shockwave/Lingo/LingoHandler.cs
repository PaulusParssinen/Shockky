using System.Collections.Generic;
using System.Diagnostics;
using Shockky.IO;
using Shockky.Shockwave.Lingo.Bytecode;

namespace Shockky.Shockwave.Lingo
{
    public class LingoHandler : ShockwaveItem
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

        public List<Instruction> Instructions { get; }
        
        public Stack<Instruction> Expressions { get; }

        public LingoScript Script { get; }

        public LingoHandler(LingoScript script, ref ShockwaveReader input)
        {
            _input = input;
            Script = script;

            NameList = script.Names;

            Expressions = new Stack<Instruction>();
            Instructions = new List<Instruction>(); 

            Name = NameList[input.ReadInt16(true)];
            HandlerVectorPosition = input.ReadInt16(true);

            CodeLength = input.ReadInt32(true);
            CodeOffset = input.ReadInt32(true);

            int argumentsCount = input.ReadInt16(true);
            int argumentsOffset = input.ReadInt32(true);

            int localsCount = input.ReadInt16(true);
            int localsOffset = input.ReadInt32(true);

            input.ReadInt16(true); //unk1Count
            input.ReadInt32(true); //unk1Offset

            input.ReadInt32(true);
            input.ReadInt16(true);

            LineCount = input.ReadInt16(true);
            LineOffset = input.ReadInt32(true);

            StackHeight = input.ReadInt32(true); //TODO: lets see if this isreal

            Arguments = input.MapNameList(argumentsCount, argumentsOffset, NameList, true);
            Locals = input.MapNameList(localsCount, localsOffset, NameList, true);
            
        }

        public void LoadInstructions()
        {
            _input.Position = CodeOffset;

            while (_input.Position < CodeOffset + CodeLength)
            {
                var ins = Instruction.Create(this, ref _input);
                Instructions.Add(ins);
            }

            Translate(); //TODO: Move bitch
        }

        public void Translate()
        {
            foreach (var ins in Instructions)
            {
                ins.Translate();

                if(!ins.IsStatement)
                    Expressions.Push(ins);

                Debug.WriteLine((ins.IsStatement ? "Statement: ": "Expression: ") + ins);
            }
        }
    }
}
