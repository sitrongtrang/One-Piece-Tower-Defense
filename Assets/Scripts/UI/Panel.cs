using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DataSlotInfo
{
    public CharacterData data;
    public int slotNum;

    public DataSlotInfo(CharacterData data, int slotNum)
    {
        this.data = data;
        this.slotNum = slotNum;
    }
}

public abstract class Panel : MonoBehaviour
{
    [SerializeField] protected Button closeButton;

    protected virtual void Start()
    {
        gameObject.SetActive(false);
        if (closeButton != null)
            closeButton.onClick.AddListener(Close);
    }

    protected abstract void Setup(object data);

    public virtual void Show(object data)
    {
        Setup(data); 
        gameObject.SetActive(true);
    }

    public virtual void Close()
    {
        gameObject.SetActive(false);
    }
}
