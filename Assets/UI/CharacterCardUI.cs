using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterCardUI : Panel
{
    private CharacterData characterData;

    [SerializeField] private Image characterPortrait;
    [SerializeField] private TextMeshProUGUI characterName;
    [SerializeField] private TextMeshProUGUI characterRarity;
    [SerializeField] private TextMeshProUGUI statsText;
    //public Transform abilityContainer;
    //public GameObject abilityIconPrefab;
    [SerializeField] private Button viewUpgradesButton;
    [SerializeField] private GameObject upgradeList;

    protected override void Start()
    {
        base.Start();
        if (viewUpgradesButton != null)
        {
            viewUpgradesButton.onClick.AddListener(OpenUpgradeList);
        }
    }

    protected override void Setup(object data)
    {
        if (!(data is CharacterData character)) return;

        characterData = character;
        characterPortrait.sprite = character.characterPortrait;
        characterName.text = character.characterName;
        characterRarity.text = character.rarity.ToString();

        statsText.text = $"Attack: {character.attackPower}\n" +
                         $"Speed: {character.attackSpeed}\n" +
                         $"Range: {character.range}\n" +
                         $"Health: {character.health}";

        //foreach (Transform child in abilityContainer)
        //    Destroy(child.gameObject);

        //foreach (Sprite ability in data.abilityIcons)
        //{
        //    GameObject icon = Instantiate(abilityIconPrefab, abilityContainer);
        //    icon.GetComponent<Image>().sprite = ability;
        //}
    }

    public override void Close()
    {
        base.Close();
        upgradeList.GetComponent<UpgradeListUI>().Close();
    }

    private void OpenUpgradeList()
    {
        if (upgradeList.activeInHierarchy) return;
        if (characterData.upgradeOptions.Count > 0) upgradeList.GetComponent<UpgradeListUI>().Show(characterData.upgradeOptions);
    }
}
