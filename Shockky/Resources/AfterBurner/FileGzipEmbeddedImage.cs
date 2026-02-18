using Shockky.IO;

namespace Shockky.Resources;

// Contains logic to read embedded Zlib compressed resources from the FGEI resource
public static class FileGzipEmbeddedImage
{
    // TODO: Tidy up more.
    public static IDictionary<int, IResource> ReadResources(
        ref ShockwaveReader input, ReaderContext context,
        AfterburnerMap afterburnerMap, FileCompressionTypes compressionTypes)
    {
        int chunkStart = input.Position;
        var resources = new Dictionary<int, IResource>(afterburnerMap.Entries.Count);

        TryReadInitialLoadSegment(ref input, context, afterburnerMap, resources);

        foreach ((int index, AfterburnerMapEntry entry) in afterburnerMap.Entries)
        {
            if (entry.Offset < 1) continue;

            input.Position = chunkStart + entry.Offset;

            // TODO: Support more compression types: font maps, sounds.
            IResource resource = compressionTypes.CompressionTypes[entry.CompressionTypeIndex].Id.Equals(ZLib.MoaId) ?
                input.ReadCompressedResource(entry, context)
                : IResource.Read(ref input, context, entry.Kind, entry.Length);

            resources.Add(index, resource);
        }
        return resources;
    }

    private static bool TryReadInitialLoadSegment(
        ref ShockwaveReader input, ReaderContext context,
        AfterburnerMap afterburnerMap, Dictionary<int, IResource> resources)
    {
        // First entry in the AfterburnerMap must be ILS.
        AfterburnerMapEntry ilsEntry = afterburnerMap.Entries.First().Value;
        input.Advance(ilsEntry.Offset);

        // TODO: this shouldn't be here
        ReadOnlySpan<byte> compressedData = input.ReadBytes(ilsEntry.Length);

        Span<byte> decompressedData = ilsEntry.DecompressedLength <= 1024 ?
                stackalloc byte[1024].Slice(0, ilsEntry.DecompressedLength) : new byte[ilsEntry.DecompressedLength];

        ZLib.DecompressUnsafe(compressedData, decompressedData);

        var ilsReader = new ShockwaveReader(decompressedData, input.ReverseEndianness);

        while (ilsReader.IsDataAvailable)
        {
            int index = ilsReader.Read7BitEncodedInt();

            if (index < 1 || index > afterburnerMap.LastIndex)
                return false;

            AfterburnerMapEntry entry = afterburnerMap.Entries[index];
            resources.Add(index, IResource.Read(ref ilsReader, context, entry.Kind, entry.DecompressedLength));
        }
        return true;
    }
}