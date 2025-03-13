using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterCardUI : MonoBehaviour
{
    public Image characterPortrait;
    public TextMeshProUGUI characterName;
    public TextMeshProUGUI characterRarity;
    public TextMeshProUGUI statsText;
    //public Transform abilityContainer;
    //public GameObject abilityIconPrefab;
    public Button viewUpgradesButton;
    public Button closeButton;

    private void Start()
    {
        gameObject.SetActive(false);
        closeButton.onClick.AddListener(Close);
    }

    public void Setup(CharacterData data)
    {
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

        // viewUpgradesButton.onClick.AddListener(OpenUpgradeScreen);
    }

    //private void OpenUpgradeScreen()
    //{
    //    gameManager.ShowUpgradeOptions(characterData);
    //}

    public void ShowInfo(CharacterData characterData)
    {
        gameObject.SetActive(true);
        Setup(characterData);
    }

    public void Close()
    {
        gameObject.SetActive(false);
        GameManager.Instance.CloseCharacterInfo();
    }
}
