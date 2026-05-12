using System.Runtime.CompilerServices;

using Shockky.Lingo.Instructions;

namespace Shockky.SourceGeneration.Tests;

static class ModuleInitializer
{
    [ModuleInitializer]
    public static void Init()
    {
        // Ensure referenced assemblies are loaded before any test enumerates AppDomain.GetAssemblies()
        RuntimeHelpers.RunClassConstructor(typeof(ShockwaveItemAttribute).TypeHandle);
        RuntimeHelpers.RunClassConstructor(typeof(OPAttribute).TypeHandle);

        VerifySourceGenerators.Initialize();
        DerivePathInfo((sourceFile, projectDirectory, type, method) =>
            new(Path.Combine(projectDirectory, "Snapshots"), type.Name, method.Name));
    }
}