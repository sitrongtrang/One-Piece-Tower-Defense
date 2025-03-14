using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradePanel : MonoBehaviour
{
    [SerializeField]
    private List<Button> upgradeMaterials;
    [SerializeField]
    private Button closeButton;
    private Canvas canvas;

    void Start()
    {
        gameObject.SetActive(false);
        canvas = FindObjectOfType<Canvas>();
        closeButton.onClick.AddListener(Close);
    }

    private void Setup(UpgradeRequirement requirement)
    {
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

    public void ShowRequirements(UpgradeRequirement requirement)
    {
        Setup(requirement);
        gameObject.SetActive(true);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
}
