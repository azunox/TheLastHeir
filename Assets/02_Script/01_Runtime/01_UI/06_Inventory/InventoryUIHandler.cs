using UnityEngine;
using System.Collections.Generic;
using TheLastHeir.Runtime.Entity;
using TheLastHeir.Runtime.UI;

public class InventoryUIHandler : MonoBehaviour
{
    [SerializeField] private PlayerInventoryHandler inventoryHandler;
    [SerializeField] private GameObject itemSlotPrefab;
    [SerializeField] private Transform contentArea;
    [SerializeField] private List<EquipmentSlotUI> equipmentSlots; 
    [SerializeField] private ItemDescriptionUI descriptionFrame;

    void Start()
    {
        if (inventoryHandler == null)
        {
            var player = FindObjectOfType<Player>();
            if (player != null) inventoryHandler = player.GetComponent<PlayerInventoryHandler>();
        }

        if (inventoryHandler != null)
        {
            inventoryHandler.OnInventoryChanged += RedrawAll;
            RedrawAll();
        }

        foreach (var slotUI in equipmentSlots)
            slotUI.OnEquipmentSlotClicked += HandleEquipmentSlotClick;
    }
    
    private void OnDestroy()
    {
        if (inventoryHandler != null) inventoryHandler.OnInventoryChanged -= RedrawAll;
    }

    public void RedrawAll()
    {
        foreach (Transform child in contentArea) Destroy(child.gameObject);
        if (descriptionFrame != null) descriptionFrame.ClearDescription();

        foreach (InventorySlot slot in inventoryHandler.inventorySlots)
        {
            GameObject newSlot = Instantiate(itemSlotPrefab, contentArea);
            ItemSlotUI slotUI = newSlot.GetComponent<ItemSlotUI>();
            if (slotUI != null)
            {
                slotUI.Setup(slot);
                slotUI.OnItemSlotClicked += HandleInventoryItemClick;
            }
        }

        foreach (var slotUI in equipmentSlots)
        {
            int index = (int)slotUI.slotType;
            if (index >= 0 && index < inventoryHandler.currentEquipment.Length)
                slotUI.Setup(inventoryHandler.currentEquipment[index]);
        }
    }

    private void HandleInventoryItemClick(InventorySlot slot)
    {
        if (descriptionFrame == null) return;
        Item item = slot.item;
        
        if (item is EquippableItem)
            descriptionFrame.UpdateDescription(item, "장착", () => inventoryHandler.EquipItem(slot));
        else
            descriptionFrame.UpdateDescription(item, "", null);
    }

    private void HandleEquipmentSlotClick(EquipmentSlot slotType)
    {
        int index = (int)slotType;
        EquippableItem item = inventoryHandler.currentEquipment[index];

        if (item != null)
            descriptionFrame.UpdateDescription(item, "해제", () => inventoryHandler.UnequipItem(index));
        else
            descriptionFrame.ClearDescription();
    }
}