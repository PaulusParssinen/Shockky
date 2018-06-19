namespace Shockky.Shockwave.Chunks.Enum
{
    public static class EnumExtensions
    {
        public static ChunkKind ToChunkKind(this string chunkName)
        {
            if (!System.Enum.TryParse(chunkName.Replace("*", "Star").Replace(" ", string.Empty), out ChunkKind chunkKind))
                return ChunkKind.Unknown;
            return chunkKind;
        }

        public static CodecKind ToCodec(this string codec)
        {
            if (!System.Enum.TryParse(codec, out CodecKind codecKind))
                return CodecKind.Unknown;
            return codecKind;
        }
    }
}
