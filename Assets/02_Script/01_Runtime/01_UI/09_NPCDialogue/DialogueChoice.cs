using UnityEngine;
using System.Collections.Generic;
using TheLastHeir.Runtime.Shop;

namespace TheLastHeir.Runtime.UI
{
    [System.Serializable]
    public class DialogueChoice
    {
        public string choiceText;          
        public DialogueData nextDialogue;  
        
        [Header("Event")]
        public ShopData shopData;
    }
}
