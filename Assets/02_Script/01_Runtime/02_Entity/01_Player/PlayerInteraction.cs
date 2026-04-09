using UnityEngine;
using TheLastHeir.Runtime.Interactions;

namespace TheLastHeir.Runtime.Entity
{
    public class PlayerInteraction : MonoBehaviour
    {
        public PlayerInventoryHandler inventoryHandler;
        [SerializeField] private PlayerInputHandler inputHandler;
        private IInteractable currentTarget;

        private void Update()
        {
            if (currentTarget != null && inputHandler.InteractTriggered)
            {
                currentTarget.Interact(this);
            }
        }

        private void OnTriggerEnter(Collider other)
        { 
            IInteractable interactable = other.GetComponent<IInteractable>();
            if (interactable != null)
            {
                currentTarget = interactable;
                Debug.Log("상호작용 가능: " + interactable.GetInteractionPrompt());
            }
        }

        private void OnTriggerExit(Collider other)
        {
            IInteractable interactable = other.GetComponent<IInteractable>();
            if (interactable == currentTarget)
            {
                currentTarget = null;
            }
        }
        public PlayerInventoryHandler Inventory => inventoryHandler;
    }
}