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
    public List<CharacterData> poolCharacters;

    [SerializeField] private GameObject recruitPanel;
    [SerializeField] private GameObject characterPanel;

    private List<string> allCharacterKeys = new();

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
        LoadCharacterKeysFromFile();
        poolCharacters = new List<CharacterData>();
        foreach (string key in allCharacterKeys)
        {
            Addressables.LoadAssetAsync<CharacterData>(key).Completed += handle =>
            {
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    CharacterData data = handle.Result;
                    poolCharacters.Add(data);
                }
            };
        }

        basicCharacterData = Instantiate(basicCharacterPrefab).GetComponent<TowerCharacter>().characterData;
        characterInventory.Add(basicCharacterData);
    }

    private void LoadCharacterKeysFromFile()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "CharacterKeys.txt");

        if (File.Exists(path))
        {
            allCharacterKeys = new List<string>(File.ReadAllLines(path));
            Debug.Log($"Loaded {allCharacterKeys.Count} character keys from file.");
        }
        else Debug.LogError("Character keys file not found!");
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
}
