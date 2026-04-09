using UnityEngine;
using System.Collections.Generic;
using TheLastHeir.Runtime.Entity;

namespace TheLastHeir.Runtime.Shop
{
    [System.Serializable]
    public class ShopItemEntry
    {
        public Item item;
        public int price;
    }

    [CreateAssetMenu(fileName = "NewShopData", menuName = "Shop/Shop Data")]
    public class ShopData : ScriptableObject
    {
        public string shopName = "김덕배";
        public List<ShopItemEntry> itemsForSale;
    }
}