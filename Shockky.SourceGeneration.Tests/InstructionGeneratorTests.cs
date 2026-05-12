using static Shockky.SourceGeneration.Tests.GeneratorTestHelper;

namespace Shockky.SourceGeneration.Tests;

public sealed class InstructionGeneratorTests
{
    [Fact]
    public Task OpWithoutImmediate()
    {
        // lang=C#
        string source = """
            public enum OPCode : byte
            {
                [OP] Return = 0x01,
            }
            """;

        return Verify(CreateDriver<InstructionGenerator>(source));
    }

    [Fact]
    public Task OpWithImmediate()
    {
        // lang=C#
        string source = """
            public enum OPCode : byte
            {
                [OP(ImmediateKind.Integer)] PushInt = 0x41,
            }
            """;

        return Verify(CreateDriver<InstructionGenerator>(source));
    }

    [Fact]
    public Task SharedReadLogic()
    {
        // lang=C#
        string source = """
            public enum OPCode : byte
            {
                [OP] Return = 0x01,
                [OP(ImmediateKind.Integer)] PushInt = 0x41,
            }
            """;

        return Verify(CreateDriver<InstructionGenerator>(source));
    }
}