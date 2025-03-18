using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MaterialList : Panel
{
    [SerializeField] private GameObject materialPrefab;  
    [SerializeField] private Transform contentPanel;    
    [SerializeField] private ScrollRect scrollRect;     
    [SerializeField] private Slider slider;

    [SerializeField] private GameObject upgradePanel;

    private List<CharacterData> characters = new();
    private List<GameObject> materialOptionPool = new();

    protected override void Start()
    {
        base.Start();
        slider.onValueChanged.AddListener(UpdateScrollPosition);  
    }

    protected override void Setup(object data)
    {
        if (!(data is Rarity rarity)) return;

        characters = new List<CharacterData>();

        foreach (CharacterData characterData in GameManager.Instance.characterInventory)
        {
            if (characterData.rarity == rarity)
            {
                if (!upgradePanel.GetComponent<UpgradePanel>().IsChosenMaterial(characterData)) characters.Add(characterData);  
            }
        }

        // Instantiate more game objects if pool is not enough, otherwise reuse old objects
        for (int i = materialOptionPool.Count; i < characters.Count; i++)
        {
            GameObject newOption = Instantiate(materialPrefab, contentPanel);
            newOption.GetComponent<MaterialOption>().setMaterialPanel(gameObject);
            newOption.SetActive(false);
            materialOptionPool.Add(newOption);
        }

        for (int i = 0; i < characters.Count; i++) materialOptionPool[i].GetComponent<MaterialOption>().Show(characters[i]);

        AdjustSlider();
    }

    public override void Close()
    {
        base.Close();
        foreach (GameObject option in materialOptionPool) option.GetComponent<MaterialOption>().Close();
    }

    public void ChooseMaterial(CharacterData material)
    {
        upgradePanel.GetComponent<UpgradePanel>().ChooseMaterial(material);
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
