using UnityEngine;
using TMPro;

public class DamageIndicator : MonoBehaviour
{
    public TextMeshProUGUI damageText;
    public bool IsActiveAndAnimating { get; private set; }

    public void SetDamage(int damage, Color color, bool isCritical = false)
    {
        damageText.text = damage.ToString();
        damageText.color = color;
        
        if (isCritical)
        {
            // 크리티컬일때 폰트
            damageText.fontSize = 30; 
            damageText.fontStyle = FontStyles.Bold;
        }
        else
        {
            // 그냥
            damageText.fontSize = 24;
            damageText.fontStyle = FontStyles.Normal;
        }
        
        IsActiveAndAnimating = true;
        gameObject.SetActive(true);
    }

    public void ResetIndicator()
    {
        IsActiveAndAnimating = false;
        gameObject.SetActive(false);
    }
}