using UnityEngine;

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

    public void OpenPanel(GameObject panel)
    {
        for (int i = 0; i < panels.Length; i++)
        {
            // When a panel opens, close all other panels
            if (panels[i] != panel && panels[i].activeInHierarchy) panels[i].GetComponent<Panel>().Close(); 
        }
    }

    public void CloseAllPanels()
    {
        foreach (GameObject panel in panels) panel.GetComponent<Panel>().Close();
    }
}
