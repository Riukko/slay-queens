using System.Collections.Generic;
using UnityEngine;

public enum CellGroup
{
    WHITE,
    GREEN,
    BLUE,
    RED,
    MAGENTA,
    YELLOW,
    CYAN,
    PINK,
    PURPLE,
    ORANGE,
    BROWN

}

[System.Serializable]
public class GroupColorEntry
{
    public CellGroup group;
    public Color color;
}


[CreateAssetMenu(fileName = "GroupColorPalette", menuName = "Grid/Group Color Palette")]
public class CellGroupColorPalette : ScriptableObject
{
    public List<GroupColorEntry> groupColors;

    private Dictionary<CellGroup, Color> lookup;

    public Color GetColor(CellGroup group)
    {
        if (lookup == null || lookup.Count != groupColors.Count)
        {
            lookup = new Dictionary<CellGroup, Color>();
            foreach (var entry in groupColors)
            {
                if (!lookup.ContainsKey(entry.group))
                    lookup.Add(entry.group, entry.color);
            }
        }

        return lookup.TryGetValue(group, out var color) ? color : Color.white;
    }

    public Color GetRandomColor()
    {
        return groupColors[Random.Range(0, groupColors.Count - 1)].color;
    }
}

