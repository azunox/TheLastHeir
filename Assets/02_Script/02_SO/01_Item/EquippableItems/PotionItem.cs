using UnityEngine;

namespace TheLastHeir.Runtime.Entity
{
    [CreateAssetMenu(fileName = "New Potion", menuName = "Inventory/Potion Item")]
    public class PotionItem : EquippableItem
    {
        [Header("Recovery Settings")]
        public int healthRecoveryAmount;
        public float staminaRecoveryAmount;

        [Header("Consumption")]
        public bool isConsumedOnUse = true;

        private void Reset()
        {
            equipmentSlot = EquipmentSlot.Potion;
            maxStackSize = 10; 
        }
    }
}