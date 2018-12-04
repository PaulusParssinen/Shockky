using Shockky.Shockwave.Chunks.Cast;

namespace Shockky.Shockwave.Chunks
{
    public interface ICastMemberMediaChunk<TProperties>
        where TProperties : ICastTypeProperties
    {
        void PopulateMedia(TProperties castProperties);
    }
}
