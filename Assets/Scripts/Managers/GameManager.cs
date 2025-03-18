using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.IO;

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
    public GameObject basicCharacterPrefab;
    public CharacterData basicCharacterData;

    public Dictionary<Rarity, Color> RarityToColor;

    public List<CharacterData> ownedCharacters;
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

        RarityToColor = new Dictionary<Rarity, Color>();
        RarityToColor.Add(Rarity.Common, Color.white);
        RarityToColor.Add(Rarity.Uncommon, Color.green);
        RarityToColor.Add(Rarity.Rare, Color.blue);
        RarityToColor.Add(Rarity.Epic, new Color(128, 0, 128));
        RarityToColor.Add(Rarity.Heroic, Color.yellow);
        RarityToColor.Add(Rarity.Mythic, Color.red);
        RarityToColor.Add(Rarity.Unique, Color.magenta);
        RarityToColor.Add(Rarity.Extreme, new Color(255, 131, 83));
        RarityToColor.Add(Rarity.Legendary, new Color(255, 94, 32));

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
        ownedCharacters.Add(basicCharacterData);
    }

    private void LoadCharacterKeysFromFile()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "CharacterKeys.txt");

        if (File.Exists(path))
        {
            allCharacterKeys = new List<string>(File.ReadAllLines(path));
            Debug.Log($"Loaded {allCharacterKeys.Count} character keys from file.");
        }
        else
        {
            Debug.LogError("Character keys file not found!");
        }
    }

    public void ViewCharacters(int index)
    {
        if (index >= ownedCharacters.Count || characterPanel.activeInHierarchy) return;
        List<CharacterData> characters = new List<CharacterData>();
        for (int  i = 0; i < 6; i++)
        {
            if (index + i < ownedCharacters.Count)
            {
                characters.Add(ownedCharacters[index + i]);
            } else
            {
                characters.Add(null);
            }
        }
        characterPanel.GetComponent<CharacterPanel>().Show(characters);
    }

    public void ViewRecruits()
    {
        if (recruitPanel.activeInHierarchy) return;
        recruitPanel.GetComponent<RecruitPanel>().Show(6);
    }
}
