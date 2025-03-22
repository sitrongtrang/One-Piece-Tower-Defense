using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradePanel : Panel
{
    [SerializeField] private Button[] upgradeMaterials;
    [SerializeField] private Image target;
    [SerializeField] private Button upgradeButton;
    [SerializeField] private GameObject materialPanel;
    [SerializeField] private Sprite emptySlot;

    private UpgradeRequirement requirement;
    private Canvas canvas;
    private int choosingSlot = -1;
    private const int numMaterials = 5;
    private CharacterData[] chosenMaterial = new CharacterData[numMaterials];
    private CharacterData targetCharacter;

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

        targetCharacter = null;
        if (requirement.upgradeTarget != null)
        {
            CharacterLoader.LoadCharacter(requirement.upgradeTarget, (characterData) =>
            {
                targetCharacter = characterData;
                target.sprite = targetCharacter?.characterPortrait ?? emptySlot;
                if (targetCharacter != null)
                {
                    for (int i = requirement.obligatoryRequirements.Count; i < numMaterials; i++)
                    {
                        chosenMaterial[i] = null;
                        int localIndex = i;
                        Rarity materialRarity = (Rarity)((int)targetCharacter.rarity - 1);
                        upgradeMaterials[i].image.color = RarityMapper.RarityToColor[materialRarity];
                        upgradeMaterials[i].onClick.AddListener(() => ViewMaterials(materialRarity, localIndex));
                    }
                }

            });
        }

        for (int i = 0; i < numMaterials; i++)
        {
            upgradeMaterials[i].onClick.RemoveAllListeners();
            if (i < requirement.obligatoryRequirements.Count)
            {
                int localIndex = i;
                CharacterData oblReq = null;
                CharacterLoader.LoadCharacter(requirement.obligatoryRequirements[i], (characterData) =>
                {
                    oblReq = characterData;
                    upgradeMaterials[localIndex].image.sprite = oblReq?.characterPortrait ?? emptySlot;
                    if (oblReq != null) 
                    {
                        if (!CharacterInventory.Instance.HasCharacter(oblReq))
                        {
                            chosenMaterial[localIndex] = null;
                            upgradeMaterials[localIndex].interactable = false;
                        }
                        else
                        {
                            chosenMaterial[localIndex] = oblReq;
                            upgradeMaterials[localIndex].interactable = true;
                        }
                        CharacterLoader.ReleaseCharacter(oblReq);
                    }
                });
            } 
        }
    }

    public override void Close()
    {
        base.Close();
        choosingSlot = -1;
        for (int i = requirement.obligatoryRequirements.Count; i < numMaterials; i++) upgradeMaterials[i].image.sprite = null;
        materialPanel.GetComponent<MaterialList>().Close();
        if (targetCharacter != null) CharacterLoader.ReleaseCharacter(targetCharacter);
        targetCharacter = null;
    }

    public void ChooseMaterial(CharacterData material)
    {
        upgradeMaterials[choosingSlot].image.color = Color.white;
        upgradeMaterials[choosingSlot].image.sprite = material.characterPortrait;
        chosenMaterial[choosingSlot] = material;
        choosingSlot = -1;
        materialPanel.GetComponent<MaterialList>().Close();
    }

    private void ViewMaterials(Rarity rarity, int index)
    {
        choosingSlot = index;

        // Find materials with input rarity while filtering out already chosen materials
        var materialSpec = new AndSpecification(new RaritySpecification(rarity), new NotSpecification(new IdenticalSpecification(chosenMaterial[0])));
        for (int i = 1; i < numMaterials; i++)
        {
            // filter out chosen material
            if (chosenMaterial[i]) materialSpec = new AndSpecification(materialSpec, new NotSpecification(new IdenticalSpecification(chosenMaterial[i]))); 
        }

        List<CharacterData> characters = CharacterFilter.Filter(CharacterInventory.Instance.GetCharacters(), materialSpec);
        materialPanel.GetComponent<MaterialList>().Show(characters);
    }

    private void Upgrade()
    {
        if (CheckUpgradeRequirements())
        {
            // Satisfy requirement, remove all material from player's inventory, and add the upgraded character to the player's inventory
            if (CharacterInventory.Instance.HasCharacter(targetCharacter))
            {
                Debug.Log("Already have character");
                return;
            }
            for (int i = 0; i < numMaterials; i++) {
                CharacterInventory.Instance.RemoveCharacter(chosenMaterial[i]);
            }
            CharacterInventory.Instance.AddCharacter(targetCharacter);
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
