using UnityEngine;
using UnityEngine.UI;
using TheLastHeir.Runtime.Entity;

namespace TheLastHeir.Runtime.UI
{
    public class HealthUIHandler : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private PlayerAttributeHandler attributeHandler;
        [SerializeField] private Image healthFillImage;
        [SerializeField] private GameObject healthBarParent;

        [Header("Settings")]
        [SerializeField] private float fadeSpeed = 2f;
        [SerializeField] private float showDuration = 2f;

        private float _hideTimer;
        private CanvasGroup _canvasGroup;

        private void Awake()
        {
            _canvasGroup = healthBarParent.GetComponent<CanvasGroup>();
            if (_canvasGroup == null) _canvasGroup = healthBarParent.AddComponent<CanvasGroup>();
        }

        private void Update()
        {
            if (attributeHandler == null || healthFillImage == null) return;

            UpdateStaminaBar();
            HandleVisibility();
        }

        private void UpdateStaminaBar()
        {
            float current = attributeHandler.CurHp;
            float max = attributeHandler.MaxHp;

            float fillAmount = current / max;
            healthFillImage.fillAmount = fillAmount;

            if (fillAmount < 0.99f)
            {
                _hideTimer = showDuration;
            }
        }

        private void HandleVisibility()
        {
            if (_hideTimer > 0)
            {
                _hideTimer -= Time.deltaTime;
                _canvasGroup.alpha = Mathf.MoveTowards(_canvasGroup.alpha, 1f, fadeSpeed * Time.deltaTime);
            }
            else
            {
                _canvasGroup.alpha = Mathf.MoveTowards(_canvasGroup.alpha, 0f, fadeSpeed * Time.deltaTime);
            }
        }
    }
}