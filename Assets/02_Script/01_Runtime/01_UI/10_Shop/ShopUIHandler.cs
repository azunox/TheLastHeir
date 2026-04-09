using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using TheLastHeir.Runtime.Entity;
using TheLastHeir.Runtime.Shop;

namespace TheLastHeir.Runtime.UI
{
    public class ShopUIHandler : MonoBehaviour
    {
        public static ShopUIHandler Instance { get; private set; }

        [SerializeField] private GameObject shopPanel;
        [SerializeField] private TextMeshProUGUI shopTitleText;
        [SerializeField] private Transform listContent;
        [SerializeField] private GameObject shopSlotPrefab;

        [Header("Detail Area")]
        [SerializeField] private GameObject detailPanel;
        [SerializeField] private Image detailIcon;
        [SerializeField] private TextMeshProUGUI detailName;
        [SerializeField] private TextMeshProUGUI detailDesc;
        [SerializeField] private TextMeshProUGUI detailPrice;
        [SerializeField] private Button buyButton;

        [SerializeField] private PlayerAttributeHandler playerAttributes; 
        [SerializeField] private PlayerInventoryHandler playerInventory;

        private ShopItemEntry _selectedEntry;

        private void Awake()
        {
            if (Instance == null) Instance = this; else Destroy(gameObject);
            shopPanel.SetActive(false);
            detailPanel.SetActive(false);
        }

        public void OpenShop(ShopData shopData)
        {
            shopPanel.SetActive(true);
            shopTitleText.text = shopData.shopName;
            detailPanel.SetActive(false);
            _selectedEntry = null;

            foreach (Transform child in listContent) Destroy(child.gameObject);

            foreach (var entry in shopData.itemsForSale)
            {
                GameObject slotObj = Instantiate(shopSlotPrefab, listContent);
                ShopSlotUI slotUI = slotObj.GetComponent<ShopSlotUI>();
                if (slotUI != null)
                {
                    slotUI.Setup(entry);
                    slotUI.OnSlotClicked += HandleSlotSelected;
                }
            }
        }

        public void CloseShop() => shopPanel.SetActive(false);

        private void HandleSlotSelected(ShopItemEntry entry)
        {
            _selectedEntry = entry;
            detailPanel.SetActive(true);

            detailIcon.sprite = entry.item.icon;
            detailName.text = entry.item.itemName;
            detailDesc.text = entry.item.description;
            detailPrice.text = $"{entry.price}";

            buyButton.onClick.RemoveAllListeners();
            buyButton.onClick.AddListener(TryBuyItem);
        }

        private void TryBuyItem()
        {
            if (_selectedEntry == null) return;

            if (playerAttributes.Amso >= _selectedEntry.price)
            {
                playerAttributes.Amso -= _selectedEntry.price;
                playerInventory.AddItem(_selectedEntry.item);
                Debug.Log($"구매 성공: {_selectedEntry.item.itemName}");
            }
            else
            {
                Debug.Log("돈 부족");
            }
        }
    }
}