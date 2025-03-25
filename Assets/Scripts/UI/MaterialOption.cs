using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MaterialOption : Panel
{
    [SerializeField] private Button characterPortrait;
    [SerializeField] private TextMeshProUGUI characterName;

    private CharacterData material;
    private GameObject materialPanel;

    protected override void Start()
    {
        characterPortrait.onClick.AddListener(ChooseMaterial);
    }

    protected override void Setup(object data)
    {
        if (!(data is CharacterData character)) return;

        this.material = character;
        characterPortrait.GetComponent<Image>().sprite = character.characterPortrait;
        characterName.text = character.characterName;
        characterName.GetComponent<TMP_Text>().color = RarityMapper.RarityToColor[character.rarity];
    }

    public void setMaterialPanel(GameObject materialPanel)
    {
        this.materialPanel = materialPanel;
    }

    private void ChooseMaterial()
    {
        materialPanel.GetComponent<MaterialList>().ChooseMaterial(material);
    }
}
