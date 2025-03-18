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

    public List<CharacterData> chosenMaterial;
    private int choosingSlot;

    protected override void Start()
    {
        base.Start();
        canvas = FindObjectOfType<Canvas>();
        upgradeButton.onClick.AddListener(Upgrade);
        chosenMaterial = new List<CharacterData>();
    }

    protected override void Setup(object data)
    {
        if (!(data is UpgradeRequirement requirement)) return;

        chosenMaterial = new List<CharacterData>();
        choosingSlot = -1;

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
                    chosenMaterial.Add(requirement.obligatoryRequirements[i]);
                }
            } else
            {
                int localIndex = i;
                Rarity materialRarity = requirement.obligatoryRequirements[0].rarity;
                upgradeMaterials[i].image.color = GameManager.Instance.RarityToColor[materialRarity];
                upgradeMaterials[i].onClick.AddListener(() => ViewMaterials(materialRarity, localIndex));
            }
        }
    }

    public override void Close()
    {
        base.Close();
        materialPanel.GetComponent<MaterialList>().Close();
    }

    private void ViewMaterials(Rarity rarity, int index)
    {
        choosingSlot = index;
        materialPanel.GetComponent<MaterialList>().Show(rarity);
    }

    public void ChooseMaterial(CharacterData material)
    {
        upgradeMaterials[choosingSlot].image.color = Color.white;
        upgradeMaterials[choosingSlot].image.sprite = material.characterPortrait;
        chosenMaterial.Add(material);
        choosingSlot = -1;
        materialPanel.GetComponent<MaterialList>().Close();
    }

    private void Upgrade()
    {
        if (CheckUpgradeRequirements())
        {
            for (int i = 0; i < requirement.obligatoryRequirements.Count; i++) {
                GameManager.Instance.ownedCharacters.Remove(requirement.obligatoryRequirements[i]);
            }
            GameManager.Instance.ownedCharacters.Add(requirement.upgradeTarget);
            PanelManager.Instance.CloseAllPanels();
            GameManager.Instance.ViewCharacters(0);
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
