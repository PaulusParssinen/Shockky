using Shockky.Resources;

using Xunit;

namespace Shockky.Tests.FunctionalTests;

public class Director10FileTests(Director10FileTests.Fixture fixture) : IClassFixture<Director10FileTests.Fixture>
{
    public class Fixture
    {
        public ShockwaveFile EmptyFile { get; } = ShockwaveFile.Read("./Data/d101_empty.dir");
        public ShockwaveFile EmptyFileAfterburned { get; } = ShockwaveFile.Read("./Data/d101_empty.dcr");
    }

    [Fact]
    public void EmptyFile_HasKeyMapWithHardcodedResourceId()
    {
        bool found = fixture.EmptyFile.Resources.TryGetValue(3, out var resource);

        Assert.True(found);
        Assert.IsType<KeyMapResource>(resource);
    }

    [Fact]
    public void EmptyFileAfterburned_HasKeyMapWithHardcodedResourceId()
    {
        bool found = fixture.EmptyFileAfterburned.Resources.TryGetValue(3, out var resource);

        Assert.True(found);
        Assert.IsType<KeyMapResource>(resource);
    }
}