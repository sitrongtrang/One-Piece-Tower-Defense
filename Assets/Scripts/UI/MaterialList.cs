using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MaterialList : Panel
{
    [SerializeField] private GameObject materialPrefab;  
    [SerializeField] private Transform contentPanel;    
    [SerializeField] private ScrollRect scrollRect;     
    [SerializeField] private Slider slider;             

    private List<CharacterData> characters;
    private List<GameObject> materialOptionPool = new List<GameObject>();

    protected override void Start()
    {
        base.Start();
        characters = new List<CharacterData>();
        slider.onValueChanged.AddListener(UpdateScrollPosition);  
    }

    protected override void Setup(object data)
    {
        if (!(data is Rarity rarity)) return;

        characters = new List<CharacterData>();

        foreach (GameObject option in materialOptionPool)
        {
            Destroy(option);
        }
        materialOptionPool.Clear();

        foreach (CharacterData characterData in GameManager.Instance.ownedCharacters)
        {
            if (characterData.rarity == rarity)
            {
                characters.Add(characterData);
                GameObject newOption = Instantiate(materialPrefab, contentPanel);
                newOption.GetComponent<MaterialOption>().Show(characterData);
                materialOptionPool.Add(newOption);
            }
        }

        AdjustSlider();
    }

    public override void Show(object data)
    {
        base.Show(data);
    }

    public override void Close()
    {
        base.Close();
        foreach (GameObject option in materialOptionPool)
        {
            option.GetComponent<MaterialOption>().Close();
        }
    }

    private void AdjustSlider()
    {
        float contentWidth = characters.Count * materialPrefab.GetComponent<RectTransform>().sizeDelta.x;
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
