using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeListUI : Panel
{
    [SerializeField] private GameObject characterCard;
    [SerializeField] private GameObject upgradeOptionPrefab;
    private RectTransform upgradeListRect;

    private List<GameObject> upgradeOptionPool = new List<GameObject>();
    private float padding = 10f;
    private float optionWidth;

    protected override void Start()
    {
        base.Start();
        upgradeListRect = GetComponent<RectTransform>();
        optionWidth = upgradeOptionPrefab.GetComponent<RectTransform>().sizeDelta.x;
    }

    protected override void Setup(object data)
    {
        if (!(data is List<UpgradeRequirement> requirements)) return;
        float totalWidth = requirements.Count * (optionWidth + padding) + padding;

        float currentPanelWidth = upgradeListRect.sizeDelta.x;
        if (totalWidth > currentPanelWidth)
        {
            upgradeListRect.sizeDelta = new Vector2(totalWidth, upgradeListRect.sizeDelta.y);
        }

        float currentX = padding + optionWidth / 2;

        for (int i = upgradeOptionPool.Count; i < requirements.Count; i++)
        {
            GameObject newOption = Instantiate(upgradeOptionPrefab, transform);
            newOption.SetActive(false);
            upgradeOptionPool.Add(newOption);
        }

        for (int i = 0; i < requirements.Count; i++)
        {
            GameObject upgradeOption = upgradeOptionPool[i];

            RectTransform optionRect = upgradeOption.GetComponent<RectTransform>();

            optionRect.anchoredPosition = new Vector2(currentX, 0);
            currentX += optionWidth + padding;

            upgradeOption.GetComponent<UpgradeOption>().Show(requirements[i]);
        }
    }

    public override void Close()
    {
        base.Close();
        foreach (GameObject option in upgradeOptionPool)
        {
            option.GetComponent<UpgradeOption>().Close();
        }
    }
}
