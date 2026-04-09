using UnityEngine;
using TheLastHeir.Runtime.Structs;
using TheLastHeir.Runtime.Enums;
using Unity.VisualScripting;

namespace TheLastHeir.Runtime.Entity
{
    public class PlayerAttributeHandler : StandardAttributeEntity
    {
        [Header("References")]
        [SerializeField] private PlayerStats stats;
        [SerializeField] private Player _player;

        [Header("Growth Stats (Base)")]
        [SerializeField] private int _level = 1;
        [SerializeField] private int _statPoints = 0;

        [SerializeField] private int _strength = 10;
        [SerializeField] private int _magic = 10;
        [SerializeField] private int _defense = 10;
        [SerializeField] private int _health = 10;
        [SerializeField] private int _stamina = 10;
        [SerializeField] private int _mp = 10;

        private int _equipStrength;
        private int _equipMagic;
        private int _equipDefense;
        private int _equipHealth;
        private int _equipStamina;
        private int _equipMp;

        [Header("Resources")]
        [SerializeField] private int _maxHp;
        [SerializeField] private int _curHp;
        [SerializeField] private float _maxStamina;
        [SerializeField] private float _currentStamina;
        [SerializeField] private float _maxMp;
        [SerializeField] private float _currentMp;
        
        public int Amso = 0; 

        [Header("Defenses & Type")]
        [SerializeField] private DamageNegation _damageNegation;
        [SerializeField] private ElementType _type;
        [SerializeField] private StatusResistances _statusResistances;

        private float _staminaRegenTimer;

        public int Level => _level;
        public int StatPoints => _statPoints;

        public int Strength => _strength + _equipStrength;
        public int Magic => _magic + _equipMagic;
        public int Defense => _defense + _equipDefense;
        public int Health => _health + _equipHealth;
        public int StaminaStat => _stamina + _equipStamina;
        public int MpStat => _mp + _equipMp;

        public float CurrentStamina => _currentStamina;
        public float MaxStamina => _maxStamina;
        public float MaxMp => _maxMp;

        public int NextLevelCost => Mathf.RoundToInt(stats.BaseLevelUpCost * Mathf.Pow(stats.CostMultiplier, _level - 1));

        protected override void Awake()
        {
            base.Awake();
            RecalculateStats();
            
            _curHp = _maxHp;
            _currentStamina = _maxStamina;
            _currentMp = _maxMp;
        }

        private void Update()
        {
            HandleStaminaRegen();
        }

        public void RecalculateStats()
        {
            if (stats == null) return;

            int baseMaxHp = 20;
            _maxHp = baseMaxHp + (Health * stats.HpPerHealthPoint);

            float baseMaxStamina = stats.MaxStamina;
            _maxStamina = baseMaxStamina + (StaminaStat * stats.StaminaPerStaminaPoint);

            float baseMaxMp = 50f;
            _maxMp = baseMaxMp + (MpStat * stats.MpPerMpPoint);

            if (_curHp > _maxHp) _curHp = _maxHp;
            if (_currentStamina > _maxStamina) _currentStamina = _maxStamina;
        }

        public void UpdateEquipmentBonuses(int str, int mag, int def, int vit, int sta, int mp)
        {
            _equipStrength = str;
            _equipMagic = mag;
            _equipDefense = def;
            _equipHealth = vit;
            _equipStamina = sta;
            _equipMp = mp;

            RecalculateStats();
        }

        public bool TryLevelUp()
        {
            int cost = NextLevelCost;

            if (Amso < cost)
            {
                Debug.Log("Amso(재화)가 부족합니다!");
                return false;
            }

            Amso -= cost;
            _level++;
            _statPoints += stats.StatPointsPerLevel; 
            
            return true;
        }

        public bool TryIncreaseStat(StatType targetStat)
        {
            if (_statPoints <= 0)
            {
                return false;
            }

            _statPoints--;

            switch (targetStat)
            {
                case StatType.Strength: _strength++; break;
                case StatType.Magic: _magic++; break;
                case StatType.Defense: _defense++; break;
                case StatType.Health: _health++; break;
                case StatType.Stamina: _stamina++; break;
                case StatType.Mp: _mp++; break;
            }

            RecalculateStats();
            return true;
        }

        private void HandleStaminaRegen()
        {
            if (stats == null) return;

            _staminaRegenTimer += Time.deltaTime;

            if (_currentStamina < _maxStamina && _staminaRegenTimer >= stats.StaminaRegenDelay)
            {
                _currentStamina += stats.StaminaRegenRate * Time.deltaTime;
                if (_currentStamina > _maxStamina) _currentStamina = _maxStamina;
            }
        }

        public bool UseStamina(float amount)
        {
            if (_currentStamina >= amount)
            {
                _currentStamina -= amount;
                _staminaRegenTimer = 0f;
                return true;
            }
            return false;
        }
        
        public void RestoreStamina(float amount)
        {
            _currentStamina += amount;
            if (_currentStamina > _maxStamina) _currentStamina = _maxStamina;
        }

        public override int MaxHp
        {
            get => _maxHp;
            set
            {
                _maxHp = Mathf.Max(1, value); 
                if (_curHp > _maxHp) _curHp = _maxHp;
            }
        }

        public override int CurHp
        {
            get => _curHp;
            set => _curHp = Mathf.Clamp(value, 0, _maxHp);
        }

        public override DamageNegation DamageNegation
        {
            get => _damageNegation;
            protected set => _damageNegation = value;
        }

        public override ElementType Type
        {
            get => _type;
            protected set => _type = value;
        }

        public override StatusResistances StatusResistances
        {
            get => _statusResistances;
            protected set => _statusResistances = value;
        }
        
        public override void TakeDamage(float amount)
        {
            if (_player != null && _player.IsInvincible)
            {
                Debug.Log("(I-Frame)");
                return;
            }
            
            CurHp -= (int)amount;
            _player.StateMachine.ChangeState(_player.HitState);
        }
        
        public void LoadData(int level, int amso, int points, int str, int mag, int def, int vit, int sta, int mp)
        {
            _level = level;
            Amso = amso;
            _statPoints = points;
            _strength = str;
            _magic = mag;
            _defense = def;
            _health = vit;
            _stamina = sta;
            _mp = mp;
            
            RecalculateStats();
        }
    }
}