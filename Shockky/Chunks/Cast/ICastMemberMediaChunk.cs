using Shockky.Chunks.Cast;

namespace Shockky.Chunks
{
    public interface ICastMemberMediaChunk<TProperties>
        where TProperties : ICastProperties
    {
        void PopulateMedia(TProperties castProperties);
    }
}
