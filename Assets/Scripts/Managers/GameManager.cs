using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Rarity
{
    Common,
    Uncommon,
    Rare,
    Epic,
    Legendary,
    Unique,
    Mythic,
    Awakened
}

public class EnumHelper
{
    public static Rarity GetPreviousEnumValue(Rarity current)
    {
        Rarity[] values = (Rarity[])Rarity.GetValues(typeof(Rarity));
        int currentIndex = Array.IndexOf(values, current);

        // If current is the first value, return the last one (looping behavior)
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
    public List<CharacterData> characters;

    private string prefabFolderPath = "Prefabs/Towers/Strawhats";

    public List<CharacterData> ownedCharacters;

    [SerializeField] private GameObject recruitPanel;

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
        RarityToColor.Add(Rarity.Legendary, Color.yellow);
        RarityToColor.Add(Rarity.Unique, Color.magenta);
        RarityToColor.Add(Rarity.Mythic, Color.red);
        RarityToColor.Add(Rarity.Awakened, new Color(255, 165, 0));

        GameObject[] prefabs = Resources.LoadAll<GameObject>(prefabFolderPath);
        foreach (GameObject prefab in prefabs)
        {
            CharacterData data = Instantiate(prefab).GetComponent<TowerCharacter>().characterData;
            characters.Add(data);
        }

        basicCharacterData = Instantiate(basicCharacterPrefab).GetComponent<TowerCharacter>().characterData;
    }

    public void ViewCharacterInfo()
    {
        CharacterData data;
        if (ownedCharacters.Count > 0)
        {
            int index = UnityEngine.Random.Range(0, ownedCharacters.Count);
            data = ownedCharacters[index];
        } else
        {
            data = basicCharacterData;
        }

        CharacterCardUIManager.Instance.OnCharacterSelected(data);
    }

    public void ViewRecruits()
    {
        if (recruitPanel.activeInHierarchy) return;
        recruitPanel.GetComponent<RecruitPanel>().Show(6);
    }
}
