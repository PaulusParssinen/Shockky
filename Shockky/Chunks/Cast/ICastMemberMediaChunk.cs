using Shockky.Chunks.Cast;

namespace Shockky.Chunks
{
    public interface ICastMemberMediaChunk<TProperties>
        where TProperties : ICastTypeProperties
    {
        void PopulateMedia(TProperties castProperties);
    }
}
