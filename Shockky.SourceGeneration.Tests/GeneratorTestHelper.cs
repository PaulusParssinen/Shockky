using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Shockky.SourceGeneration.Tests;

internal static class GeneratorTestHelper
{
    public static GeneratorDriver CreateDriver<T>(string source) where T : IIncrementalGenerator, new()
    {
        IEnumerable<MetadataReference> references =
            from assembly in AppDomain.CurrentDomain.GetAssemblies()
            where !assembly.IsDynamic
            select MetadataReference.CreateFromFile(assembly.Location);

        SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(source, CSharpParseOptions.Default.WithLanguageVersion(LanguageVersion.CSharp12));

        CSharpCompilation compilation = CSharpCompilation.Create(
            "TestAssembly",
            [syntaxTree],
            references,
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

        return CSharpGeneratorDriver.Create(new T())
            .WithUpdatedParseOptions((CSharpParseOptions)compilation.SyntaxTrees.First().Options)
            .RunGenerators(compilation);
    }
}