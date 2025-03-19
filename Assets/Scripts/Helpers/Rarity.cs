using System;

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
