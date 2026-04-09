using UnityEngine;
using UnityEngine.UI;
using System;
using TheLastHeir.Runtime.Entity;

namespace TheLastHeir.Runtime.UI
{
    public class EquipmentSlotUI : MonoBehaviour
    {
        public EquipmentSlot slotType;
        [SerializeField] private Image iconImage;
        [SerializeField] private Sprite emptyIcon;
        public event Action<EquipmentSlot> OnEquipmentSlotClicked;

        public void Setup(Item item)
        {
            if (item != null)
            {
                iconImage.sprite = item.icon;
                iconImage.enabled = true;
                iconImage.color = Color.white;
            }
            else
            {
                if (emptyIcon != null)
                {
                    iconImage.sprite = emptyIcon;
                    iconImage.enabled = true;
                    iconImage.color = new Color(1, 1, 1, 0f);
                }
                else iconImage.enabled = false;
            }
        }

        public void OnClickSlot() => OnEquipmentSlotClicked?.Invoke(slotType);
    }
}