namespace Shockky.Resources;

[ShockwaveItem(BigEndian = false, GenerateSerialization = true)]
public readonly partial record struct Guide(Axis Axis, short Position) : IShockwaveItem<Guide>;