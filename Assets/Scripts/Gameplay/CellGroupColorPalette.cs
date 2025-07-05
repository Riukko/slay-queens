using System;
using System.Collections.Generic;
using UnityEngine;

public enum CellColorGroup
{
    GREEN,
    BLUE,
    RED,
    MAGENTA,
    YELLOW,
    CYAN,
    PINK,
    PURPLE,
    ORANGE,
    BROWN,
    WHITE,
}

public static class CellGroupColorPalette
{
    public static int MaxColorCount => Enum.GetValues(typeof(CellColorGroup)).Length;

    private static readonly Dictionary<CellColorGroup, Color> groupColors = new()
    {
        { CellColorGroup.GREEN,   new Color32(168, 255, 158, 255) },
        { CellColorGroup.BLUE,    new Color32(133, 206, 255, 255) },
        { CellColorGroup.RED,     new Color32(252, 121, 128, 255) },
        { CellColorGroup.MAGENTA, new Color32(255, 117, 248, 255) },
        { CellColorGroup.YELLOW,  new Color32(255, 239, 117, 255) },
        { CellColorGroup.CYAN,    new Color32(130, 255, 253, 255) },
        { CellColorGroup.PINK,    new Color32(250, 152, 216, 255) },
        { CellColorGroup.PURPLE,  new Color32(188, 134, 235, 255) },
        { CellColorGroup.ORANGE,  new Color32(255, 187, 110, 255) },
        { CellColorGroup.BROWN,   new Color32(156, 113, 81, 255) },
        { CellColorGroup.WHITE,   Color.white }
    };

    public static Color GetColor(CellColorGroup group)
    {
        return groupColors.TryGetValue(group, out var color) ? color : Color.white;
    }

    public static Color GetRandomColor()
    {
        var values = System.Enum.GetValues(typeof(CellColorGroup));
        var randomGroup = (CellColorGroup)values.GetValue(UnityEngine.Random.Range(0, values.Length));
        return GetColor(randomGroup);
    }

    public static CellColorGroup GetRandomColorGroup()
    {
        var values = Enum.GetValues(typeof(CellColorGroup));
        return (CellColorGroup)values.GetValue(UnityEngine.Random.Range(0, values.Length));
    }

    public static CellColorGroup GetColorGroupAtIndex(int index)
    {
        try
        {
            CellColorGroup colorGroup = (CellColorGroup)Enum.GetValues(typeof(CellColorGroup)).GetValue(index);
            return colorGroup;
        }
        catch (Exception e)
        {
            Debug.Log($"GetColorGroupAtIndex throwed an exception : {e.Message}");
            return CellColorGroup.WHITE;
        }

    }

    public static int GetColorIndexFromGroup(this CellColorGroup colorGroup) => (int)colorGroup;
}

