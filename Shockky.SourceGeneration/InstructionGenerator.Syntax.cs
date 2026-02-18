using System.Collections.Immutable;

using Shockky.SourceGeneration.Extensions;
using Shockky.SourceGeneration.Helpers;
using Shockky.SourceGeneration.Models;

namespace Shockky.SourceGeneration;

public sealed partial class InstructionGenerator
{
    /// <summary>
    /// Writes syntax for a class definition of the given <paramref name="instruction"/>.
    /// </summary>
    internal static void WriteInstructionSyntax(IndentedTextWriter writer,
       InstructionInfo instruction)
    {
        bool hasImmediate = instruction.ImmediateKind is not ImmediateKind.None;

        writer.WriteGeneratedAttributes(nameof(InstructionGenerator));

        if (hasImmediate)
        {
            writer.WriteLine($"public sealed class {instruction.Name}(int immediate) : IInstruction");
        }
        else writer.WriteLine($"public sealed class {instruction.Name} : IInstruction");

        using (writer.WriteBlock())
        {
            // Offer cached instance for instruction with no immediate
            writer.WriteLineIf(!hasImmediate, $"public static readonly {instruction.Name} Default = new();");
            writer.WriteLineIf(!hasImmediate);

            writer.WriteLine($"public OPCode OP => OPCode.{instruction.Name};");
            writer.WriteLine();

            // If the OP has no immediate, hide the property representing it with explicit implementation.
            if (!hasImmediate)
            {
                writer.WriteLine("int IInstruction.Immediate { get; set; }");
            }
            else writer.WriteLine("public int Immediate { get; set; } = immediate;");
            writer.WriteLine();

            if (hasImmediate)
            {
                writer.WriteLine("""
                    public int GetSize()
                    {
                        uint imm = (uint)Immediate;
                        if (imm <= byte.MaxValue) return sizeof(OPCode) + 1;
                        else if (imm <= ushort.MaxValue) return sizeof(OPCode) + 2;
                        else return sizeof(OPCode) + 4;
                    }
                    """, true);
            }
            else writer.WriteLine("public int GetSize() => sizeof(OPCode);");

            writer.WriteLine();
            using (writer.WriteBlock("public void WriteTo(global::Shockky.IO.ShockwaveWriter output)"))
            {
                if (hasImmediate)
                {
                    writer.WriteLine("""
                        byte op = (byte)OP;

                        if (Immediate <= byte.MaxValue)
                        {
                            output.WriteByte(op);
                            output.WriteByte((byte)Immediate);
                        }
                        else if (Immediate <= ushort.MaxValue)
                        {
                            output.WriteByte((byte)(op + 0x40));
                            output.WriteUInt16LittleEndian((ushort)Immediate);
                        }
                        else
                        {
                            output.WriteByte((byte)(op + 0x80));
                            output.WriteInt32LittleEndian(Immediate);
                        }
                        """, true);
                }
                else writer.WriteLine("output.WriteByte((byte)OP);");
            }
        }
    }

    /// <summary>
    /// Writes syntax for a static factory method for deserialzing instructions.
    /// </summary>
    internal static void WriteInstructionReadSyntax(IndentedTextWriter writer, ImmutableArray<InstructionInfo> instructions)
    {
        using (writer.WriteBlock("public partial interface IInstruction"))
        {
            writer.WriteGeneratedAttributes(nameof(InstructionGenerator));
            using (writer.WriteBlock("public static IInstruction Read(ref global::Shockky.IO.ShockwaveReader input)"))
            {
                writer.WriteLine("""
                        byte op = input.ReadByte();
                        int immediate = op >> 6 switch 
                        {
                            1 => input.ReadByte(),
                            2 => input.ReadInt16LittleEndian(),
                            3 => input.ReadInt32LittleEndian(),
                            _ => 0
                        };
                        """, true);

                writer.WriteLine();
                writer.WriteLine("if (op >= 0x80) op = (byte)(op & 0x3F | 0x40);");
                using (writer.WriteBlock("return (OPCode)op switch", isExpression: true))
                {
                    foreach (var instruction in instructions.AsSpan())
                    {
                        bool hasImmediate = instruction.ImmediateKind is not ImmediateKind.None;

                        writer.Write($"OPCode.{instruction.Name} => new {instruction.Name}(");
                        writer.WriteIf(hasImmediate, "immediate");
                        writer.WriteLine("),");
                    }
                    writer.WriteLine("_ => throw null");
                }
            }
        }
    }
}