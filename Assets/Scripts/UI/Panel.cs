using UnityEngine;
using UnityEngine.UI;

public abstract class Panel : MonoBehaviour
{
    [SerializeField] protected Button closeButton;

    protected virtual void Start()
    {
        gameObject.SetActive(false);
        if (closeButton != null) closeButton.onClick.AddListener(Close);
    }

    // Setup data needed for displaying
    protected abstract void Setup(object data);

    // Display the UI element with the received data
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
