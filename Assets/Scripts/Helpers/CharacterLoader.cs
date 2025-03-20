using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public static class CharacterLoader 
{
    private static Dictionary<CharacterData, AsyncOperationHandle<CharacterData>> loadedCharacters = new();

    private static List<AssetReferenceT<CharacterData>> allCharacterReferences = new();
    private static string characterDataLabel = "CharacterData";

    public static void LoadAllCharacterReferences()
    {
        Addressables.LoadResourceLocationsAsync(characterDataLabel, typeof(CharacterData)).Completed += (handle) =>
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                foreach (UnityEngine.ResourceManagement.ResourceLocations.IResourceLocation location in handle.Result)
                {
                    AssetReferenceT<CharacterData> assetReference = new(location.PrimaryKey);
                    allCharacterReferences.Add(assetReference);
                }

                Debug.Log($"Loaded {allCharacterReferences.Count} character references from Addressables.");
            }
            else Debug.LogError("Failed to load character references.");
        };
    }

    public static void LoadCharacter(AssetReferenceT<CharacterData> characterReference, Action<CharacterData> onCharacterLoaded = null)
    {
        AsyncOperationHandle<CharacterData> handle = Addressables.LoadAssetAsync<CharacterData>(characterReference);
        handle.Completed += (task) =>
        {
            if (task.Status == AsyncOperationStatus.Succeeded)
            {
                CharacterData data = task.Result;
                loadedCharacters[data] = task;
                onCharacterLoaded?.Invoke(data);
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

    public static int GetNumChar() => allCharacterReferences.Count;

    public static AssetReferenceT<CharacterData> GetCharRef(int index) 
    { 
        if (index > allCharacterReferences.Count)
        {
            Debug.Log("Invalid index");
            return null;
        }
        return allCharacterReferences[index]; 
    }
}
