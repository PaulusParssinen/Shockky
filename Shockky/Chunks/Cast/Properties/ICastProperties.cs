using Shockky.IO;

namespace Shockky.Chunks.Cast
{
    public interface ICastProperties
    {
        int GetBodySize();
        void WriteTo(ShockwaveWriter output);
    }
}
