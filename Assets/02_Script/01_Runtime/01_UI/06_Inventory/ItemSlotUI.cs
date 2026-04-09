using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using TheLastHeir.Runtime.Entity;

public class ItemSlotUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI quantityText;

    private InventorySlot _currentSlot;
    
    public event Action<InventorySlot> OnItemSlotClicked;

    public void Setup(InventorySlot slot)
    {
        _currentSlot = slot;

        if (slot != null && slot.item != null)
        {
            iconImage.sprite = slot.item.icon;
            iconImage.enabled = true;
            
            if (quantityText != null)
            {
                if (slot.quantity > 1)
                {
                    quantityText.text = slot.quantity.ToString();
                    quantityText.gameObject.SetActive(true);
                }
                else
                {
                    quantityText.gameObject.SetActive(false);
                }
            }
        }
        else
        {
            iconImage.enabled = false;
            if (quantityText != null) quantityText.gameObject.SetActive(false);
        }
    }
    
    public void OnSlotClicked()
    {
        if (_currentSlot == null) return;
        OnItemSlotClicked?.Invoke(_currentSlot);
    }
}