using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

namespace TheLastHeir.Runtime.UI
{
    public class CutsceneUI : MonoBehaviour
    {
        public static CutsceneUI Instance { get; private set; }

        [Header("UI")]
        [SerializeField] private GameObject cutscenePanel;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI subtitleText;
        [SerializeField] private CanvasGroup subtitleGroup;

        [Header("Effects")] 
        [Tooltip("컷신 페이트 인/아웃")]
        [SerializeField] private Image fadeOverlay;
        
        [SerializeField] private float typingSpeed = 0.05f;

        private Coroutine _typingCoroutine;
        
        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
            
            if (cutscenePanel != null) cutscenePanel.SetActive(false);
            
            if (fadeOverlay != null)
            {
                fadeOverlay.gameObject.SetActive(true);
                fadeOverlay.color = new Color(0, 0, 0, 0);
            }
        }

        public void EnterCutsceneMode()
        {
            // 컷신 진입시 UI 초기화
            cutscenePanel.SetActive(true);
            subtitleText.text = "";
            nameText.text = "";
            if (subtitleGroup != null) subtitleGroup.alpha = 0;
            if (fadeOverlay != null) fadeOverlay.color = new Color(0, 0, 0, 0);
        }

        public void ExitCutsceneMode()
        {
            if (fadeOverlay != null) fadeOverlay.color = new Color(0, 0, 0, 0);
            cutscenePanel.SetActive(false);
        }

        public void ShowSubtitle(string name, string text)
        {
            nameText.text = name;
            subtitleText.text = text;
            
            if (subtitleGroup != null)
                subtitleGroup.alpha = 1;
            
            if (_typingCoroutine != null)
                StopCoroutine(_typingCoroutine);
            
            _typingCoroutine = StartCoroutine(TypeSentence(text));
        }

        public IEnumerator FadeScreen(float targetAlpha, float duration)
        {
            if (fadeOverlay == null) yield break;

            float startAlpha = fadeOverlay.color.a;
            float timer = 0f;

            while (timer < duration)
            {
                timer += Time.deltaTime;
                float t = timer / duration;
                float alpha = Mathf.Lerp(startAlpha, targetAlpha, t);
                fadeOverlay.color = new Color(0, 0, 0, alpha);
                yield return null;
            }

            fadeOverlay.color = new Color(0, 0, 0, targetAlpha);
        }
        
        public void HideSubtitle()
        {
            if (subtitleGroup != null) subtitleGroup.alpha = 0;
            nameText.text = "";
            subtitleText.text = "";
        }

        private IEnumerator TypeSentence(string sentence)
        {
            // 자막 타이핑 효과
            subtitleText.text = "";
            
            foreach (char letter in sentence.ToCharArray())
            {
                if (letter == ' ')
                {
                    subtitleText.text += letter;
                    yield return new WaitForSeconds(typingSpeed * 2);
                }
                else
                {
                    subtitleText.text += letter;
                    yield return new WaitForSeconds(typingSpeed);
                }
                
                
            }
        }
    }
}