using System;
using TheLastHeir.Runtime.Entity;
using TMPro;
using UnityEngine;

namespace TheLastHeir.Runtime.UI
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class AmsoDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI AmsoDisplayText;
        [SerializeField] private PlayerAttributeHandler playerAttributeHandler;

        private void Update()
        {
            AmsoDisplayText.text = playerAttributeHandler.Amso.ToString();
        }
    }
}