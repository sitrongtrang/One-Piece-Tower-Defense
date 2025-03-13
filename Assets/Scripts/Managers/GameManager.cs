using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameObject characterPrefab;

    public GameObject viewCharacterButton;
    private bool isViewingCharacters;

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

    public void ViewCharacterInfo()
    {
        if (isViewingCharacters) return;
        isViewingCharacters = true;
        CharacterData data = Instantiate(characterPrefab).GetComponent<TowerCharacter>().characterData;
        CharacterCardUIManager.Instance.OnCharacterSelected(data);
    }

    public void CloseCharacterInfo()
    {
        if (!isViewingCharacters) return;
        isViewingCharacters = false;
    }
}
