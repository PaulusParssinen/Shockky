using Shockky.IO;

namespace Shockky.Resources;

public sealed class FileVersion : IResource, IShockwaveItem
{
    public OsType Kind => OsType.Fver;

    public DirectorVersion Version { get; set; }
    public string VersionString { get; set; }

    public FileVersion(scoped ref ShockwaveReader input, ReaderContext context)
    {
        int versionMaybeTooForgot = input.Read7BitEncodedInt();
        if (versionMaybeTooForgot < 0x401) return;
        int unk1 = input.Read7BitEncodedInt();
        Version = (DirectorVersion)input.Read7BitEncodedInt();

        if (versionMaybeTooForgot < 0x501) return;
        VersionString = input.ReadString();
    }

    public int GetBodySize(WriterOptions options)
    {
        throw new NotImplementedException();
    }
    public void WriteTo(ShockwaveWriter output, WriterOptions options)
    {
        throw new NotImplementedException();
    }

    public static FileVersion Read(ref ShockwaveReader input, ReaderContext context) => new FileVersion(ref input, context);
}