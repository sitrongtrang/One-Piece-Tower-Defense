using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public const int numCharEachPage = 6;

    [SerializeField] private GameObject recruitPanel;
    [SerializeField] private GameObject characterPanel;

    [SerializeField] private GameObject characterInventory;

    private List<AssetReferenceT<CharacterData>> allCharacterReferences = new();
    private string characterDataLabel = "CharacterData";
    private Dictionary<CharacterData, AsyncOperationHandle<CharacterData>> loadedCharacters = new();


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        // Load character data's references into character pool
        LoadAllCharacterReferences();
    }

    private void LoadAllCharacterReferences()
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

    // View player's character inventory
    public void ViewCharacters(int index)
    {
        if (characterPanel.activeInHierarchy) return;
        List<CharacterData> characters = CharacterInventory.Instance.GetCharacters(index, index + numCharEachPage);
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
    public void ReleaseCharacter(CharacterData characterData)
    {
        if (loadedCharacters.TryGetValue(characterData, out var handle))
        {
            Addressables.Release(handle);
            loadedCharacters.Remove(characterData);
        }
    }

}
