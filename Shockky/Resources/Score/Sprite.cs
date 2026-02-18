namespace Shockky.Resources;

public class Sprite
{
    // by @csnover :)
    // D1–D3: 24
    // D4: 48
    // D5: 48
    // D6: 120
    // D7: 150
    public const int SPRITES = 150;

    public const int NON_SPRITE_CHANNELS = 6;
    public const int MIN_SPRITE = NON_SPRITE_CHANNELS;
    public const int MAX_SPRITE = SPRITES + NON_SPRITE_CHANNELS - 1;
    public const int CHANNELS = SPRITES + NON_SPRITE_CHANNELS;
    public const int SIZE = (CHANNELS + 7) / 8;
}