using UnityEngine;
using UnityEngine.UI;

public class PanelManager : MonoBehaviour
{
    public GameObject[] panels; 
    public Button[] buttons; 

    public void OpenPanel(int index)
    {
        for (int i = 0; i < panels.Length; i++)
        {
            if (i == index)
                panels[i].SetActive(true); 
            else
                panels[i].SetActive(false); 
        }
    }

    public void CloseAllPanels()
    {
        foreach (GameObject panel in panels)
        {
            panel.SetActive(false);
        }
    }
}
