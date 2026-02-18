namespace Shockky.Lingo.Instructions;

public enum BinaryOperatorKind
{
    None,
    Or,
    And,
    GreaterThan,
    GreaterThanOrEqual,
    LessThan,
    LessThanOrEqual,
    Equality,
    Inequality,
    Add,
    Subtract,
    Multiply,
    Divide,
    Modulo,
    JoinString,
    JoinPadString,
    StartsWith,
    ContainsString,
    SpriteIntersects,
    SpriteWithin
}