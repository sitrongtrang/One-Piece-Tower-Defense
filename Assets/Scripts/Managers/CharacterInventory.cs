using System.Collections.Generic;
using UnityEngine;

public class CharacterInventory : MonoBehaviour
{
    public static CharacterInventory Instance { get; private set; }

    // Data of player's owned characters
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
        if (character.isRecruitable == true)  CharacterPool.Instance.RemoveCharacter(character);
    }

    public void RemoveCharacter(CharacterData character)
    {
        if (HasCharacter(character))
        {
            characterInventory.Remove(character);
            if (character.isRecruitable == true) CharacterPool.Instance.AddCharacter(character);
        }
        else Debug.Log("No character to be removed");

    }

    private void LoadInventory()
    {
        // TODO: load inventory from save file
        characterInventory = new();
    }
}
