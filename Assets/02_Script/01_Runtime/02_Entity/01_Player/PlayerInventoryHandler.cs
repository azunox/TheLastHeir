using System.Collections.Generic;
using UnityEngine;
using System;

namespace TheLastHeir.Runtime.Entity
{
    public class PlayerInventoryHandler : MonoBehaviour
    {
        [Header("Inventory")]
        public List<InventorySlot> inventorySlots = new List<InventorySlot>();

        [Header("Equipment")]
        public EquippableItem[] currentEquipment;
        public int[] currentEquipmentStack;

        private PlayerAttributeHandler _attributeHandler;
        
        public event Action OnInventoryChanged;

        private void Awake()
        {
            int numSlots = System.Enum.GetNames(typeof(EquipmentSlot)).Length;
            currentEquipment = new EquippableItem[numSlots];
            currentEquipmentStack = new int[numSlots];

            _attributeHandler = GetComponent<PlayerAttributeHandler>();
        }

        private void Start()
        {
            CalculateEquipmentStats();
        }
        
        public void AddItem(Item item, int amount = 1)
        {
            if (item.maxStackSize > 1)
            {
                foreach (InventorySlot slot in inventorySlots)
                {
                    if (slot.item == item && slot.quantity < item.maxStackSize)
                    {
                        int space = item.maxStackSize - slot.quantity;
                        int addAmount = Mathf.Min(space, amount);

                        slot.AddQuantity(addAmount);
                        amount -= addAmount;

                        if (amount <= 0) break;
                    }
                }
            }
            
            while (amount > 0)
            {
                int addAmount = Mathf.Min(amount, item.maxStackSize);
                inventorySlots.Add(new InventorySlot(item, addAmount));
                amount -= addAmount;
            }

            OnInventoryChanged?.Invoke();
        }
        
        public void RemoveItem(Item item, int amount = 1)
        {
            for (int i = inventorySlots.Count - 1; i >= 0; i--)
            {
                if (inventorySlots[i].item == item)
                {
                    int removeAmount = Mathf.Min(amount, inventorySlots[i].quantity);
                    inventorySlots[i].quantity -= removeAmount;
                    amount -= removeAmount;

                    if (inventorySlots[i].quantity <= 0)
                    {
                        inventorySlots.RemoveAt(i);
                    }

                    if (amount <= 0) break;
                }
            }
            OnInventoryChanged?.Invoke();
        }
        
        public void EquipItem(InventorySlot slotToEquip)
        {
            if (!inventorySlots.Contains(slotToEquip)) return;
            if (!(slotToEquip.item is EquippableItem equippable)) return;

            int slotIndex = (int)equippable.equipmentSlot;

            if (currentEquipment[slotIndex] != null)
            {
                UnequipItem(slotIndex);
            }

            currentEquipment[slotIndex] = equippable;
            currentEquipmentStack[slotIndex] = slotToEquip.quantity;

            inventorySlots.Remove(slotToEquip);
            
            CalculateEquipmentStats();
            OnInventoryChanged?.Invoke();
        }
        
        public void UnequipItem(int slotIndex)
        {
            if (currentEquipment[slotIndex] == null) return;

            EquippableItem item = currentEquipment[slotIndex];
            int amount = currentEquipmentStack[slotIndex];

            AddItem(item, amount);

            currentEquipment[slotIndex] = null;
            currentEquipmentStack[slotIndex] = 0;

            CalculateEquipmentStats();
            OnInventoryChanged?.Invoke();
        }
        
        public void ConsumeCurrentPotion()
        {
            int index = (int)EquipmentSlot.Potion;
            PotionItem potion = GetEquippedPotion();

            if (potion != null)
            {
                if (potion.isConsumedOnUse)
                {
                    currentEquipmentStack[index]--;

                    if (currentEquipmentStack[index] <= 0)
                    {
                        currentEquipment[index] = null;
                        currentEquipmentStack[index] = 0;
                    }
                    
                    OnInventoryChanged?.Invoke();
                }
            }
        }

        public PotionItem GetEquippedPotion()
        {
            int index = (int)EquipmentSlot.Potion;
            if (index < currentEquipment.Length)
            {
                return currentEquipment[index] as PotionItem;
            }
            return null;
        }

        private void CalculateEquipmentStats()
        {
            if (_attributeHandler == null) return;

            int str = 0; int mag = 0; int def = 0;
            int hp = 0; int sta = 0; int mp = 0;

            foreach (var item in currentEquipment)
            {
                if (item != null)
                {
                    str += item.strengthBonus;
                    mag += item.magicBonus;
                    def += item.defenseBonus;
                    hp += item.healthBonus;
                    sta += item.staminaBonus;
                    mp += item.mpBonus;
                }
            }

            _attributeHandler.UpdateEquipmentBonuses(str, mag, def, hp, sta, mp);
        }
    }
}