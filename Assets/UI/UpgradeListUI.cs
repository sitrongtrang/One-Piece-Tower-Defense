using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeListUI : MonoBehaviour
{
    [SerializeField]
    private Button closeButton;
    [SerializeField]
    private GameObject characterCard;
    [SerializeField]
    private GameObject upgradeOptionPrefab;
    private RectTransform upgradeListRect;

    private List<GameObject> upgradeOptionPool = new List<GameObject>();
    private float padding = 10f;
    private float optionWidth;

    void Start()
    {
        gameObject.SetActive(false);
        closeButton.onClick.AddListener(Close);
        upgradeListRect = GetComponent<RectTransform>();
        optionWidth = upgradeOptionPrefab.GetComponent<RectTransform>().sizeDelta.x;
    }

    private void Setup(List<UpgradeRequirement> requirements)
    {
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

            upgradeOption.GetComponent<UpgradeOption>().ShowOption(requirements[i]);
        }
    }

    public void ShowOptionList(List<UpgradeRequirement> requirements)
    {
        Setup(requirements);
        gameObject.SetActive(true);
    }

    public void Close()
    {
        gameObject.SetActive(false);
        foreach (GameObject option in upgradeOptionPool)
        {
            option.GetComponent<UpgradeOption>().Close();
        }
    }
}
