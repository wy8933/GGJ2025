using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject SettingsPanel;
    public GameObject CreditsPanel;

    public void OnStartClicked() 
    {
        SceneManager.LoadScene(1);
    }
    public void OnSettingClicked()
    {
        SettingsPanel.SetActive(true);
    }

    public void OnSettingClose() 
    {
        SettingsPanel.SetActive(false);
    }

    public void OnCreditClicked() 
    {
        CreditsPanel.SetActive(true);
    }

    public void OnCreditClose() 
    {
        CreditsPanel.SetActive(false);
    }

    public void OnExitClicked() {
        Application.Quit();
    }

    public void OnMainMenuClicked() {
        SceneManager.LoadScene(0);
    }
}
