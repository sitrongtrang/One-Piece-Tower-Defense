using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCardUIManager : MonoBehaviour
{
    public static CharacterCardUIManager Instance { get; private set; }

    [SerializeField]
    private GameObject characterCard;
    [SerializeField]
    private GameObject parentUI;
    private CharacterCardUI cardUI;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnCharacterSelected(CharacterData characterData)
    {
        if (characterCard.activeInHierarchy) return;
        characterCard.GetComponent<CharacterCardUI>().ShowInfo(characterData);
    }
}
