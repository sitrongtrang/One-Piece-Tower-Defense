using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public interface ISpecification
{
    bool IsSatisfied(CharacterData character);
}

public class RaritySpecification : ISpecification
{
    private Rarity rarity;
    public RaritySpecification(Rarity rarity) => this.rarity = rarity;
    public bool IsSatisfied(CharacterData character) => character.rarity == rarity;
}

public class IdenticalSpecification : ISpecification
{
    private CharacterData characterData;
    public IdenticalSpecification(CharacterData characterData) => this.characterData = characterData;
    public bool IsSatisfied(CharacterData character) => object.ReferenceEquals(character, characterData);
}

public class AndSpecification : ISpecification
{
    private ISpecification first, second;
    public AndSpecification(ISpecification first, ISpecification second)
    {
        this.first = first;
        this.second = second;
    }
    public bool IsSatisfied(CharacterData character) => first.IsSatisfied(character) && second.IsSatisfied(character);
}

public class OrSpecification : ISpecification
{
    private ISpecification first, second;
    public OrSpecification(ISpecification first, ISpecification second)
    {
        this.first = first;
        this.second = second;
    }
    public bool IsSatisfied(CharacterData character) => first.IsSatisfied(character) || second.IsSatisfied(character);
}

public class NotSpecification : ISpecification
{
    private ISpecification spec;
    public NotSpecification(ISpecification spec) => this.spec = spec;
    public bool IsSatisfied(CharacterData character) => !spec.IsSatisfied(character);
}

public static class CharacterFilter
{
    public static List<CharacterData> Filter(List<CharacterData> characters, ISpecification spec)
    {
        return characters.Where(c => spec.IsSatisfied(c)).ToList();
    }
}
