using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeOption : MonoBehaviour
{
    [SerializeField]
    private Button characterPortrait;
    [SerializeField]
    private TextMeshProUGUI characterName;
    private UpgradeRequirement requirement;
    [SerializeField]
    private GameObject upgradePanel;

    private float padding = 10f;

    void Start()
    {
        //closeButton.onClick.AddListener(Close);
        characterPortrait.onClick.AddListener(OpenUpgradePanel);
    }

    public void Setup(UpgradeRequirement requirement)
    {
        this.requirement = requirement;
        characterPortrait.GetComponent<Image>().sprite = requirement.upgradeTarget.characterPortrait;
        characterName.text = requirement.upgradeTarget.characterName;
    }

    public void ShowOption(UpgradeRequirement requirement)
    {
        Setup(requirement);
        gameObject.SetActive(true);
    }

    public void OpenUpgradePanel()
    {
        if (upgradePanel.activeInHierarchy) return;
        upgradePanel.GetComponent<UpgradePanel>().ShowRequirements(requirement);
    }

    public void Close()
    {
        gameObject.SetActive(false);
        upgradePanel.GetComponent<UpgradePanel>().Close();
    }
}
