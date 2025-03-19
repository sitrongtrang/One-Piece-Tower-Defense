using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollViewPanel : Panel
{

    [SerializeField] protected GameObject optionPrefab;
    [SerializeField] protected Transform contentPanel;
    [SerializeField] protected ScrollRect scrollRect;
    [SerializeField] protected Slider slider;

    protected List<object> options = new();
    protected List<object> optionPool = new();

    protected override void Start()
    {
        base.Start();
        slider.onValueChanged.AddListener(UpdateScrollPosition);
    }

    protected override void Setup(object data)
    {
        AdjustSlider();
    }

    private void AdjustSlider()
    {
        float contentWidth = options.Count * optionPrefab.GetComponent<RectTransform>().sizeDelta.x;
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