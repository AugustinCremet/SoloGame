using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RainbowColor
{
    Neutral,
    Red,
    Orange,
    Yellow,
    Green,
    Blue,
    Indigo,
    Violet,
}

public static class RainbowColorExtensions
{
    private static readonly Dictionary<RainbowColor, Color> ColorMap = new Dictionary<RainbowColor, Color>
    {
        {RainbowColor.Neutral, Color.gray },
        {RainbowColor.Red, new Color32(232, 20, 22, 255) },
        {RainbowColor.Orange, new Color32(255, 165, 0, 255) },
        {RainbowColor.Yellow, new Color32(250, 235, 54, 255) },
        {RainbowColor.Green, new Color32(121, 195, 20, 255) },
        {RainbowColor.Blue, new Color32(72, 125, 231, 255) },
        {RainbowColor.Indigo, new Color32(75, 54, 157, 255) },
        {RainbowColor.Violet, new Color32(112, 54, 157, 255) }
    };

    public static Color32 GetColor(this RainbowColor color)
    {
        return ColorMap[color];
    }
}
