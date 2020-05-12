using System;
using System.Collections;
using System.Collections.Generic;

using Shockky.IO;
using Shockky.Lingo.Instructions;

namespace Shockky.Lingo
{
    public class LingoCode : ShockwaveItem, IList<Instruction>
    {
        private readonly LingoHandlerBody _body;
        private readonly List<Instruction> _instructions;

        private readonly Dictionary<Instruction, int> _indices;
        private readonly Dictionary<OPCode, List<Instruction>> _opGroups;

        public Dictionary<Jumper, Instruction> JumpExits { get; }

        public int Count => _instructions.Count;
        public bool IsReadOnly => false;
        
        public Instruction this[int index]
        {
            get => _instructions[index];
            set => _instructions[index] = value;
        }
        public Instruction this[Index index]
        {
            get => _instructions[index];
            set => _instructions[index] = value;
        }
        public Instruction[] this[Range range]
        {
            get
            {
                var (start, end) = (range.Start.GetOffset(_instructions.Count), range.End.GetOffset(_instructions.Count));
                var instructions = new Instruction[end - start];

                CopyTo(start, instructions, 0, instructions.Length);
                return instructions;
            }
        }

        public LingoCode(LingoHandlerBody body)
        {
            _body = body;

            _instructions = new List<Instruction>();
            _indices = new Dictionary<Instruction, int>();
            _opGroups = new Dictionary<OPCode, List<Instruction>>();

            JumpExits = new Dictionary<Jumper, Instruction>();

            LoadInstruction();
        }
        
        public void LoadInstruction()
        {
            var marks = new Dictionary<long, Instruction>();
            var sharedExits = new Dictionary<long, List<Jumper>>();

            var input = new ShockwaveReader(_body.Code);
            while (input.IsDataAvailable)
            {
                long previousPosition = input.Position;
                var instruction = Instruction.Create(_body.Handler, ref input);
                marks[previousPosition] = instruction;

                _instructions.Add(instruction);

                if (instruction == null) continue; //TODO: TEMPORARY DEBUGGING MEASURE

                _indices.Add(instruction, _indices.Count);

                if (!_opGroups.TryGetValue(instruction.OP, out List<Instruction> instructions))
                {
                    instructions = new List<Instruction>();
                    _opGroups.Add(instruction.OP, instructions);
                }
                instructions.Add(instruction);
                
                if (sharedExits.TryGetValue(previousPosition, out List<Jumper> jumpers))
                {
                    foreach (Jumper jumper in jumpers)
                    {
                        JumpExits.Add(jumper, instruction);
                    }
                    sharedExits.Remove(previousPosition);
                }

                if (Jumper.IsValid(instruction.OP))
                {
                    var jumper = (Jumper)instruction;
                    if (jumper.Offset == 0) continue;

                    long exitPosition = instruction.OP == OPCode.EndRepeat ?
                        previousPosition - jumper.Offset :
                        previousPosition + jumper.Offset;

                    if (exitPosition == input.Length) continue;

                    jumpers = null;
                    if (!sharedExits.TryGetValue(exitPosition, out jumpers))
                    {
                        jumpers = new List<Jumper>();
                        sharedExits.Add(exitPosition, jumpers);
                    }
                    jumpers.Add(jumper);
                }
            }

            foreach ((long exitOffset, List<Jumper> jumpers) in sharedExits)
            {
                foreach (var jump in jumpers)
                {
                    JumpExits.Add(jump, marks[exitOffset]);
                }
            }
        }

        public bool IsBackwardsJump(Jumper jumper)
        {
            //TODO: jumper.OP == OPCode.EndRepeat
            return (IndexOf(jumper) > IndexOf(JumpExits[jumper]));
        }
        public Jumper GetJumperEntry(Instruction exit)
        {
            foreach (KeyValuePair<Jumper, Instruction> jumpExit in JumpExits)
            {
                if (jumpExit.Value != exit) continue;
                return jumpExit.Key;
            }
            return null;
        }
        public Instruction[] GetJumpBlock(Jumper jumper)
        {
            if (jumper.OP == OPCode.EndRepeat)
                throw new InvalidOperationException("Backward jump blocks are not supported.");

            int blockStart = (_indices[jumper] + 1);
            int scopeEnd = _indices[JumpExits[jumper]];

            return this[blockStart..scopeEnd];
        }
        public int IndexOf(OPCode op)
        {
            if (_opGroups.ContainsKey(op))
            {
                List<Instruction> instructions = _opGroups[op];
                return _indices[instructions[0]];
            }
            return -1;
        }
        public int IndexOf(Instruction instruction)
        {
            if (_indices.ContainsKey(instruction))
            {
                return _indices[instruction];
            }
            return -1;
        }
        
        public void Add(Instruction item)
        {
            _instructions.Add(item);
        }

        public void Clear()
        {
            _instructions.Clear();
            _opGroups.Clear();
            _indices.Clear();

            JumpExits.Clear();
        }

        public bool Contains(Instruction item)
        {
            return _instructions.Contains(item);
        }

        public void CopyTo(Instruction[] array)
        {
            CopyTo(array, 0);
        }
        public void CopyTo(Instruction[] array, int arrayIndex)
        {
            CopyTo(0, array, arrayIndex, array.Length);
        }
        public void CopyTo(int index, Instruction[] array, int arrayIndex, int count)
        {
            _instructions.CopyTo(index, array, arrayIndex, count);
        }

        public bool Remove(Instruction item)
        {
            return _instructions.Remove(item);
        }

        public void Insert(int index, Instruction item)
        {
            _instructions.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            _instructions.RemoveAt(index);
        }

        public override int GetBodySize() => _body.Code.Length;

        public override void WriteTo(ShockwaveWriter output)
        {
            foreach (var instruction in _instructions)
            {
                instruction.WriteTo(output);
            }
        }

        public IEnumerator<Instruction> GetEnumerator() => _instructions.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
