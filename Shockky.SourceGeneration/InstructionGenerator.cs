using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using Shockky.SourceGeneration.Models;
using Shockky.SourceGeneration.Helpers;
using Shockky.SourceGeneration.Extensions;
using Shockky.SourceGeneration.Diagnostics;

namespace Shockky.SourceGeneration;

[Generator(LanguageNames.CSharp)]
public sealed partial class InstructionGenerator : IIncrementalGenerator
{
    private const string OPAttributeFullName = "Shockky.Lingo.Instructions.OPAttribute";
    private const string InstructionNamespace = "Shockky.Lingo.Instructions";

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        // Gather info for all the annotated instruction definitions
        IncrementalValuesProvider<Result<InstructionInfo?>> instructionInfoResults =
            context.SyntaxProvider.ForAttributeWithMetadataName(OPAttributeFullName,
                predicate: static (node, token) => node is EnumMemberDeclarationSyntax,
                transform: static (context, token) =>
                {
                    var enumMemberDeclaration = (EnumMemberDeclarationSyntax)context.TargetNode;
                    var fieldSymbol = (IFieldSymbol)context.TargetSymbol;

                    _ = TryGetInfo(
                        enumMemberDeclaration,
                        fieldSymbol,
                        context.SemanticModel,
                        token,
                        out InstructionInfo? instructionInfo,
                        out ImmutableArray<DiagnosticInfo> diagnostics);

                    return new Result<InstructionInfo?>(instructionInfo, diagnostics);
                })
            .WithTrackingName(nameof(instructionInfoResults));

        context.ReportDiagnostics(instructionInfoResults.Select(static (item, _) => item.Errors));

        // Drop the nulls
        IncrementalValuesProvider<InstructionInfo> instructions = instructionInfoResults
            .Where(static (result) => result.Value is not null)
            .Select(static (result, token) => result.Value!);

        // Generate instruction definitions
        context.RegisterSourceOutput(instructions, static (context, instruction) =>
        {
            using IndentedTextWriter writer = new();

            writer.WriteLine($"namespace {InstructionNamespace};");
            writer.WriteLine();

            WriteInstructionSyntax(writer, instruction);

            context.AddSource($"{InstructionNamespace}.{instruction.Name}.g.cs", writer.ToString());
        });

        // Gather all instructions and generate factory method
        var allInstructions = instructions.Collect();

        context.RegisterSourceOutput(allInstructions, static (context, instructions) =>
        {
            using IndentedTextWriter writer = new();

            writer.WriteLine($"namespace {InstructionNamespace};");
            writer.WriteLine();

            WriteInstructionReadSyntax(writer, instructions);

            context.AddSource($"{InstructionNamespace}.IInstruction.Read.g.cs", writer.ToString());
        });
    }

    internal static bool TryGetInfo(
        EnumMemberDeclarationSyntax enumMemberSyntax,
        IFieldSymbol fieldSymbol,
        SemanticModel semanticModel,
        CancellationToken cancellationToken,
        [NotNullWhen(true)] out InstructionInfo? instructionInfo,
        out ImmutableArray<DiagnosticInfo> diagnostics)
    {
        // TODO: More sanity checks
        using ImmutableArrayBuilder<DiagnosticInfo> diagnosticBuilder = ImmutableArrayBuilder<DiagnosticInfo>.Rent();

        // Get ImmediateKind from the attribute if given.
        ImmediateKind immediateKind = ImmediateKind.None;

        if (fieldSymbol.TryGetAttributeWithFullyQualifiedMetadataName(OPAttributeFullName, out AttributeData? attributeData) &&
            attributeData.ConstructorArguments is [{ Value: int immediateKindValue }])
        {
            immediateKind = (ImmediateKind)immediateKindValue;
        }

        // Get instruction OP value from the assigned constant expression
        if (enumMemberSyntax.EqualsValue is null)
        {
            diagnosticBuilder.Add(
                DiagnosticDescriptors.MissingExplicitValueOnOPAttributeAnnotatedEnumMember,
                fieldSymbol,
                fieldSymbol.ContainingType, fieldSymbol.Name);

            instructionInfo = null;
            diagnostics = diagnosticBuilder.ToImmutable();

            return false;
        }

        var equalsValueExpression = enumMemberSyntax.EqualsValue.Value;
        if (semanticModel.GetOperation(equalsValueExpression) is not IOperation equalsValueOperation)
        {
            throw new ArgumentException("Failed to retrieve an operation for the equals expression");
        }

        var enumValueConstant = TypedConstantInfo.From(
            equalsValueOperation, semanticModel, equalsValueExpression, cancellationToken);

        instructionInfo = new InstructionInfo(fieldSymbol.Name, enumValueConstant, immediateKind);
        diagnostics = diagnosticBuilder.ToImmutable();

        return true;
    }
}
