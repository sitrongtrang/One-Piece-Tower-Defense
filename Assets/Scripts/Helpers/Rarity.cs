using System;
using System.Collections.Generic;
using UnityEngine;

// Rarity system
public enum Rarity
{
    Common,
    Uncommon,
    Rare,
    Epic,
    Heroic,
    Mythic,
    Unique,
    Extreme,
    Legendary
}

public class EnumHelper
{
    public static Rarity GetPreviousEnumValue(Rarity current)
    {
        Rarity[] values = (Rarity[])Rarity.GetValues(typeof(Rarity));
        int currentIndex = Array.IndexOf(values, current);
        if (currentIndex <= 0)
            return values[values.Length - 1];

        return values[currentIndex - 1];
    }
}

public static class RarityMapper
{
    public static readonly Dictionary<Rarity, Color> RarityToColor = new Dictionary<Rarity, Color> 
    {
        { Rarity.Common, Color.white },
        { Rarity.Uncommon, Color.green },
        { Rarity.Rare, Color.blue },
        { Rarity.Epic, new Color(191f / 255f, 64f / 255f, 191f / 255f) },
        { Rarity.Heroic, Color.yellow },
        { Rarity.Mythic, Color.red },
        { Rarity.Unique, Color.magenta },
        { Rarity.Extreme, new Color(255f / 255f, 131f / 255f, 83f / 255f) },
        { Rarity.Legendary, new Color(255f / 255f, 94f / 255f, 32f / 255f) },
    };
}


