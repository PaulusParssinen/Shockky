using Shockky.IO;
using Shockky.Resources;
using Shockky.Resources.Cast;

using Xunit;

namespace Shockky.Tests.Resources;

public class GeneratedSerializationTests
{
    [Fact]
    public void FileInfoResource_RoundTrips()
    {
        var resource = new FileInfoResource
        {
            Header = new FileInfoResource.HeaderData
            {
                EventHandlers = MovieEventHandlers.MouseUp | MovieEventHandlers.StepMovie,
                Flags = FileInfoFlags.PauseWhenUnfocused | FileInfoFlags.AllowOutdatedLingo,
                ScriptContextNum = 42,
            },
            MovieScriptText = "go",
            CreatedBy = "A\u00E9",
            ModifiedBy = string.Empty,
            FilePath = null,
            Preload = CastPreloadStrategy.BeforeFirstFrame,
            SharedCastLibNum = 7,
            OldSharedMinCast = null,
            NewSharedMinCast = 9,
        };

        FileInfoResource roundTripped = WriteAndRead(resource, FileInfoResource.Read);

        Assert.Equal(resource.Header.EventHandlers, roundTripped.Header.EventHandlers);
        Assert.Equal(resource.Header.Flags, roundTripped.Header.Flags);
        Assert.Equal(resource.Header.ScriptContextNum, roundTripped.Header.ScriptContextNum);
        Assert.Equal(resource.MovieScriptText, roundTripped.MovieScriptText);
        Assert.Equal(resource.CreatedBy, roundTripped.CreatedBy);
        Assert.Equal(resource.ModifiedBy, roundTripped.ModifiedBy);
        Assert.Equal(resource.FilePath, roundTripped.FilePath);
        Assert.Equal(resource.Preload, roundTripped.Preload);
        Assert.Equal(resource.SharedCastLibNum, roundTripped.SharedCastLibNum);
        Assert.Equal(resource.OldSharedMinCast, roundTripped.OldSharedMinCast);
        Assert.Equal(resource.NewSharedMinCast, roundTripped.NewSharedMinCast);
    }

    [Fact]
    public void CastMemberMetadata_RoundTrips()
    {
        var resource = new CastMemberMetadata
        {
            Header = new CastMemberMetadata.MetadataHeader
            {
                LegacyFlags = 0x01020304,
                Flags = CastMemberInfoFlags.LinkedFile,
            },
            ScriptText = "on mouseDown\nend",
            Name = string.Empty,
            XtraName = string.Empty,
            ClipboardFormat = "clip",
            CreationDate = 123456,
            ModifiedBy = string.Empty,
        };

        CastMemberMetadata roundTripped = WriteAndRead(resource, CastMemberMetadata.Read);

        Assert.Equal(resource.ScriptText, roundTripped.ScriptText);
        Assert.Equal(resource.Name, roundTripped.Name);
        Assert.Equal(resource.FilePath, roundTripped.FilePath);
        Assert.Equal(resource.FileName, roundTripped.FileName);
        Assert.Equal(resource.FileType, roundTripped.FileType);
        Assert.Equal(resource.XtraName, roundTripped.XtraName);
        Assert.Equal(resource.ClipboardFormat, roundTripped.ClipboardFormat);
        Assert.Equal(resource.CreationDate, roundTripped.CreationDate);
        Assert.Equal(resource.ModifiedDate, roundTripped.ModifiedDate);
        Assert.Equal(resource.ModifiedBy, roundTripped.ModifiedBy);
        Assert.Equal(resource.Comments, roundTripped.Comments);
    }

    private delegate T ReadDelegate<T>(ref ShockwaveReader input, ReaderContext context);

    private static T WriteAndRead<T>(T resource, ReadDelegate<T> read) where T : IShockwaveItem
    {
        WriterOptions writerOptions = new(DirectorVersion.V1201);
        ReaderContext readerContext = new(DirectorVersion.V1201);

        int size = resource.GetBodySize(writerOptions);
        byte[] buffer = new byte[size];
        var output = new ShockwaveWriter(buffer, reverseEndianness: false);

        resource.WriteTo(output, writerOptions);

        var input = new ShockwaveReader(buffer, reverseEndianness: false);
        T roundTripped = read(ref input, readerContext);
        Assert.Equal(size, input.Position);

        return roundTripped;
    }
}