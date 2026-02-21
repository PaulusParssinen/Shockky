using System.Collections.Immutable;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using Shockky.SourceGeneration.Extensions;
using Shockky.SourceGeneration.Helpers;
using Shockky.SourceGeneration.Models;

namespace Shockky.SourceGeneration;

/// <summary>
/// A source generator for [ShockwaveItem] and [Header] annotated types.
/// </summary>
[Generator(LanguageNames.CSharp)]
public sealed partial class ShockwaveItemGenerator : IIncrementalGenerator
{
    private const string ShockwaveItemAttributeName = "Shockky.ShockwaveItemAttribute";
    private const string HeaderAttributeName = "Shockky.HeaderAttribute";
    private const string OffsetTableAttributeName = "Shockky.OffsetTableAttribute";
    private const string EntryAttributeName = "Shockky.EntryAttribute";
    private const string PadBeforeAttributeName = "Shockky.PadBeforeAttribute";
    private const string PadAfterAttributeName = "Shockky.PadAfterAttribute";
    private const string ConditionAttributeName = "Shockky.ConditionAttribute";
    private const string ParseAsAttributeName = "Shockky.ParseAsAttribute";

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

        // Process [Header] types (nested header classes)
        IncrementalValuesProvider<Result<HeaderInfo?>> headerResults =
            context.SyntaxProvider.ForAttributeWithMetadataName(
                HeaderAttributeName,
                predicate: static (node, _) => node is TypeDeclarationSyntax { Modifiers: var m } && m.Any(Microsoft.CodeAnalysis.CSharp.SyntaxKind.PartialKeyword),
                transform: static (ctx, token) => GetHeaderInfo(ctx, token))
            .WithTrackingName(nameof(HeaderInfo));

        context.ReportDiagnostics(headerResults.Select(static (item, _) => item.Errors));

        IncrementalValuesProvider<HeaderInfo> headers = headerResults
            .Where(static r => r.Value is not null)
            .Select(static (r, _) => r.Value!);

        context.RegisterSourceOutput(headers, static (ctx, header) =>
        {
            using IndentedTextWriter writer = new();
            WriteHeaderReadMethod(writer, header);
            ctx.AddSource($"{header.Hierarchy.FilenameHint}.Read.g.cs", writer.ToString());
        });
    }
}