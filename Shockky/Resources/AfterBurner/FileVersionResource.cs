using Shockky.IO;

namespace Shockky.Resources;

public sealed class FileVersionResource : IResource, IShockwaveItem
{
    public OsType Kind => OsType.Fver;

    public DirectorVersion Version { get; set; }
    public string VersionString { get; set; }

    public FileVersionResource(scoped ref ShockwaveReader input, ReaderContext context)
    {
        int versionMaybeTooForgot = input.Read7BitEncodedInt();
        if (versionMaybeTooForgot < 0x401) return;
        int unk1 = input.Read7BitEncodedInt();
        Version = (DirectorVersion)input.Read7BitEncodedInt();

        if (versionMaybeTooForgot < 0x501) return;
        VersionString = input.ReadPString();
    }

    public int GetBodySize(WriterOptions options)
    {
        throw new NotImplementedException();
    }
    public void WriteTo(ShockwaveWriter output, WriterOptions options)
    {
        throw new NotImplementedException();
    }

    public static FileVersionResource Read(ref ShockwaveReader input, ReaderContext context) => new FileVersionResource(ref input, context);
}