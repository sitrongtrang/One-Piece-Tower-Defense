using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class CharacterPool : MonoBehaviour
{
    public static CharacterPool Instance { get; private set; }

    private Dictionary<Rarity, List<AssetReferenceT<CharacterData>>> characterRarities = new();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        LoadPool();
    }

    public void AddCharacter(AssetReferenceT<CharacterData> charRef, Rarity rarity) => characterRarities[rarity].Add(charRef);

    public void LoadPool()
    {
        List<AssetReferenceT<CharacterData>> charRefs = CharacterLoader.GetAllChar();

        foreach (AssetReferenceT<CharacterData> charRef in charRefs)
        {
            CharacterData temp = null;
            CharacterLoader.LoadCharacter(charRef, (characterData) =>
            {
                temp = characterData;
                if (temp != null) characterRarities[temp.rarity].Add(charRef);
            });
        }
    }

    public List<AssetReferenceT<CharacterData>> GetCharacters(Rarity rarity) => characterRarities[rarity];
}

