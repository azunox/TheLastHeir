using UnityEngine;
using TheLastHeir.Runtime.Interactions;

namespace TheLastHeir.Runtime.Entity
{
    [RequireComponent(typeof(Collider))]
    public class ItemPickup : MonoBehaviour, IInteractable
    {
        public Item itemToPickup; 
        public int amount = 1; 

        public string GetInteractionPrompt()
        {
            if (itemToPickup != null)
                return $"{itemToPickup.itemName} x{amount} 줍기 (E)";
            return "아이템 줍기 (E)";
        }

        public void Interact(PlayerInteraction player)
        {
            if (itemToPickup != null)
            {
                player.inventoryHandler.AddItem(itemToPickup, amount);
                Destroy(gameObject);
            }
        }
        
        public void Setup(Item item, int count)
        {
            itemToPickup = item;
            amount = count;
        }
    }
}