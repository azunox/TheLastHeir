using UnityEngine;

namespace TheLastHeir.Runtime.Entity
{
    [CreateAssetMenu(fileName = "Player Stats", menuName = "Player Stats/Player Stats")]
    public class PlayerStats : ScriptableObject
    {
        [Header("Components")]
        public LayerMask GroundMask;
        public float GroundCheckRadius = 0.2f;

        [Header("Movement Stats")]
        public float WalkSpeed = 3f;
        public float SprintSpeed = 6f;
        public float RotationSmoothTime = 0.1f;
        public float JumpHeight = 1.5f;
        public float Gravity = -9.81f;
        public float FallMultiplier = 2.5f;
        public float RollSpeed = 6f;
        public float RollDuration = 0.5f;
        public float RollInvincibilityDuration = 0.4f; 
        
        [Header("Stamina Stats")]
        public float MaxStamina = 100f;
        public float StaminaRegenRate = 15f;    
        public float StaminaRegenDelay = 1.5f;  
        public float JumpStaminaCost = 15f;
        public float RollStaminaCost = 20f;
        public float AttackStaminaCost = 25f;   
        public float SprintStaminaCost = 10f;

        [Header("Leveling Settings")]
        public int BaseLevelUpCost = 100;
        public float CostMultiplier = 1.2f;
        public int StatPointsPerLevel = 1;
        
        [Header("Stat Growth Modifiers")]
        public int HpPerHealthPoint = 10;
        public float StaminaPerStaminaPoint = 2f;
        public float MpPerMpPoint = 5f;
        
        [Header("Cooldowns")]
        public float RollCooldown = 1.5f;
    }   
}