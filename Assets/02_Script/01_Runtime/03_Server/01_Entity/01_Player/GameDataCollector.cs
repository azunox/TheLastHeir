using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using TheLastHeir.Runtime.Network;
using TheLastHeir.Runtime.Entity;

// Player data 수집
public class GameDataCollector : MonoBehaviour
{
    public static GameDataCollector Instance { get; private set; }

    [Header("References")]
    [SerializeField] private Player player;
    [SerializeField] private PlayerInventoryHandler inventoryHandler;
    [SerializeField] private PlayerAttributeHandler attributeHandler;

    private void Awake()
    {
        Instance = this;
    }

    public PlayerDataResponse CollectCurrentData()
    {
        if (player == null) player = FindObjectOfType<Player>();
        if (player != null)
        {
            if (inventoryHandler == null) inventoryHandler = player.GetComponent<PlayerInventoryHandler>();
            if (attributeHandler == null) attributeHandler = player.GetComponent<PlayerAttributeHandler>();
        }

        PlayerDataResponse data = new PlayerDataResponse();
        
        data.location = new LocationData();
        data.location.sceneName = SceneManager.GetActiveScene().name;
        data.location.campfireNumber = -1;
        
        data.status = new StatusData();
        if (attributeHandler != null)
        {
            data.status.maxHp = attributeHandler.MaxHp;
            data.status.amso = attributeHandler.Amso;
            
            data.status.maxStamina = 100f; 
            data.status.maxMp = 50f;       
            data.status.level = 1;         
        }
        
        data.attributes = new AttributeData();
        data.attributes.strength = 0;
        data.attributes.magic = 0;
        data.attributes.defense = 0;
        data.attributes.health = 0;
        data.attributes.stamina = 0;
        data.attributes.mp = 0;
        
        /*
        data.inventory = new List<InventoryItemData>();
        if (inventoryHandler != null && inventoryHandler.inventoryItems != null)
        {
            foreach (var item in inventoryHandler.inventoryItems)
            {
                if (item == null) continue;

                InventoryItemData itemData = new InventoryItemData();
                itemData.itemId = item.itemName; 
                itemData.quantity = 1; 
                itemData.enhancement = 0; 

                data.inventory.Add(itemData);
            }
        }
        */
        
        data.equipment = new EquipmentData();
        if (inventoryHandler != null && inventoryHandler.currentEquipment != null)
        {
            data.equipment.helmet = GetEquippedItemName(EquipmentSlot.Helmet);
            data.equipment.armor = GetEquippedItemName(EquipmentSlot.Armor);
            data.equipment.legs = GetEquippedItemName(EquipmentSlot.Legs);
            data.equipment.boots = GetEquippedItemName(EquipmentSlot.Boots);
            data.equipment.weapon = GetEquippedItemName(EquipmentSlot.Weapon);
        }
        
        data.progress = new int[0];
        data.quickSlots = new string[0];

        return data;
    }
    
    private string GetEquippedItemName(EquipmentSlot slot)
    {
        int index = (int)slot;
        
        if (inventoryHandler.currentEquipment != null && 
            index >= 0 && 
            index < inventoryHandler.currentEquipment.Length)
        {
            var item = inventoryHandler.currentEquipment[index];
            return item != null ? item.itemName : "";
        }
        return "";
    }
}