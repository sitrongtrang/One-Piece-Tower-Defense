using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public static class CharacterLoader 
{
    // Map each character data to the task that loaded it
    private static Dictionary<CharacterData, AsyncOperationHandle<CharacterData>> loadedCharacters = new();

    // Map each character id to its asset reference
    private static Dictionary<string, AssetReferenceT<CharacterData>> characterIds = new();

    private static List<AssetReferenceT<CharacterData>> allCharacterReferences = new();
    private static List<string> recruitableCharLabel = new List<string> { "CharacterData", "Recruitables" };

    public static void LoadAllCharacterReferences()
    {
        // Load asset references of characters that are recruitable
        Addressables.LoadResourceLocationsAsync(recruitableCharLabel, Addressables.MergeMode.Intersection, typeof(CharacterData)).Completed += (handle) =>
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                foreach (UnityEngine.ResourceManagement.ResourceLocations.IResourceLocation location in handle.Result)
                {
                    AssetReferenceT<CharacterData> assetReference = new(location.PrimaryKey);
                    allCharacterReferences.Add(assetReference);
                }

                Debug.Log($"Loaded {allCharacterReferences.Count} character references from Addressables.");
                CharacterPool.Instance.LoadPool(); // Load the pool after loading all references
            }
            else Debug.LogError("Failed to load character references.");
        };
    }

    // Load the character data given then asset reference
    public static void LoadCharacter(AssetReferenceT<CharacterData> characterReference, Action<CharacterData> onCharacterLoaded = null)
    {
        AsyncOperationHandle<CharacterData> handle = Addressables.LoadAssetAsync<CharacterData>(characterReference);
        handle.Completed += (task) =>
        {
            if (task.Status == AsyncOperationStatus.Succeeded)
            {
                CharacterData data = task.Result;
                loadedCharacters[data] = task; // Store the task responsible for loading the data
                onCharacterLoaded?.Invoke(data);
                if (data != null) characterIds[data.name] = characterReference;
            }
            else Debug.LogError($"Failed to load character: {characterReference}");
        };
    }

    // Release character data from memory when not used
    public static void ReleaseCharacter(CharacterData characterData)
    {
        if (loadedCharacters.TryGetValue(characterData, out var handle))
        {
            Addressables.Release(handle);
            loadedCharacters.Remove(characterData);
        }
    }

    // Get asset reference by index
    public static AssetReferenceT<CharacterData> GetCharRef(int index) 
    { 
        if (index > allCharacterReferences.Count)
        {
            Debug.Log("Invalid index");
            return null;
        }
        return allCharacterReferences[index]; 
    }

    // Get asset reference by character id
    public static AssetReferenceT<CharacterData> GetCharRef(string id) => characterIds.TryGetValue(id, out AssetReferenceT<CharacterData> value) ? value : null;

    public static int GetNumChar() => allCharacterReferences.Count;
    public static List<AssetReferenceT<CharacterData>> GetAllChar() => allCharacterReferences;
    
}
