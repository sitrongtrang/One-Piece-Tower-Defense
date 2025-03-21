using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PullMode
{
    Low = 2,
    Medium = 3,
    High = 6
}

public static class GachaSystem
{
    private static Dictionary<PullMode, Dictionary<Rarity, float>> rarityRates = new Dictionary<PullMode, Dictionary<Rarity, float>>
    {
        { PullMode.Low, new Dictionary<Rarity, float>
            {
                { Rarity.Common, 60f },    // 60% chance
                { Rarity.Uncommon, 30f },  // 30% chance
                { Rarity.Rare, 9f },       // 9% chance
                { Rarity.Epic, 1f },       // 1% chance
                { Rarity.Heroic, 0f }      // 0% chance
            }
        },
        { PullMode.Medium, new Dictionary<Rarity, float>
            {
                { Rarity.Common, 20f },    
                { Rarity.Uncommon, 50f },  
                { Rarity.Rare, 25f },       
                { Rarity.Epic, 4f },       
                { Rarity.Heroic, 1f }   
            }
        },
        { PullMode.High, new Dictionary<Rarity, float>
            {
                { Rarity.Common, 3f },
                { Rarity.Uncommon, 7f },
                { Rarity.Rare, 50f },
                { Rarity.Epic, 30f },
                { Rarity.Heroic, 10f }
            }
        }
    };

    public static float GetRate(PullMode mode, Rarity rarity) => rarityRates[mode][rarity];

    public static Rarity RollRarity(PullMode mode)
    {
        float roll = Random.Range(0f, 100f);
        float cumulative = 0f;

        foreach (var entry in rarityRates[mode])
        {
            cumulative += entry.Value;
            if (roll < cumulative) return entry.Key;
        }

        return Rarity.Common; // Fallback (should never reach here)
    }
}
