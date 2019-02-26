namespace Shockky.Chunks
{
    public static class EnumExtensions
    {
        public static string ToFourCC(this ChunkKind chunkKind)
        {
            return chunkKind.ToString()
                .Replace("Star", "*")
                .PadRight(4);
        }

        public static ChunkKind ToChunkKind(this string chunkName)
        {
            if (System.Enum.TryParse(chunkName.Replace("*", "Star").Replace(" ", string.Empty), out ChunkKind chunkKind))
                return chunkKind;
            return ChunkKind.Unknown;
        }

        public static CodecKind ToCodec(this string codec)
        {
            if (System.Enum.TryParse(codec, out CodecKind codecKind))
                return codecKind;
            return CodecKind.Unknown;
        }
    }
}
