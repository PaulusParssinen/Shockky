using Microsoft.CodeAnalysis;

namespace Shockky.SourceGeneration.Diagnostics;

internal static class DiagnosticDescriptors
{
    public static readonly DiagnosticDescriptor MissingExplicitValueOnOPAttributeAnnotatedEnumMember = new(
        id: "SHCK0001",
        title: "Missing explicit numeric value on [OP] annotated enum member",
        messageFormat: "The enum member {0}.{1} annotated with [OP] attribute must have explicitly assigned numeric value",
        category: typeof(InstructionGenerator).FullName,
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true);
}