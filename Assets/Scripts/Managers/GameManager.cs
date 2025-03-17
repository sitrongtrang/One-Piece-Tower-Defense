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
    public GameObject characterPrefab;

    public Dictionary<Rarity, Color> RarityToColor;

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
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ViewCharacterInfo()
    {
        CharacterData data = Instantiate(characterPrefab).GetComponent<TowerCharacter>().characterData;
        CharacterCardUIManager.Instance.OnCharacterSelected(data);
    }
}
