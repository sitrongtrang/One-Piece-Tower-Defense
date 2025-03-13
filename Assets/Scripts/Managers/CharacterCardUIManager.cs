using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCardUIManager : MonoBehaviour
{
    public static CharacterCardUIManager Instance { get; private set; }

    public GameObject characterCardPrefab;
    public GameObject parentUI;
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
        GameObject cardObj = Instantiate(characterCardPrefab, parentUI.GetComponent<Transform>());
        cardUI = cardObj.GetComponent<CharacterCardUI>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnCharacterSelected(CharacterData characterData)
    {
        cardUI.ShowInfo(characterData);
    }
}
