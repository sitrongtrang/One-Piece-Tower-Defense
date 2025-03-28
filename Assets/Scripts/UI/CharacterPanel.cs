using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterPanel : Panel
{

    [SerializeField] private GameObject characterCard;
    [SerializeField] private Button[] characterButtons;
    [SerializeField] private Button nextButton;
    [SerializeField] private Button previousButton;
    [SerializeField] private TextMeshProUGUI currentPageText;
    [SerializeField] private TextMeshProUGUI maxPageText;

    private const int numCharEachPage = 6;
    private CharacterData[] characters = new CharacterData[numCharEachPage];
    private int currentPage;
    private int numPages;

    protected override void Start()
    {
        base.Start();
        for (int i = 0; i < numCharEachPage; i++) characters[i] = null;
        previousButton.onClick.AddListener(ViewPreviousPage);
        nextButton.onClick.AddListener(ViewNextPage);
    }

    protected override void Setup(object data)
    {
        if (!(data is List<CharacterData> characters)) return;

        numPages = (int)Mathf.Ceil((CharacterInventory.Instance.CountOwnedCharacters() * 1.0f)/ numCharEachPage);

        for (int i = 0; i < numCharEachPage; i++)
        {
            if (i < characters.Count && characters[i] != null)
            {
                this.characters[i] = characters[i];
                characterButtons[i].image.sprite = characters[i].characterPortrait;
            } else
            {
                this.characters[i] = null;
                characterButtons[i].image.sprite = null;
            }

            int localIndex = i;
            characterButtons[i].onClick.AddListener(() => SelectCharacter(localIndex));
        }

        previousButton.gameObject.SetActive(currentPage != 0);
        nextButton.gameObject.SetActive(currentPage != numPages - 1);

        currentPageText.text = (currentPage + 1).ToString();
        maxPageText.text = "/" + numPages.ToString();
    }

    public override void Show(object data)
    {
        base.Show(data);
        PanelManager.Instance.OpenPanel(gameObject);
        characterCard.GetComponent<CharacterCard>().Show(characters[0]);
    }

    public override void Close()
    {
        base.Close();
        currentPage = 0;
        characterCard.GetComponent<CharacterCard>().Close();
    }

    public int GetNumCharEachPage() => numCharEachPage;

    private void SelectCharacter(int index)
    {
        if (characters[index] != null) characterCard.GetComponent<CharacterCard>().Show(characters[index]);
    }

    private void ViewNextPage()
    {
        currentPage++;
        List<CharacterData> characters = CharacterInventory.Instance.GetCharacters(currentPage * 6, (currentPage + 1) * 6);
        Show(characters);
    }

    private void ViewPreviousPage()
    {
        currentPage--;
        List<CharacterData> characters = CharacterInventory.Instance.GetCharacters(currentPage * 6, (currentPage + 1) * 6);
        Show(characters);
    }
}
