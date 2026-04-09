using UnityEngine;
using UnityEngine.UI;
using TheLastHeir.Runtime.Entity;

namespace TheLastHeir.Runtime.UI
{
    public class StaminaUIHandler : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private PlayerAttributeHandler attributeHandler;
        [SerializeField] private Image staminaFillImage;
        [SerializeField] private GameObject staminaBarParent;

        [Header("Settings")]
        [SerializeField] private float fadeSpeed = 2f;
        [SerializeField] private float showDuration = 2f;

        private float _hideTimer;
        private CanvasGroup _canvasGroup;

        private void Awake()
        {
            _canvasGroup = staminaBarParent.GetComponent<CanvasGroup>();
            if (_canvasGroup == null) _canvasGroup = staminaBarParent.AddComponent<CanvasGroup>();
        }

        private void Update()
        {
            if (attributeHandler == null || staminaFillImage == null) return;

            UpdateStaminaBar();
            HandleVisibility();
        }

        private void UpdateStaminaBar()
        {
            float current = attributeHandler.CurrentStamina;
            float max = attributeHandler.MaxStamina;

            float fillAmount = current / max;
            staminaFillImage.fillAmount = fillAmount;

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