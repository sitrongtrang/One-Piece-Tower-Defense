using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

[System.Serializable]
public class UpgradeRequirement
{
    public AssetReferenceT<CharacterData> upgradeTarget; // The character this upgrade leads to
    public List<AssetReferenceT<CharacterData>> obligatoryRequirements; // Required specific characters
}

[CreateAssetMenu(fileName = "NewCharacterData", menuName = "Character/Character Data")]
public class CharacterData : ScriptableObject
{
    public Sprite characterPortrait;
    public string characterName;
    public Rarity rarity;
    public float attackPower;
    public float attackSpeed;
    public float range;
    public float health;

    [Header("Animations")]
    public string attackAnimation;
    public string skillAnimation;

    [Header("Upgrade Info")]
    public List<UpgradeRequirement> upgradeOptions;
}
