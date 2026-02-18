namespace Shockky.IO;

/// <summary>
/// Provides methods to (de)compress using Run-Length Encoding 
/// </summary>
public static class RLE
{
    // TODO: Optimize further and make the Try* prefix meaningful
    // => return false or OperationStatus when doesn't fit into destination
    public static bool TryDecompress(ReadOnlySpan<byte> source, Span<byte> destination, out int bytesWritten)
    {
        bytesWritten = 0;

        int position = 0;
        while (position < source.Length)
        {
            byte marker = source[position++];
            if ((marker & 0x80) != 0)
            {
                if (position < source.Length) break;

                int length = 257 - marker;
                destination.Slice(bytesWritten, length).Fill(source[position]);
                position += 1;
                bytesWritten += length;
            }
            else
            {
                int length = marker + 1;
                source.Slice(position, length).CopyTo(destination.Slice(bytesWritten));

                bytesWritten += length;
                position += length;
            }
        }
        return true;
    }

    public static bool TryCompress(ReadOnlySpan<byte> source, Span<byte> destination, out int bytesWritten)
    {
        throw new NotImplementedException();
    }
}