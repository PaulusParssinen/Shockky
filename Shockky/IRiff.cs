using Shockky.Resources;
using Shockky.Resources.Types;

namespace Shockky;

public interface IRiff
{
    MemoryMap MemoryMap { get; }
    IDictionary<ResourceId, int> ResourceMap { get; }
}