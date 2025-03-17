using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterPanel : Panel
{

    [SerializeField] private GameObject characterCard;
    [SerializeField] private List<Button> characterButtons;
    [SerializeField] private Button nextButton;
    [SerializeField] private Button previousButton;

    private CharacterData[] characters = new CharacterData[6];
    private int currentPage;
    private int numPages;

    protected override void Start()
    {
        base.Start();
        for (int i = 0; i < 6; i++) characters[i] = null;
        previousButton.onClick.AddListener(ViewPreviousPage);
        nextButton.onClick.AddListener(ViewNextPage);
    }

    protected override void Setup(object data)
    {
        if (!(data is List<CharacterData> characters)) return;
        numPages = (int)Mathf.Ceil(GameManager.Instance.ownedCharacters.Count / 6.0f);
        for (int i = 0; i < 6; i++)
        {
            int localIndex = i;
            if (i < characters.Count && characters[i] != null)
            {
                this.characters[i] = characters[i];
                characterButtons[i].image.sprite = characters[i].characterPortrait;
            } else
            {
                this.characters[i] = null;
                characterButtons[i].image.sprite = null;
            }
            characterButtons[i].onClick.AddListener(() => OnCharacterSelected(localIndex));
        }
        Debug.Log(currentPage);
        previousButton.gameObject.SetActive(currentPage != 0);
        nextButton.gameObject.SetActive(currentPage != numPages - 1);
    }

    public override void Show(object data)
    {
        base.Show(data);
        PanelManager.Instance.OpenPanel(gameObject);
        characterCard.GetComponent<CharacterCardUI>().Show(characters[0]);
    }

    public override void Close()
    {
        base.Close();
        characterCard.GetComponent<CharacterCardUI>().Close();
    }

    private void OnCharacterSelected(int index)
    {
        if (characters[index] != null) characterCard.GetComponent<CharacterCardUI>().Show(characters[index]);
    }

    private void ViewNextPage()
    {
        currentPage++;
        GameManager.Instance.ViewCharacters(currentPage * 6);
    }

    private void ViewPreviousPage()
    {
        currentPage--;
        GameManager.Instance.ViewCharacters(currentPage * 6);
    }
}
