using System.Collections.Immutable;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using Shockky.SourceGeneration.Extensions;
using Shockky.SourceGeneration.Helpers;
using Shockky.SourceGeneration.Models;

namespace Shockky.SourceGeneration;

/// <summary>
/// A source generator for [ShockwaveItem] annotated types.
/// </summary>
[Generator(LanguageNames.CSharp)]
public sealed partial class ShockwaveItemGenerator : IIncrementalGenerator
{
    private const string ShockwaveItemAttributeName = "Shockky.ShockwaveItemAttribute";
    private const string EntryAttributeName = "Shockky.EntryAttribute";
    private const string PadBeforeAttributeName = "Shockky.PadBeforeAttribute";
    private const string PadAfterAttributeName = "Shockky.PadAfterAttribute";
    private const string ConditionAttributeName = "Shockky.ConditionAttribute";
    private const string ParseStringAsAttributeName = "Shockky.ParseStringAsAttribute";
    private const string FullyQualifiedIShockwaveItemName = "Shockky.IShockwaveItem";
    private const string FullyQualifiedWriterOptionsMetadataName = "Shockky.IO.WriterOptions";
    private const string FullyQualifiedShockwaveWriterMetadataName = "Shockky.IO.ShockwaveWriter";

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        // Process [ShockwaveItem] types
        IncrementalValuesProvider<Result<ShockwaveItemInfo?>> itemResults =
            context.SyntaxProvider.ForAttributeWithMetadataName(
                ShockwaveItemAttributeName,
                predicate: static (node, _) => node is TypeDeclarationSyntax { Modifiers: var m } && m.Any(Microsoft.CodeAnalysis.CSharp.SyntaxKind.PartialKeyword),
                transform: static (ctx, token) => ExtractShockwaveItemInfo(ctx, token))
            .WithTrackingName(nameof(ShockwaveItemInfo));

        context.ReportDiagnostics(itemResults.Select(static (item, _) => item.Errors));

        IncrementalValuesProvider<ShockwaveItemInfo> items = itemResults
            .Where(static r => r.Value is not null)
            .Select(static (r, _) => r.Value!);

        context.RegisterSourceOutput(items, static (ctx, item) =>
        {
            using IndentedTextWriter writer = new();
            WriteShockwaveItemReadMethod(writer, item);
            ctx.AddSource($"{item.Hierarchy.FilenameHint}.Read.g.cs", writer.ToString());
        });

        IncrementalValuesProvider<ShockwaveItemInfo> serializableItems = items
            .Where(static i => i.GenerateSerialization && (!i.HasExistingGetBodySize || !i.HasExistingWriteTo));

        context.RegisterSourceOutput(serializableItems, static (ctx, item) =>
        {
            using IndentedTextWriter writer = new();
            WriteShockwaveItemWriteMethod(writer, item);
            ctx.AddSource($"{item.Hierarchy.FilenameHint}.Write.g.cs", writer.ToString());
        });
    }
}