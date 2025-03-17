using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MaterialOption : Panel
{
    [SerializeField] private Button characterPortrait;
    [SerializeField] private TextMeshProUGUI characterName;
    private CharacterData characterData;

    private float padding = 10f;

    protected override void Start()
    {
        characterPortrait.onClick.AddListener(ChooseMaterial);
    }

    protected override void Setup(object data)
    {
        if (!(data is CharacterData characterData)) return;
        this.characterData = characterData;
        characterPortrait.GetComponent<Image>().sprite = characterData.characterPortrait;
        characterName.text = characterData.characterName;
    }

    private void ChooseMaterial()
    {
        return;
    }
}
