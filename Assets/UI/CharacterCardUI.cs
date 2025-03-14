using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterCardUI : MonoBehaviour
{
    private CharacterData characterData;

    [SerializeField]
    private Image characterPortrait;
    [SerializeField]
    private TextMeshProUGUI characterName;
    [SerializeField]
    private TextMeshProUGUI characterRarity;
    [SerializeField]
    private TextMeshProUGUI statsText;
    //public Transform abilityContainer;
    //public GameObject abilityIconPrefab;
    [SerializeField]
    private Button viewUpgradesButton;
    [SerializeField]
    private Button closeButton;
    [SerializeField]
    private GameObject upgradeList;

    private void Start()
    {
        gameObject.SetActive(false);
        closeButton.onClick.AddListener(Close);
    }

    private void Setup(CharacterData data)
    {
        characterData = data;

        characterPortrait.sprite = data.characterPortrait;
        characterName.text = data.characterName;
        characterRarity.text = data.rarity.ToString();

        statsText.text = $"Attack: {data.attackPower}\n" +
                         $"Speed: {data.attackSpeed}\n" +
                         $"Range: {data.range}\n" +
                         $"Health: {data.health}";

        //foreach (Transform child in abilityContainer)
        //    Destroy(child.gameObject);

        //foreach (Sprite ability in data.abilityIcons)
        //{
        //    GameObject icon = Instantiate(abilityIconPrefab, abilityContainer);
        //    icon.GetComponent<Image>().sprite = ability;
        //}

        viewUpgradesButton.onClick.AddListener(OpenUpgradeList);
    }

    public void ShowInfo(CharacterData characterData)
    {
        Setup(characterData);
        gameObject.SetActive(true);
    }

    public void Close()
    {
        gameObject.SetActive(false);
        upgradeList.GetComponent<UpgradeListUI>().Close();
    }

    private void OpenUpgradeList()
    {
        if (upgradeList.activeInHierarchy) return;
        if (characterData.upgradeOptions.Count > 0) upgradeList.GetComponent<UpgradeListUI>().ShowOptionList(characterData.upgradeOptions);
    }
}
