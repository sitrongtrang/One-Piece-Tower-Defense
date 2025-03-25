using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeOption : Panel
{
    [SerializeField] private Button characterPortrait;
    [SerializeField] private TextMeshProUGUI characterName;
    [SerializeField] private Sprite emptySlot;

    private UpgradeRequirement requirement;
    private GameObject upgradePanel;

    protected override void Start()
    {
        characterPortrait.onClick.AddListener(OpenUpgradePanel);
        upgradePanel = GameObject.Find("Canvas/CharacterPanel/CharacterCard/UpgradePanel");
    }

    protected override void Setup(object data)
    {
        if (!(data is UpgradeRequirement requirement)) return;

        this.requirement = requirement;

        CharacterData target = null;
        if (requirement.upgradeTarget != null)
        {
            CharacterLoader.LoadCharacter(requirement.upgradeTarget, (characterData) =>
            {
                target = characterData;
                characterPortrait.GetComponent<Image>().sprite = target?.characterPortrait ?? emptySlot;
                characterName.text = target?.characterName ?? "";
                if (target != null) 
                {
                    characterName.GetComponent<TMP_Text>().color = RarityMapper.RarityToColor[target.rarity];
                    CharacterLoader.ReleaseCharacter(target);
                }
            });
        }

        if (target != null) CharacterLoader.ReleaseCharacter(target);
    }

    public override void Close()
    {
        base.Close();
        upgradePanel.GetComponent<UpgradePanel>().Close();
    }

    public void OpenUpgradePanel()
    {
        if (upgradePanel.activeInHierarchy) return;
        upgradePanel.GetComponent<UpgradePanel>().Show(requirement);
    }
}
