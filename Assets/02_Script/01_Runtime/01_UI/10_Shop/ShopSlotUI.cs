using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using TheLastHeir.Runtime.Shop;

namespace TheLastHeir.Runtime.UI
{
    public class ShopSlotUI : MonoBehaviour
    {
        [SerializeField] private Image iconImage;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI priceText;
        [SerializeField] private Button button;

        private ShopItemEntry _currentEntry;
        public event Action<ShopItemEntry> OnSlotClicked;

        public void Setup(ShopItemEntry entry)
        {
            _currentEntry = entry;
            if (entry.item != null)
            {
                iconImage.sprite = entry.item.icon;
                nameText.text = entry.item.itemName;
                priceText.text = $"{entry.price}";
            }
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => OnSlotClicked?.Invoke(_currentEntry));
        }
    }
}