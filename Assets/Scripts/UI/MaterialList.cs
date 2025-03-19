using System.Collections.Generic;
using UnityEngine;

public class MaterialList : ScrollViewPanel
{
    [SerializeField] private GameObject upgradePanel;

    private List<GameObject> materialOptionPool = new();

    protected override void Setup(object data)
    {
        if (!(data is List<CharacterData> characters)) return;

        options = new();

        for (int i = 0; i < characters.Count; i++) options.Add(characters[i]);

        // Instantiate more game objects if pool is not enough, otherwise reuse old objects
        for (int i = materialOptionPool.Count; i < characters.Count; i++)
        {
            GameObject newOption = Instantiate(optionPrefab, contentPanel);
            newOption.GetComponent<MaterialOption>().setMaterialPanel(gameObject);
            newOption.SetActive(false);
            materialOptionPool.Add(newOption);
        }

        for (int i = 0; i < characters.Count; i++) materialOptionPool[i].GetComponent<MaterialOption>().Show(characters[i]);

        base.Setup(data);
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
}
