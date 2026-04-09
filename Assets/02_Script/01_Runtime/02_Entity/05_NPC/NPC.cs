using UnityEngine;
using TheLastHeir.Runtime.UI;
using TheLastHeir.Runtime.Interactions;

namespace TheLastHeir.Runtime.Entity
{
    public class NPC : MonoBehaviour, IInteractable
    {
        [SerializeField] private DialogueData dialogueData;

        public string GetInteractionPrompt()
        {
            return dialogueData != null ? $"{dialogueData.npcName}와 대화하기 (E)" : "대화하기";
        }

        public void Interact(PlayerInteraction player)
        {
            if (dialogueData != null)
            {
                Debug.Log("Dialogue Started");
                if (DialogueManager.Instance != null)
                {
                    
                    DialogueManager.Instance.StartDialogue(dialogueData);
                }
            }
        }
    }
}