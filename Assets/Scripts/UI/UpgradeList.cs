using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeList : ScrollViewPanel
{
    private List<GameObject> upgradeOptionPool = new();

    protected override void Setup(object data)
    {
        if (!(data is List<UpgradeRequirement> requirements)) return;

        options = new();

        for (int i = 0; i < requirements.Count; i++) options.Add(requirements[i]);

        // Instantiate more game objects if pool is not enough, otherwise reuse old objects
        for (int i = upgradeOptionPool.Count; i < options.Count; i++)
        {
            GameObject newOption = Instantiate(optionPrefab, contentPanel);
            newOption.SetActive(false);
            upgradeOptionPool.Add(newOption);
        }

        for (int i = 0; i < options.Count; i++) upgradeOptionPool[i].GetComponent<UpgradeOption>().Show(options[i]);

        base.Setup(data);
    }

    public override void Close()
    {
        base.Close();
        foreach (GameObject option in upgradeOptionPool) option.GetComponent<UpgradeOption>().Close();
    }
}
