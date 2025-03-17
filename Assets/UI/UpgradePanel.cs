using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradePanel : Panel
{
    [SerializeField] private List<Button> upgradeMaterials;

    private Canvas canvas;

    protected override void Start()
    {
        base.Start();
        canvas = FindObjectOfType<Canvas>();
    }

    protected override void Setup(object data)
    {
        if (!(data is UpgradeRequirement requirement)) return;

        Vector3 middleOfCanvas = canvas.transform.position;
        transform.position = middleOfCanvas;
        upgradeMaterials[0].image.sprite = requirement.upgradeTarget.characterPortrait;
        for (int i = 1; i < upgradeMaterials.Count; i++)
        {
            if (i - 1 < requirement.obligatoryRequirements.Count)
            {
                upgradeMaterials[i].image.sprite = requirement.obligatoryRequirements[i - 1].characterPortrait;
            } else
            {
                upgradeMaterials[i].image.color = GameManager.Instance.RarityToColor[EnumHelper.GetPreviousEnumValue(requirement.upgradeTarget.rarity)];
            }
        }
    }
}
