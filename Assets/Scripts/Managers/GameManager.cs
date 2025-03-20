using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public const int numCharEachPage = 6;

    [SerializeField] private GameObject recruitPanel;
    [SerializeField] private GameObject characterPanel;

    [SerializeField] private GameObject characterInventory;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        // Load character data's references into character pool
        CharacterLoader.LoadAllCharacterReferences();
    }


    // View player's character inventory
    public void ViewCharacters(int index)
    {
        if (characterPanel.activeInHierarchy) return;
        List<CharacterData> characterIds = CharacterInventory.Instance.GetCharacters(index, index + numCharEachPage);
        characterPanel.GetComponent<CharacterPanel>().Show(characterIds);
    }

    // Recruit characters
    public void ViewRecruits()
    {
        if (recruitPanel.activeInHierarchy) return;
        recruitPanel.GetComponent<RecruitPanel>().Show(6);
    }
}
