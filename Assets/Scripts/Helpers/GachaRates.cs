﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public enum PullMode
{
    // Maximum number of recruits for each mode
    Low = 2, 
    Medium = 3,
    High = 6
}

public static class GachaSystem
{
    private static int numRarity = 9;
    private static int numRecruitableRarity = 5;

    private static Dictionary<PullMode, Dictionary<Rarity, float>> rarityRates = new Dictionary<PullMode, Dictionary<Rarity, float>>
    {
        // Static rates for each rarity in each recruit mode
        { PullMode.Low, new Dictionary<Rarity, float>
            {
                { Rarity.Common, 0.6f },       // 60% chance
                { Rarity.Uncommon, 0.3f },     // 30% chance
                { Rarity.Rare, 0.08f },        // 8% chance
                { Rarity.Epic, 0.015f },       // 1.5% chance
                { Rarity.Heroic, 0.005f }      // 0.5% chance
            }
        },
        { PullMode.Medium, new Dictionary<Rarity, float>
            {
                { Rarity.Common, 0.2f },    
                { Rarity.Uncommon, 0.45f },  
                { Rarity.Rare, 0.25f },       
                { Rarity.Epic, 0.07f },       
                { Rarity.Heroic, 0.03f }   
            }
        },
        { PullMode.High, new Dictionary<Rarity, float>
            {
                { Rarity.Common, 0.03f },
                { Rarity.Uncommon, 0.07f },
                { Rarity.Rare, 0.5f },
                { Rarity.Epic, 0.3f },
                { Rarity.Heroic, 0.1f }
            }
        }
    };

    public static int GetNumRarity() => numRarity;
    public static int GetNumRecruitableRarity() => numRecruitableRarity;

    public static float GetStaticRate(PullMode mode, Rarity rarity) => rarityRates[mode][rarity];

    public static Dictionary<Rarity, float> GetStaticRates(PullMode mode) => rarityRates[mode];

    // Dynamic rarity rate = (Static rarity rate * number of characters of that rarity) / total number of characters
    private static float GetDynamicRate(PullMode mode, Rarity rarity, int count, int totalChar) => (rarityRates[mode][rarity] * count) / totalChar;

    public static Dictionary<Rarity, float> GetDynamicRates(PullMode mode, Dictionary<Rarity, int> counts, int totalChar)
    {
        Dictionary<Rarity, float> rates = new();
        float sum = 0f;

        // Get the dynamic rates for each rarity
        for (int i = 0; i < numRecruitableRarity; i++)
        {
            rates[(Rarity)i] = GetDynamicRate(mode, (Rarity)i, counts[(Rarity)i], totalChar);
            sum += rates[(Rarity)i];
        }

        // Normalize the result
        for (int i = 0; i < numRecruitableRarity; i++) rates[(Rarity)i] /= sum;

        return rates;
    }

    public static Rarity RollRarity(Dictionary<Rarity, float> rates)
    {
        float roll = Random.Range(0f, 1f);
        float cumulative = 0f;

        foreach (var entry in rates)
        {
            cumulative += entry.Value;
            if (roll < cumulative) return entry.Key;
        }

        return Rarity.Common; // Fallback (should never reach here)
    }

    public static List<AssetReferenceT<CharacterData>> RollCharacter(PullMode mode)
    {
        int num = Mathf.Min((int)mode, CharacterPool.Instance.GetTotalChars()); // Number of characters to be recruited
        if (num < 1)
        {
            Debug.Log("No character left in pool");
            return new();
        }

        List<Rarity> selectedRarities = new(); // Character rarity for each slot

        int totalChar = CharacterPool.Instance.GetTotalChars();

        Dictionary<Rarity, int> counts = new();
        for (int i = 0; i < numRecruitableRarity; i++) counts[(Rarity)i] = CharacterPool.Instance.GetNumCharsByRarity((Rarity)i);

        // Calculate dynamic rarity rates
        Dictionary<Rarity, float> rates = GetDynamicRates(mode, counts, totalChar);

        for (int i = 0; i < num; i++)
        {
            Rarity rarity = RollRarity(rates);
            selectedRarities.Add(rarity);
            counts[rarity]--;
            totalChar--;
            rates = GetDynamicRates(mode, counts, totalChar); // Re-calculate the dynamic rates
        }

        List<AssetReferenceT<CharacterData>> selectedRefs = new();
        selectedRefs.AddRange(CharacterPool.Instance.GetRandomCharacters(selectedRarities));

        return selectedRefs;
    }
}
