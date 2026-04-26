using UnityEngine;

public class InfoMenu : MonoBehaviour
{
    public GameObject infoPanel;
    public GameObject mainMenuPanel;

    // OPEN INFO
    public void OpenInfo()
    {
        infoPanel.SetActive(true);
        mainMenuPanel.SetActive(false);
    }

    // CLOSE INFO (BACK BUTTON)
    public void CloseInfo()
    {
        infoPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }
}