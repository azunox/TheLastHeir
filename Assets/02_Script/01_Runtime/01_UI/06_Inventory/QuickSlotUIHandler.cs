using UnityEngine;
using UnityEngine.UI;
using TheLastHeir.Runtime.Entity;

namespace TheLastHeir.Runtime.UI
{
    public class QuickSlotUIHandler : MonoBehaviour
    {
        [SerializeField] private PlayerInventoryHandler inventoryHandler;
        [SerializeField] private Image weaponIconImage;
        [SerializeField] private Image potionIconImage;
        [SerializeField] private Sprite emptySlotSprite; 

        private void Start()
        {
            if (inventoryHandler == null)
            {
                var player = FindObjectOfType<Player>();
                if (player != null) inventoryHandler = player.GetComponent<PlayerInventoryHandler>();
            }

            if (inventoryHandler != null)
            {
                UpdateQuickSlots();
                inventoryHandler.OnInventoryChanged += UpdateQuickSlots;
            }
        }

        private void OnDestroy()
        {
            if (inventoryHandler != null) inventoryHandler.OnInventoryChanged -= UpdateQuickSlots;
        }

        public void UpdateQuickSlots()
        {
            UpdateSlotUI(EquipmentSlot.Weapon, weaponIconImage);
            UpdateSlotUI(EquipmentSlot.Potion, potionIconImage);
        }

        private void UpdateSlotUI(EquipmentSlot slotType, Image iconImage)
        {
            if (iconImage == null || inventoryHandler == null) return;
            int index = (int)slotType;
            
            if (index >= 0 && index < inventoryHandler.currentEquipment.Length)
            {
                EquippableItem item = inventoryHandler.currentEquipment[index];
                if (item != null && item.icon != null)
                {
                    iconImage.sprite = item.icon;
                    iconImage.enabled = true;
                    iconImage.color = Color.white; 
                }
                else
                {
                    if (emptySlotSprite != null) { iconImage.sprite = emptySlotSprite; iconImage.color = new Color(1, 1, 1, 0.2f); }
                    else { iconImage.enabled = false; }
                }
            }
        }
    }
}