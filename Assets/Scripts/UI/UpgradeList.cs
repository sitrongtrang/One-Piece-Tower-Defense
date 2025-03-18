using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeList : Panel
{
    [SerializeField] private GameObject characterCard;
    [SerializeField] private GameObject upgradeOptionPrefab;
    [SerializeField] private Transform contentPanel;
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private Slider slider;

    private List<UpgradeRequirement> upgradeOptions = new();
    private List<GameObject> upgradeOptionPool = new();

    protected override void Start()
    {
        base.Start();
        slider.onValueChanged.AddListener(UpdateScrollPosition);
    }

    protected override void Setup(object data)
    {
        if (!(data is List<UpgradeRequirement> requirements)) return;

        upgradeOptions = new();

        for (int i = 0; i < requirements.Count; i++) upgradeOptions.Add(requirements[i]);

        // Instantiate more game objects if pool is not enough, otherwise reuse old objects
        for (int i = upgradeOptionPool.Count; i < upgradeOptions.Count; i++)
        {
            GameObject newOption = Instantiate(upgradeOptionPrefab, contentPanel);
            newOption.SetActive(false);
            upgradeOptionPool.Add(newOption);
        }

        for (int i = 0; i < upgradeOptions.Count; i++) upgradeOptionPool[i].GetComponent<UpgradeOption>().Show(upgradeOptions[i]);

        AdjustSlider();
    }

    public override void Close()
    {
        base.Close();
        foreach (GameObject option in upgradeOptionPool) option.GetComponent<UpgradeOption>().Close();
    }

    private void AdjustSlider()
    {
        float contentWidth = upgradeOptions.Count * upgradeOptionPrefab.GetComponent<RectTransform>().sizeDelta.x;
        float viewportWidth = scrollRect.viewport.rect.width;

        slider.gameObject.SetActive(contentWidth > viewportWidth);

        if (contentWidth > viewportWidth)
        {
            slider.minValue = 0;
            slider.maxValue = 1;
            slider.value = 0;
        }
    }

    private void UpdateScrollPosition(float value)
    {
        scrollRect.horizontalNormalizedPosition = value;
    }
}
