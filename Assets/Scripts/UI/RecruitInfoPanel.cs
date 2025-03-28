using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecruitInfoPanel : Panel
{
    [SerializeField] private Image characterPortrait;
    [SerializeField] private TextMeshProUGUI characterName;
    [SerializeField] private TextMeshProUGUI characterRarity;
    [SerializeField] private TextMeshProUGUI statsText;

    [SerializeField] private GameObject recruitPanel;

    protected override void Setup(object data)
    {
        if (!(data is CharacterData character)) return;

        characterPortrait.sprite = character.characterPortrait;
        characterName.text = character.characterName;
        characterRarity.text = character.rarity.ToString();
        characterRarity.GetComponent<TMP_Text>().color = RarityMapper.RarityToColor[character.rarity];
        characterName.GetComponent<TMP_Text>().color = RarityMapper.RarityToColor[character.rarity];
        statsText.text = $"Attack: {character.attackPower}\n" +
                         $"Speed: {character.attackSpeed}\n" +
                         $"Range: {character.range}\n" +
                         $"Health: {character.health}";
    }
}
