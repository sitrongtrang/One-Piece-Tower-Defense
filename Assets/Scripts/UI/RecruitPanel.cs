using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;

public class RecruitPanel : Panel
{
    [SerializeField] private List<Button> recruits;
    [SerializeField] private Sprite emptyRecruitSlot;
    [SerializeField] private GameObject recruitInfoPanel;
    [SerializeField] private Button recruitLow;
    [SerializeField] private Button recruitMedium;
    [SerializeField] private Button recruitHigh;

    private CharacterData[] recruitedChars;
    private AssetReferenceT<CharacterData>[] recruitedCharReferences;
    private int choosingSlot = -1;

    protected override void Start()
    {
        base.Start();
        recruitedChars = new CharacterData[6];
        recruitedCharReferences = new AssetReferenceT<CharacterData>[6];

        recruitLow.onClick.AddListener(() => PerformGachaRecruit(PullMode.Low));
        recruitMedium.onClick.AddListener(() => PerformGachaRecruit(PullMode.Medium));
        recruitHigh.onClick.AddListener(() => PerformGachaRecruit(PullMode.High));
    }

    protected override void Setup(object data)
    {
        choosingSlot = -1;

        for (int i = 0; i < recruits.Count; i++)
        {
            int localIndex = i;
            recruitedChars[i] = null;

            if (recruitedCharReferences[i] != null)
            {
                CharacterLoader.LoadCharacter(recruitedCharReferences[i], (characterData) =>
                {
                    recruitedChars[localIndex] = characterData;
                    recruits[localIndex].image.sprite = recruitedChars[localIndex]?.characterPortrait ?? emptyRecruitSlot;
                });
            }
            else recruits[i].image.sprite = emptyRecruitSlot;

            recruits[i].onClick.RemoveAllListeners();
            recruits[i].onClick.AddListener(() => ViewRecruitInfo(localIndex));
        }
    }

    public override void Show(object data)
    {
        base.Show(data);
        PanelManager.Instance.OpenPanel(gameObject);
    }

    public override void Close()
    {
        base.Close();
        for (int i = 0; i < recruits.Count; i++) RemoveSlot(i);
    }

    public void PerformGachaRecruit(PullMode mode)
    {
        int num = (int)mode;

        for (int i = 0; i < recruits.Count; i++) RemoveSlot(i);

        int numChars = CharacterLoader.GetNumChar();

        if (numChars == 0)
        {
            Debug.LogError("No characters available in the pool.");
            return;
        }

        for (int i = 0; i < num; i++)
        {
            int index = Random.Range(0, numChars);
            AssetReferenceT<CharacterData> charRef = CharacterLoader.GetCharRef(index);
            recruitedCharReferences[i] = charRef;

            int localIndex = i;

            CharacterLoader.LoadCharacter(recruitedCharReferences[i], (characterData) =>
            {
                recruitedChars[localIndex] = characterData;
                recruits[localIndex].image.sprite = recruitedChars[localIndex]?.characterPortrait ?? emptyRecruitSlot;
            });
        }

        for (int i = num; i < recruits.Count; i++) recruitedCharReferences[i] = null;
        recruitInfoPanel.GetComponent<RecruitInfoPanel>().Close();
    }

    public void ViewRecruitInfo(int index)
    {
        choosingSlot = index;
        if (recruitedChars[index] != null) recruitInfoPanel.GetComponent<RecruitInfoPanel>().Show(recruitedChars[index]);
        else recruitInfoPanel.GetComponent<RecruitInfoPanel>().Close();
    }

    public void Recruit()
    {
        CharacterData character = recruitedChars[choosingSlot];
        if (CharacterInventory.Instance.HasCharacter(character)) Debug.Log("Already own character");
        else
        {
            CharacterInventory.Instance.AddCharacter(character);
            RemoveSlot(choosingSlot);
            recruitedCharReferences[choosingSlot] = null;
            recruits[choosingSlot].image.sprite = emptyRecruitSlot;
            choosingSlot = -1;
            recruitInfoPanel.GetComponent<RecruitInfoPanel>().Close();
        }
    }

    public void RemoveSlot(int index)
    {
        if (recruitedChars[index] != null) CharacterLoader.ReleaseCharacter(recruitedChars[index]);
        recruitedChars[index] = null;
    }
}
