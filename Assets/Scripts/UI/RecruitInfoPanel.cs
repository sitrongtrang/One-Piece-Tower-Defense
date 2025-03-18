using System.Collections;
using System.Collections.Generic;
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

    private DataSlotInfo recruitInfo;

    protected override void Setup(object data)
    {
        if (!(data is DataSlotInfo character)) return;

        recruitInfo = character;
        characterPortrait.sprite = recruitInfo.data.characterPortrait;
        characterName.text = recruitInfo.data.characterName;
        characterRarity.text = recruitInfo.data.rarity.ToString();

        statsText.text = $"Attack: {recruitInfo.data.attackPower}\n" +
                         $"Speed: {recruitInfo.data.attackSpeed}\n" +
                         $"Range: {recruitInfo.data.range}\n" +
                         $"Health: {recruitInfo.data.health}";
    }

    public void Recruit()
    {
        if (GameManager.Instance.ownedCharacters.Contains(recruitInfo.data))
        {
            Debug.Log("Already own character");
        } else
        {
            GameManager.Instance.ownedCharacters.Add(recruitInfo.data);
            recruitInfo.data = null;
            recruitPanel.GetComponent<RecruitPanel>().RemoveSlot(recruitInfo.slotNum);
        }
    }
}
