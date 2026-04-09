using System;
using System.Collections.Generic;

namespace TheLastHeir.Runtime.Network
{
    [Serializable]
    public class PlayerDataResponse
    {
        public LocationData location;
        public StatusData status;
        public AttributeData attributes;
        public int[] progress;
        public List<InventoryItemData> inventory;
        public EquipmentData equipment;
        public string[] quickSlots;
    }

    [Serializable]
    public class LocationData
    {
        public string sceneName;
        public int campfireNumber;
    }

    [Serializable]
    public class StatusData
    {
        public int maxHp;
        public float maxStamina;
        public float maxMp;
        public int amso;
        public int level;
    }

    [Serializable]
    public class AttributeData
    {
        public int strength;
        public int magic;
        public int defense;
        public int health;
        public int stamina;
        public int mp;
    }

    [Serializable]
    public class InventoryItemData
    {
        public string itemId;
        public int quantity;
        public int enhancement;
    }

    [Serializable]
    public class EquipmentData
    {
        public string helmet;
        public string armor;
        public string legs;
        public string boots;
        public string weapon;
    }
}