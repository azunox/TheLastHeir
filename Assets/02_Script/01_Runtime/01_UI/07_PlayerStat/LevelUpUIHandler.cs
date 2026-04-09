using UnityEngine;
using TMPro;
using TheLastHeir.Runtime.Entity;
using TheLastHeir.Runtime.Enums;

namespace TheLastHeir.Runtime.UI
{
    public class LevelUpUIHandler : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private PlayerAttributeHandler attributeHandler;
        [SerializeField] private GameObject uiPanel; 

        [Header("Top Info")]
        [SerializeField] private TextMeshProUGUI levelText;
        [SerializeField] private TextMeshProUGUI amsoText; 
        [SerializeField] private TextMeshProUGUI costText;
        [SerializeField] private TextMeshProUGUI willRemainingAmsoText;
        [SerializeField] private TextMeshProUGUI pointsText;

        [Header("Stats Values")]
        [SerializeField] private TextMeshProUGUI strengthText;
        [SerializeField] private TextMeshProUGUI magicText;
        [SerializeField] private TextMeshProUGUI defenseText;
        [SerializeField] private TextMeshProUGUI healthText;
        [SerializeField] private TextMeshProUGUI staminaText;
        [SerializeField] private TextMeshProUGUI mpText;

        private void OnEnable()
        {
            UpdateUI();
        }
        
        public void OnClickLevelUp()
        {
            if (attributeHandler == null) return;
            if (attributeHandler.TryLevelUp())
            {
                UpdateUI();
            }
        }

        public void OnClickIncreaseStat(int statTypeIndex)
        {
            StatType type = (StatType)statTypeIndex;
            AttemptIncreaseStat(type);
        }
        
        public void OnClickStrength() => AttemptIncreaseStat(StatType.Strength);
        public void OnClickMagic() => AttemptIncreaseStat(StatType.Magic);
        public void OnClickDefense() => AttemptIncreaseStat(StatType.Defense);
        public void OnClickHealth() => AttemptIncreaseStat(StatType.Health);
        public void OnClickStamina() => AttemptIncreaseStat(StatType.Stamina);
        public void OnClickMp() => AttemptIncreaseStat(StatType.Mp);

        private void AttemptIncreaseStat(StatType type)
        {
            if (attributeHandler == null) return;

            if (attributeHandler.TryIncreaseStat(type))
            {
                UpdateUI();
            }
        }

        public void UpdateUI()
        {
            if (attributeHandler == null) return;

            levelText.text = $"{attributeHandler.Level}";
            amsoText.text = $"{attributeHandler.Amso}";
            costText.text = $"{attributeHandler.NextLevelCost}";
            willRemainingAmsoText.text = $"{attributeHandler.Amso - attributeHandler.NextLevelCost}";
            if(pointsText != null) 
                pointsText.text = $"{attributeHandler.StatPoints}";
            
            
            strengthText.text = $"{attributeHandler.Strength}";
            magicText.text = $"{attributeHandler.Magic}";
            defenseText.text = $"{attributeHandler.Defense}";
            healthText.text = $"{attributeHandler.Health}";
            staminaText.text = $"{attributeHandler.StaminaStat}";
            mpText.text = $"{attributeHandler.MpStat}";
        }

        public void OpenUI()
        {
            uiPanel.SetActive(true);
            UpdateUI(); 
        }

        public void CloseUI()
        {
            uiPanel.SetActive(false);
        }
    }
}