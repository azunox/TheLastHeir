using UnityEngine;
using UnityEngine.SceneManagement;

public class EscMenuHandler : MonoBehaviour
{
    public GameObject menuPanel;
    public GameObject settingsPanel;
    
    public void resumButtonClicked()
    {
        menuPanel.SetActive(false);
    }

    public void settingsButtonClicked()
    {
        settingsPanel.SetActive(true);
        menuPanel.SetActive(false);
    }

    public void returnMainLobbyClicked()
    {
        SceneManager.LoadScene(0);
    }

    public void quitButtonClicked()
    {
        Debug.Log("종료");
    }
}
