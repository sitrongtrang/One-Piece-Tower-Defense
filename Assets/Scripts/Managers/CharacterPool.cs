using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class CharacterPool : MonoBehaviour
{
    public static CharacterPool Instance { get; private set; }

    // Map each rarity to the list of references of characters with that rarity
    private Dictionary<Rarity, List<AssetReferenceT<CharacterData>>> characterRarities = new();
    private int totalChars;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    public void LoadPool()
    {
        for (int i = 0; i < 9; i++) characterRarities[(Rarity)i] = new List<AssetReferenceT<CharacterData>>();
        List<AssetReferenceT<CharacterData>> charRefs = CharacterLoader.GetAllChar();

        foreach (AssetReferenceT<CharacterData> charRef in charRefs)
        {
            CharacterData temp = null;
            CharacterLoader.LoadCharacter(charRef, (characterData) =>
            {
                temp = characterData;
                if (temp != null)
                {
                    characterRarities[temp.rarity].Add(charRef);
                    totalChars++;
                    CharacterLoader.ReleaseCharacter(temp);
                }
            });

            //if (temp != null) CharacterLoader.ReleaseCharacter(temp);
        }
    }

    public List<AssetReferenceT<CharacterData>> GetCharsByRarity(Rarity rarity) => characterRarities[rarity];
    public int GetNumCharsByRarity(Rarity rarity) => characterRarities[rarity].Count;
    public int GetTotalChars() => totalChars;

    public List<AssetReferenceT<CharacterData>> GetRandomCharacters(List<Rarity> rarities)
    {
        HashSet<AssetReferenceT<CharacterData>> uniqueChars = new();
        System.Random rand = new System.Random();

        int totalChar = GetTotalChars();

        for (int i = 0; i < Mathf.Min(rarities.Count, totalChar); i++)
        {
            while (uniqueChars.Count <= i) // Loop until a new character is added
            {
                int index = rand.Next(0, characterRarities[rarities[i]].Count);
                uniqueChars.Add(characterRarities[rarities[i]][index]);
            }
        }

        return new List<AssetReferenceT<CharacterData>>(uniqueChars);
    }

    public bool HasCharacter(CharacterData character)
    {
        return CharacterLoader.GetCharRef(character.name) != null;
    }

    public void AddCharacter(CharacterData character)
    {
        characterRarities[character.rarity].Add(CharacterLoader.GetCharRef(character.name));
        totalChars++;
    }

    public void RemoveCharacter(CharacterData character)
    {
        if (HasCharacter(character))
        {
            AssetReferenceT<CharacterData> reference = CharacterLoader.GetCharRef(character.name);
            characterRarities[character.rarity].Remove(reference);
            totalChars--;
        }
    }
}

