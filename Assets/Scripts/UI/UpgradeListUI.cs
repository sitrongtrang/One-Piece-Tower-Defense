using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeListUI : Panel
{
    [SerializeField] private GameObject characterCard;
    [SerializeField] private GameObject upgradeOptionPrefab;
    [SerializeField] private Transform contentPanel;
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private Slider slider;

    private RectTransform upgradeListRect;
    private List<GameObject> upgradeOptionPool = new List<GameObject>();
    private List<UpgradeRequirement> upgradeOptions = new List<UpgradeRequirement>();
    private float padding = 10f;

    protected override void Start()
    {
        base.Start();
        upgradeListRect = GetComponent<RectTransform>();
        slider.onValueChanged.AddListener(UpdateScrollPosition);
    }

    protected override void Setup(object data)
    {
        if (!(data is List<UpgradeRequirement> requirements)) return;

        upgradeOptions = new List<UpgradeRequirement>();
        foreach (GameObject option in upgradeOptionPool)
        {
            Destroy(option);
        }
        upgradeOptionPool.Clear();

        for (int i = 0; i < requirements.Count; i++)
        {
            upgradeOptions.Add(requirements[i]);
            GameObject upgradeOption = Instantiate(upgradeOptionPrefab, contentPanel);
            upgradeOption.GetComponent<UpgradeOption>().Show(requirements[i]);
            upgradeOptionPool.Add(upgradeOption);
        }

        AdjustSlider();
    }

    public override void Close()
    {
        base.Close();
        foreach (GameObject option in upgradeOptionPool)
        {
            option.GetComponent<UpgradeOption>().Close();
        }
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
