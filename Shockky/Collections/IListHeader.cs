namespace Shockky.Collections;

/// <summary>
/// Represents a Director serialized list header contract.
/// </summary>
public interface IListHeader : IShockwaveItem
{
    int Count { get; set; }
}