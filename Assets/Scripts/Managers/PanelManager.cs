using UnityEngine;
using UnityEngine.UI;

public class PanelManager : MonoBehaviour
{
    public static PanelManager Instance { get; private set; }

    [SerializeField] private GameObject[] panels;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    public void OpenPanel(Panel panel)
    {
        for (int i = 0; i < panels.Length; i++)
        {
            if (panels[i] != panel.gameObject && panels[i].activeInHierarchy)
                panels[i].GetComponent<Panel>().Close(); 
        }
    }

    public void CloseAllPanels()
    {
        foreach (GameObject panel in panels)
        {
            panel.GetComponent<Panel>().Close();
        }
    }
}
