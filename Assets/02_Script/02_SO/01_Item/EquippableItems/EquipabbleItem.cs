using UnityEngine;

public enum EquipmentSlot 
{ 
    Helmet, 
    Armor, 
    Legs, 
    Boots, 
    Weapon,
    Potion 
}

[CreateAssetMenu(fileName = "New Equippable", menuName = "Inventory/Equippable Item")]
public class EquippableItem : Item
{
    [Header("Equipment Info")]
    public EquipmentSlot equipmentSlot;

    [Header("Stat Bonuses")]
    public int strengthBonus;
    public int magicBonus;
    public int defenseBonus;
    public int healthBonus;
    public int staminaBonus;
    public int mpBonus;
}