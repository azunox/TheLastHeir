using UnityEngine;
using TheLastHeir.Runtime.Entity;

public class OpenStatUIHandler : EntityOwnedHandler<Player>
{
    private Player player;
    [SerializeField] private GameObject statPanel;
    void Start()
    {
        player = owner;
    }

    private void Update()
    {
        openCloseStatPanel();
        
    }
    public void openCloseStatPanel()
    {
        if (player.PlayerInput.StatTriggered)
        {
            statPanel.SetActive(!statPanel.activeSelf);
            player.PlayerInput.canInput = !statPanel.activeSelf; // 스탯창이 켜질 시 공격, 구르기 등 행동 금지
        }
    }
}
