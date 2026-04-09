using TheLastHeir.Runtime.Entity;
using UnityEngine;

public class OpenCloseInventory : EntityOwnedHandler<Player>
{
    private Player player;
    [SerializeField] private GameObject inventoryCanvas;
    void Start()
    {
        player = owner;
        //player.PlayerInput.OnInventoryOpenClose += openCloseInventory;
    }

    private void Update()
    {
        openCloseInventory();
    }
    public void openCloseInventory()
    {
        if (player.PlayerInput.InventoryTriggered)
        {
            inventoryCanvas.SetActive(!inventoryCanvas.activeSelf);
            player.PlayerInput.canInput = !inventoryCanvas.activeSelf;
        }

        
    }
}
