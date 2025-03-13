using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCharacter : MonoBehaviour
{
    public CharacterData characterData;
    public Animator animator;

    private GameManager gameManager;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Attack()
    {
        if (animator && !string.IsNullOrEmpty(characterData.attackAnimation))
        {
            animator.Play(characterData.attackAnimation);
        }
    }

    public void UseSkill()
    {
        if (animator && !string.IsNullOrEmpty(characterData.skillAnimation))
        {
            animator.Play(characterData.skillAnimation);
        }
    }
}
