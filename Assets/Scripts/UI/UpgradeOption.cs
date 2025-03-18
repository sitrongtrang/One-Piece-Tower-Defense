using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeOption : Panel
{
    [SerializeField] private Button characterPortrait;
    [SerializeField] private TextMeshProUGUI characterName;

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
        characterPortrait.GetComponent<Image>().sprite = requirement.upgradeTarget.characterPortrait;
        characterName.text = requirement.upgradeTarget.characterName;
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
