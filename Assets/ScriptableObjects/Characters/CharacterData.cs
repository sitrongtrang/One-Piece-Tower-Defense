using System.Collections.Generic;
using UnityEngine;

public enum Rarity
{
    Common,
    Uncommon,
    Rare,
    Epic,
    Legendary,
    Unique,
    Mythic,
    Awakened
}

[System.Serializable]
public class UpgradeRequirement
{
    public CharacterData upgradeTarget; // The character this upgrade leads to
    public List<CharacterData> obligatoryRequirements; // Required specific characters
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
