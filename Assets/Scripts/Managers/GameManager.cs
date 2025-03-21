using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

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
    }

    private void Start()
    {
        CharacterLoader.LoadAllCharacterReferences();
    }


    // View player's character inventory
    public void ViewCharacters(int index)
    {
        if (characterPanel.activeInHierarchy) return;
        CharacterPanel panel = characterPanel.GetComponent<CharacterPanel>();
        List<CharacterData> characterIds = CharacterInventory.Instance.GetCharacters(index, index + panel.GetNumCharEachPage());
        panel.Show(characterIds);
    }

    // Recruit characters
    public void ViewRecruits()
    {
        if (recruitPanel.activeInHierarchy) return;
        recruitPanel.GetComponent<RecruitPanel>().Show(6);
    }
}
