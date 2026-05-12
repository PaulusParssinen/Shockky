; Shipped analyzer releases
; https://github.com/dotnet/roslyn-analyzers/blob/main/src/Microsoft.CodeAnalysis.Analyzers/ReleaseTrackingAnalyzers.Help.md

## Release 1.0

### New Rules

| Rule ID  | Category                                          | Severity | Notes                                              |
| -------- | ------------------------------------------------- | -------- | -------------------------------------------------- |
| SHCK0001 | Shockky.SourceGeneration.InstructionGenerator     | Error    | Missing explicit value on [OP] annotated enum member |
| SHCK0100 | Shockky.SourceGeneration.ShockwaveItemGenerator   | Error    | Unsupported property type for Shockwave item       |
| SHCK0101 | Shockky.SourceGeneration.ShockwaveItemGenerator   | Error    | Duplicate offset table entry index                 |
| SHCK0102 | Shockky.SourceGeneration.ShockwaveItemGenerator   | Error    | ParseStringAs can only be used on string properties |
| SHCK0103 | Shockky.SourceGeneration.ShockwaveItemGenerator   | Error    | Nullable sequential property needs a write condition |
| SHCK0104 | Shockky.SourceGeneration.ShockwaveItemGenerator   | Error    | Conditional property cannot be sized for generated write |
| SHCK0105 | Shockky.SourceGeneration.ShockwaveItemGenerator   | Warning  | Non-nullable entry property may be absent          |
