using Shockky.IO;

namespace Shockky.Resources;

public sealed class AfterburnerMapResource : IShockwaveItem, IResource
{
    public OsType Kind => OsType.ABMP;

    public int Unknown { get; set; }
    public int Unknown2 { get; set; }
    public int LastIndex { get; set; }
    public Dictionary<int, AfterburnerMapEntry> Entries { get; set; }

    public AfterburnerMapResource(ref ShockwaveReader input, ReaderContext context)
    {
        input.ReadByte();
        Unknown = input.Read7BitEncodedInt();

        using ZLibShockwaveReader deflaterInput = ZLib.CreateDeflateReaderUnsafe(ref input);
        Unknown2 = deflaterInput.Read7BitEncodedInt();
        LastIndex = deflaterInput.Read7BitEncodedInt();

        int count = deflaterInput.Read7BitEncodedInt();
        Entries = new Dictionary<int, AfterburnerMapEntry>(count);
        for (int i = 0; i < count; i++)
        {
            var entry = new AfterburnerMapEntry(deflaterInput);
            Entries[entry.Index] = entry;
        }
    }

    public int GetBodySize(WriterOptions options) => throw new NotImplementedException();
    public void WriteTo(ShockwaveWriter output, WriterOptions options) => throw new NotImplementedException();
}