using Shockky.Resources;

using Xunit;

namespace Shockky.Tests.FunctionalTests;

public class DirectorEmptyMovieTests
{
    [Theory]
    [InlineData("d7", "dir")]
    [InlineData("d7", "dcr")]
    [InlineData("d8", "dir")]
    [InlineData("d8", "dcr")]
    [InlineData("d85", "dir")]
    [InlineData("d85", "dcr")]
    [InlineData("d9", "dir")]
    [InlineData("d9", "dcr")]
    [InlineData("d10", "dir")]
    [InlineData("d10", "dcr")]
    [InlineData("d11", "dir")]
    [InlineData("d11", "dcr")]
    [InlineData("d115", "dir")]
    [InlineData("d115", "dcr")]
    [InlineData("d12", "dir")]
    [InlineData("d12", "dcr")]
    public void EmptyMovie_ParsesCoreResources(string profile, string extension)
    {
        string path = Path.Combine(AppContext.BaseDirectory, "Data", profile, $"empty.{extension}");
        var movie = ShockwaveFile.Read(path);

        bool found = movie.Resources.TryGetValue(3, out var resource);
        Assert.True(found);
        Assert.IsType<KeyMapResource>(resource);

        Assert.Single(movie.Resources.Values.OfType<ConfigResource>());

        var sord = movie.Resources.Values.OfType<ScoreOrderResource>().FirstOrDefault();
        if (sord is not null)
        {
            Assert.NotNull(sord.Entries);
        }
    }

    [Theory]
    [InlineData("d7", "dir")]
    [InlineData("d7", "dcr")]
    [InlineData("d8", "dir")]
    [InlineData("d8", "dcr")]
    [InlineData("d85", "dir")]
    [InlineData("d85", "dcr")]
    [InlineData("d9", "dir")]
    [InlineData("d9", "dcr")]
    [InlineData("d10", "dir")]
    [InlineData("d10", "dcr")]
    [InlineData("d11", "dir")]
    [InlineData("d11", "dcr")]
    [InlineData("d115", "dir")]
    [InlineData("d115", "dcr")]
    [InlineData("d12", "dir")]
    [InlineData("d12", "dcr")]
    public void EmptyMovie_ConfigChecksumMatchesStoredValue(string profile, string extension)
    {
        string path = Path.Combine(AppContext.BaseDirectory, "Data", profile, $"empty.{extension}");
        var movie = ShockwaveFile.Read(path);
        var config = Assert.Single(movie.Resources.Values.OfType<ConfigResource>());
        Assert.Equal(config.Checksum, config.CalculateChecksum());
    }
}
