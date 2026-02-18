using Shockky.Resources;
using Shockky.Resources.Types;

namespace Shockky;

public interface IRiff
{
    MemoryMapResource MemoryMap { get; }
    IDictionary<ResourceId, int> ResourceMap { get; }
}