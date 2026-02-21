using System.Diagnostics.CodeAnalysis;

namespace Shockky;

/// <summary>
/// Skip bytes before reading this property.
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
public sealed class PadBeforeAttribute(int bytes) : Attribute
{
    public int Bytes { get; } = bytes;
}

/// <summary>
/// Skip bytes after reading this property.
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
public sealed class PadAfterAttribute(int bytes) : Attribute
{
    public int Bytes { get; } = bytes;
}

/// <summary>
/// Conditionally read this property based on a runtime expression.
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
public sealed class ConditionAttribute([StringSyntax("C#")] string expression) : Attribute
{
    public string Expression { get; } = expression;
}

/// <summary>
/// Specifies how to parse the string property value.
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
public sealed class ParseStringAsAttribute(StringParseKind kind) : Attribute
{
    public StringParseKind Kind { get; } = kind;
}

/// <summary>
/// Specifies parsing strategy for a string property.
/// </summary>
public enum StringParseKind
{
    /// <summary>Infer from property type.</summary>
    Default = 0,
    /// <summary>Pascal-style string (length-prefixed byte).</summary>
    PString,
    /// <summary>Null-terminated string.</summary>
    CString,
    /// <summary>Read bytes as raw string.</summary>
    FixedBytes
}