namespace Shockky.Chunks
{
    /// <summary>
    /// All Director's built-in ink definitions.
    /// </summary>
    public enum InkType
    {
        /// <summary>
        /// Displays all the original colors in a sprite. All colors, including white, are opaque unless the image contains alpha channel effects (transparency). Copy is the default ink and is useful for backgrounds or for sprites that don’t appear in front of other artwork. If the cast member is not rectangular, a white box appears around the sprite when it passes in front of another sprite or appears on a nonwhite background. Sprites with the Copy ink animate faster than sprites with any other ink.
        /// </summary>
        Copy,

        /// <summary>
        /// Makes all light colors transparent so you can see lighter objects beneath the sprite.
        /// </summary>
        Transparent,

        /// <summary>
        /// Reverses overlapping colors. When applied to the foreground sprite, where colors overlap, the upper color changes to the chromatic opposite (based on the color palette currently in use) of the color beneath it. Pixels that were originally white become transparent and let the background show through unchanged. Reverse is good for creating custom masks.
        /// </summary>
        Reverse,

        /// <summary>
        /// Like <see cref="Reverse"/>, reverses overlapping colors, except nonoverlapping colors are transparent. The sprite is not visible unless it is overlapping another sprite.
        /// </summary>
        Ghost,

        /// <summary>
        /// Reverses all the colors in an image to create a chromatic negative of the original.
        /// </summary>
        NotCopy,

        /// <summary>
        /// The foreground image is first reversed, then the <see cref="Transparent"/> ink is applied.
        /// </summary>
        NotTransparent,

        /// <summary>
        /// The foreground image is first reversed, then the <see cref="Reverse"/> ink is applied.
        /// </summary>
        NotReverse,

        /// <summary>
        /// The foreground image is first reversed, then the <see cref="Ghost"/> ink is applied.
        /// </summary>
        NotGhost,

        /// <summary>
        /// Removes the white bounding rectangle around a sprite. Artwork within the boundaries is opaque. Matte, like <see cref="Mask"/>, uses more RAM than the other inks, and sprites with this ink animate more slowly than other sprites.
        /// </summary>
        Matte,

        /// <summary>
        /// Determines the exact transparent or opaque parts of a sprite. For Mask ink to work, you must place a mask cast member in the Cast window position immediately following the cast member to be masked. The black areas of the mask make the sprite opaque, and white areas are transparent. Colors between black and white are more or less transparent; darker colors are more opaque.
        /// </summary>
        Mask,

        //Not used

        /// <summary>
        /// Ensures that the sprite uses the set color blend percentage.
        /// </summary>
        Blend = 32,

        /// <summary>
        /// Similar to <see cref="Add"/>. The foreground sprite's RGB color value is added to the background sprite's RGB color value, but the color value is not allowed to exceed 255. If the value of the new color exceeds 255, the value is reduced to 255.
        /// </summary>
        AddPin,

        /// <summary>
        /// Creates a new color that is the result of adding the RGB color value of the foreground sprite to the color value of the background sprite. If the value of the two colors exceeds the maximum RGB color value (255), Director subtracts 256 from the remaining value so the result is between 0 and 255.
        /// </summary>
        Add,

        /// <summary>
        /// Subtracts the RGB color value of pixels in the foreground sprite from the value of the background sprite. The value of the new color is not allowed to be less than 0. If the value of the new color is negative, the value is set to 0.
        /// </summary>
        SubtractPin,

        /// <summary>
        /// Makes all the pixels in the background color of the selected sprite appear transparent and permits the background to be seen.
        /// </summary>
        BackgroundTransparent,

        /// <summary>
        /// Changes the effect of the Foreground and Background color properties of a sprite so that it is easy to create dramatic color effects that generally lighten an image. Lighten ink makes the colors in a sprite lighter as the background color gets darker. The foreground color tints the image to the degree allowed by the lightening.
        /// </summary>
        Lighten,

        /// <summary>
        /// Subtracts the RGB color value of the foreground sprite’s color from the RGB value of the background sprite’s color to determine the new color. If the color value of the new color is less than 0, Director adds 256 so the remaining value is between 0 and 255.
        /// </summary>
        Subtract,

        /// <summary>
        /// changes the effect of the Foreground and Background color properties of a sprite to create dramatic color effects that generally darken and tint a sprite. Darken ink makes the background color equivalent to a color filter through which the sprite is viewed on the Stage. White provides no filtering; black darkens all color to pure black. The foreground color is then added to the filtered image, which creates an effect that is similar to shining light of that color onto the image. Darken ink has no effect on a sprite if it has default foreground and background colors.
        /// </summary>
        Darken
    }
}
