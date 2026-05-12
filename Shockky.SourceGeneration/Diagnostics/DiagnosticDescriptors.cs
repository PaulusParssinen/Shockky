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

    public static readonly DiagnosticDescriptor UnsupportedShockwaveItemPropertyType = new(
        id: "SHCK0100",
        title: "Unsupported property type for Shockwave item source generation",
        messageFormat: "The property {0}.{1} has unsupported type {2} for generated Shockwave item serialization",
        category: typeof(ShockwaveItemGenerator).FullName,
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true);

    public static readonly DiagnosticDescriptor DuplicateEntryIndex = new(
        id: "SHCK0101",
        title: "Duplicate offset table entry index",
        messageFormat: "The entry property {0}.{1} uses duplicate entry index {2}",
        category: typeof(ShockwaveItemGenerator).FullName,
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true);

    public static readonly DiagnosticDescriptor ParseStringAsRequiresString = new(
        id: "SHCK0102",
        title: "ParseStringAs can only be used on string properties",
        messageFormat: "The property {0}.{1} uses [ParseStringAs] but has non-string type {2}",
        category: typeof(ShockwaveItemGenerator).FullName,
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true);

    public static readonly DiagnosticDescriptor NullableSequentialPropertyRequiresCondition = new(
        id: "SHCK0103",
        title: "Nullable sequential property needs a write condition",
        messageFormat: "The nullable sequential property {0}.{1} needs a condition or custom serialization",
        category: typeof(ShockwaveItemGenerator).FullName,
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true);

    public static readonly DiagnosticDescriptor ConditionalPropertyCannotBeSized = new(
        id: "SHCK0104",
        title: "Conditional property cannot be sized for generated write",
        messageFormat: "The conditional property {0}.{1} cannot be sized for generated write serialization",
        category: typeof(ShockwaveItemGenerator).FullName,
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true);

    public static readonly DiagnosticDescriptor NonNullableEntryMayBeAbsent = new(
        id: "SHCK0105",
        title: "Non-nullable entry property may be absent",
        messageFormat: "The entry property {0}.{1} is non-nullable but may be absent in the offset table",
        category: typeof(ShockwaveItemGenerator).FullName,
        defaultSeverity: DiagnosticSeverity.Warning,
        isEnabledByDefault: true);
}