using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradePanel : Panel
{
    [SerializeField] private Button[] upgradeMaterials;
    [SerializeField] private Image target;
    [SerializeField] private Button upgradeButton;
    [SerializeField] private GameObject materialPanel;

    private UpgradeRequirement requirement;
    private Canvas canvas;
    private int choosingSlot;
    private const int numMaterials = 5;
    private CharacterData[] chosenMaterial = new CharacterData[numMaterials];

    protected override void Start()
    {
        base.Start();
        for (int i = 0; i < numMaterials; i++) chosenMaterial[i] = null;
        canvas = FindObjectOfType<Canvas>();
        upgradeButton.onClick.AddListener(Upgrade);
    }

    protected override void Setup(object data)
    {
        if (!(data is UpgradeRequirement requirement)) return;

        choosingSlot = -1;
        this.requirement = requirement;

        Vector3 middleOfCanvas = canvas.transform.position;
        transform.position = middleOfCanvas;

        target.sprite = requirement.upgradeTarget.characterPortrait;
        for (int i = 0; i < numMaterials; i++)
        {
            upgradeMaterials[i].onClick.RemoveAllListeners();
            if (i < requirement.obligatoryRequirements.Count)
            {
                upgradeMaterials[i].image.sprite = requirement.obligatoryRequirements[i].characterPortrait;

                if (!GameManager.Instance.characterInventory.Contains(requirement.obligatoryRequirements[i]))
                {
                    chosenMaterial[i] = null;
                    upgradeMaterials[i].interactable = false;
                }
                else
                {
                    chosenMaterial[i] = requirement.obligatoryRequirements[i];
                    upgradeMaterials[i].interactable = true;
                }

            } else
            {
                chosenMaterial[i] = null;
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
        chosenMaterial[choosingSlot] = material;
        choosingSlot = -1;
        materialPanel.GetComponent<MaterialList>().Close();
    }


    // Check if material is already chosen
    public bool IsChosenMaterial(CharacterData material)
    {
        for (int i = 0; i < numMaterials; i++)
        {
            if (chosenMaterial[i] == material) return true;
        }
        return false;
    }

    private void Upgrade()
    {
        if (CheckUpgradeRequirements())
        {
            // Satisfy requirement, remove all material from player's inventory, and add the upgraded character to the player's inventory
            for (int i = 0; i < numMaterials; i++) {
                GameManager.Instance.characterInventory.Remove(chosenMaterial[i]);
            }
            GameManager.Instance.characterInventory.Add(requirement.upgradeTarget);
            PanelManager.Instance.CloseAllPanels();
            GameManager.Instance.ViewCharacters(0);
        } else Debug.Log("Not enough requirement");
    }

    private bool CheckUpgradeRequirements()
    {
        for (int i = 0; i < numMaterials; i++) if (chosenMaterial[i] == null) return false;

        return true;
    }
}
