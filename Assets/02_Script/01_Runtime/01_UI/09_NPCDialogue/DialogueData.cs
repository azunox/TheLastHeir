using UnityEngine;
using System.Collections.Generic;

namespace TheLastHeir.Runtime.UI
{
    [CreateAssetMenu(fileName = "NewDialogue", menuName = "Dialogue/Simple Dialogue Data")]
    public class DialogueData : ScriptableObject
    {
        public string npcName;
        [TextArea(3, 10)]
        public string sentence;
        public List<DialogueChoice> choices;
    }
}