using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecruitPanel : Panel
{
    [SerializeField] private List<Button> recruits;
    [SerializeField] private Sprite emptyRecruitSlot;
    [SerializeField] private GameObject recruitInfoPanel;

    private CharacterData[] recruitedChars;
    private int choosingSlot = -1;

    protected override void Start()
    {
        base.Start();
        recruitedChars = new CharacterData[6];
    }

    protected override void Setup(object data)
    {
        choosingSlot = -1;
        for (int i = 0; i < recruits.Count; i++)
        {
            if (recruitedChars[i] != null) recruits[i].image.sprite = recruitedChars[i].characterPortrait;
            else recruits[i].image.sprite = emptyRecruitSlot;
            int localIndex = i;
            recruits[i].onClick.AddListener(() => ViewRecruitInfo(localIndex));
        }
    }

    public override void Show(object data)
    {
        base.Show(data);
        PanelManager.Instance.OpenPanel(gameObject);
    }

    public void PerformGachaRecruit(int num)
    {
        for (int i = 0; i < recruits.Count; i++) RemoveSlot(i);
        for (int i = 0; i < num; i++)
        {
            int index = i;
            GameManager.Instance.Recruit(characterData => { recruitedChars[index] = characterData; Show(num); });
        }
        for (int i = num; i < recruits.Count; i++) recruitedChars[i] = null;
        recruitInfoPanel.GetComponent<RecruitInfoPanel>().Close();
    }

    public void ViewRecruitInfo(int index)
    {
        choosingSlot = index;
        if (recruitedChars[index] != null) recruitInfoPanel.GetComponent<RecruitInfoPanel>().Show(recruitedChars[index]);
        else recruitInfoPanel.GetComponent<RecruitInfoPanel>().Close();
    }

    public void Recruit(CharacterData character)
    {
        RemoveSlot(choosingSlot);
        choosingSlot = -1;
        recruitInfoPanel.GetComponent<RecruitInfoPanel>().Close();
    }

    public void RemoveSlot(int index)
    {
        if (recruitedChars[index] != null) GameManager.Instance.ReleaseCharacter(recruitedChars[index]);
        recruitedChars[index] = null;
        Show(6);
    }
}
