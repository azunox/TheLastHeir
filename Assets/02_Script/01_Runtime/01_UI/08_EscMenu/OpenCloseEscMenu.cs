using UnityEngine;
using TheLastHeir.Runtime.Entity;
using UnityEngine.SceneManagement;

public class OpenCloseEscMenu : EntityOwnedHandler<Player>
{

    private Player player;
    [SerializeField] private GameObject menuPanel;
    void Start()
    {
        player = owner;
    }

    private void Update()
    {
        openCloseMenuPanel();
        
    }
    
    public void openCloseMenuPanel()
    {
        if (player.PlayerInput.MainMenuTriggered)
        {
            menuPanel.SetActive(!menuPanel.activeSelf);
            player.PlayerInput.canInput = !menuPanel.activeSelf;
        }
    }
}
