using System.Collections.Generic;
using UnityEngine;

public class CharacterInventory : MonoBehaviour
{
    public static CharacterInventory Instance;

    private List<CharacterData> characterInventory;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            LoadInventory();
        }
        else
            Destroy(gameObject);

        LoadInventory();
    }

    public int CountOwnedCharacters() => characterInventory.Count;

    public List<CharacterData> GetCharacters(int start = 0, int end = -1)
    {
        if (end == -1) end = characterInventory.Count;
        end = Mathf.Min(end, characterInventory.Count);
        List<CharacterData> characters = new();
        for (int i = start; i < end; i++) characters.Add(characterInventory[i]);
        return characters;
    }

    public bool HasCharacter(CharacterData character)
    {
        return characterInventory.Contains(character);
    }

    public void AddCharacter(CharacterData character)
    {
        characterInventory.Add(character);
    }

    public void RemoveCharacter(CharacterData character)
    {
        characterInventory.Remove(character);
    }

    private void LoadInventory()
    {
        characterInventory = new();
    }
}
