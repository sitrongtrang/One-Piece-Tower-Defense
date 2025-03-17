using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradePanel : Panel
{
    [SerializeField] private List<Button> upgradeMaterials;
    [SerializeField] private Image target;
    [SerializeField] private Button upgradeButton;
    [SerializeField] private GameObject materialPanel;

    private UpgradeRequirement requirement;
    private Canvas canvas;

    protected override void Start()
    {
        base.Start();
        canvas = FindObjectOfType<Canvas>();
        upgradeButton.onClick.AddListener(Upgrade);
    }

    protected override void Setup(object data)
    {
        if (!(data is UpgradeRequirement requirement)) return;

        this.requirement = requirement;
        Vector3 middleOfCanvas = canvas.transform.position;
        transform.position = middleOfCanvas;
        target.sprite = requirement.upgradeTarget.characterPortrait;
        for (int i = 0; i < upgradeMaterials.Count; i++)
        {
            if (i < requirement.obligatoryRequirements.Count)
            {
                upgradeMaterials[i].image.sprite = requirement.obligatoryRequirements[i].characterPortrait;
                if (!GameManager.Instance.ownedCharacters.Contains(requirement.obligatoryRequirements[i]))
                {
                    upgradeMaterials[i].interactable = false;
                } else
                {
                    upgradeMaterials[i].interactable = true;
                }
            } else
            {
                Rarity materialRarity = requirement.obligatoryRequirements[0].rarity;
                upgradeMaterials[i].image.color = GameManager.Instance.RarityToColor[materialRarity];
                upgradeMaterials[i].onClick.AddListener(() => ViewMaterials(materialRarity));
            }
        }
    }

    public override void Close()
    {
        base.Close();
        materialPanel.GetComponent<MaterialList>().Close();
    }

    private void ViewMaterials(Rarity rarity)
    {
        materialPanel.GetComponent<MaterialList>().Show(rarity);
    }

    private void Upgrade()
    {
        if (CheckUpgradeRequirements())
        {
            for (int i = 0; i < requirement.obligatoryRequirements.Count; i++) {
                GameManager.Instance.ownedCharacters.Remove(requirement.obligatoryRequirements[i]);
            }
            GameManager.Instance.ownedCharacters.Add(requirement.upgradeTarget);
            Close();
        } else
        {
            Debug.Log("Not enough requirement");
        }
    }

    private bool CheckUpgradeRequirements()
    {
        for (int i = 0; i < requirement.obligatoryRequirements.Count; i++)
        {
            if (!upgradeMaterials[i].interactable) return false;
        }

        for (int i = requirement.obligatoryRequirements.Count; i < upgradeMaterials.Count; i++)
        {
            if (upgradeMaterials[i].image.sprite == null) return false;
        }
        return true;
    }
}
