using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.IO;

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

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public Dictionary<Rarity, Color> RarityToColor;
    public const int numCharEachPage = 6;

    public GameObject basicCharacterPrefab;
    public CharacterData basicCharacterData;

    public List<CharacterData> characterInventory;

    [SerializeField] private GameObject recruitPanel;
    [SerializeField] private GameObject characterPanel;

    private List<AssetReferenceT<CharacterData>> allCharacterReferences = new();
    private string characterLabel = "CharacterData";
    private Dictionary<CharacterData, AsyncOperationHandle<CharacterData>> loadedCharacters = new();


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        RarityToColor = new();
        RarityToColor.Add(Rarity.Common, Color.white);
        RarityToColor.Add(Rarity.Uncommon, Color.green);
        RarityToColor.Add(Rarity.Rare, Color.blue);
        RarityToColor.Add(Rarity.Epic, new Color(128, 0, 128));
        RarityToColor.Add(Rarity.Heroic, Color.yellow);
        RarityToColor.Add(Rarity.Mythic, Color.red);
        RarityToColor.Add(Rarity.Unique, Color.magenta);
        RarityToColor.Add(Rarity.Extreme, new Color(255, 131, 83));
        RarityToColor.Add(Rarity.Legendary, new Color(255, 94, 32));

        // Load character data into character pool
        LoadAllCharacterReferences();

        basicCharacterData = Instantiate(basicCharacterPrefab).GetComponent<TowerCharacter>().characterData;
        characterInventory.Add(basicCharacterData);
    }

    private void LoadAllCharacterReferences()
    {
        Addressables.LoadResourceLocationsAsync(characterLabel, typeof(CharacterData)).Completed += handle =>
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                foreach (UnityEngine.ResourceManagement.ResourceLocations.IResourceLocation location in handle.Result)
                {
                    AssetReferenceT<CharacterData> assetReference = new AssetReferenceT<CharacterData>(location.PrimaryKey);
                    allCharacterReferences.Add(assetReference);
                }

                Debug.Log($"Loaded {allCharacterReferences.Count} character references from Addressables.");
            }
            else
            {
                Debug.LogError("Failed to load character references.");
            }
        };
    }

    // View player's character inventory
    public void ViewCharacters(int index)
    {
        if (index >= characterInventory.Count || characterPanel.activeInHierarchy) return;
        List<CharacterData> characters = new();
        for (int  i = 0; i < numCharEachPage; i++)
        {
            if (index + i < characterInventory.Count) characters.Add(characterInventory[index + i]);
            else characters.Add(null);
        }
        characterPanel.GetComponent<CharacterPanel>().Show(characters);
    }

    // Recruit characters
    public void ViewRecruits()
    {
        if (recruitPanel.activeInHierarchy) return;
        recruitPanel.GetComponent<RecruitPanel>().Show(6);
    }

    public void Recruit(Action<CharacterData> onCharacterLoaded)
    {
        if (allCharacterReferences.Count == 0)
        {
            Debug.LogError("No characters available in the pool.");
            return;
        }

        int index = UnityEngine.Random.Range(0, allCharacterReferences.Count);
        AssetReferenceT<CharacterData> characterReference = allCharacterReferences[index];

        Addressables.LoadAssetAsync<CharacterData>(characterReference).Completed += handle =>
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                CharacterData data = handle.Result;
                loadedCharacters[data] = handle;
                onCharacterLoaded?.Invoke(data);
            }
            else Debug.LogError($"Failed to load character: {characterReference}");
        };
    }

    public void ReleaseCharacter(CharacterData characterData)
    {
        if (loadedCharacters.TryGetValue(characterData, out var handle))
        {
            Addressables.Release(handle);
            loadedCharacters.Remove(characterData);
        }
    }

}
