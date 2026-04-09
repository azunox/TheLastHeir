using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using TheLastHeir.Runtime.World;

namespace TheLastHeir.Runtime.UI
{
    public class DeathUI : MonoBehaviour
    {
        public static DeathUI Instance { get; private set; }

        [Header("UI References")]
        [SerializeField] private GameObject deathPanel;
        [SerializeField] private Image backgroundBar;
        [SerializeField] private Image backgroundBar2;
        [SerializeField] private Image mainText;
        [SerializeField] private Image ghostText;
        [SerializeField] private Image fadeOverlay;

        [Header("Settings")]
        [SerializeField] private float animationSpeed = 1.0f;
        [SerializeField] private float animationDuration = 3.0f;
        [SerializeField] private float autoRespawnDelay = 5.0f;
        [SerializeField] private float displayDuration = 3.0f; 
        [SerializeField] private AudioClip deathSound;
        [SerializeField] private float fadeDuration = 1.5f;
        
        private AudioSource _audioSource;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
            
            _audioSource = GetComponent<AudioSource>();
            if (_audioSource == null) _audioSource = gameObject.AddComponent<AudioSource>();

            if (deathPanel != null) deathPanel.SetActive(false);
        }

        public void ShowDeathUI()
        {
            if (deathPanel == null) return;
            
            deathPanel.SetActive(true);
            StartCoroutine(PlayDeathSequence());
        }

        private IEnumerator PlayDeathSequence()
        {
            fadeOverlay.gameObject.SetActive(true);
            
            Color barColor = backgroundBar.color;
            barColor.a = 0;
            backgroundBar.color = barColor;
            
            Color barColor2 = backgroundBar2.color;
            barColor2.a = 0;
            backgroundBar2.color = barColor2;
            
            Color ghostColor = ghostText.color;
            ghostColor.a = 0;
            ghostText.color = ghostColor;
            ghostText.transform.localScale = Vector3.one;
            
            if (deathSound != null) _audioSource.PlayOneShot(deathSound);

            float timer = 0f;
            bool isRespawning = false;
            
            while (timer < animationDuration)
            {
                timer += Time.deltaTime * animationSpeed;
                
                if (timer < 0.5f)
                {
                    float barProgress = timer / 0.5f;
                    
                    Color c = backgroundBar.color;
                    c.a = Mathf.Lerp(0, 0.9f, barProgress);
                    backgroundBar.color = c;
                    
                    backgroundBar2.color = c;
                }
                
                
                if (timer > 0.1f && timer < 1.6f)
                {
                    float ghostProgress = (timer - 0.1f) / 1.5f;
                    Color c = ghostText.color;
                    c.a = Mathf.Lerp(0.5f, 0f, ghostProgress);
                    ghostText.color = c;
                    
                    float ghostScaleX = Mathf.Lerp(1f, 1.2f, ghostProgress);
                    ghostText.transform.localScale = new Vector3(ghostScaleX, 1f, 1f);
                }

                yield return null;
            }
            
            yield return new WaitForSeconds(displayDuration);
            
            float fadeTimer = 0f;
            while (fadeTimer < fadeDuration)
            {
                fadeTimer += Time.deltaTime;
                if (fadeOverlay != null)
                {
                    float alpha = Mathf.Clamp01(fadeTimer / fadeDuration);
                    fadeOverlay.color = new Color(0, 0, 0, alpha);
                }
                yield return null;
            }
            
            yield return new WaitForSeconds(0.5f);
            isRespawning = true;
            RestartGame();
            
        }

        private void RestartGame()
        {
            if (SpawnManager.Instance != null)
            {
                SpawnManager.Instance.RespawnPlayerAtLastPoint();
                
                StartCoroutine(PlayRespawnFadeIn());
            }
            else
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
        
        private IEnumerator PlayRespawnFadeIn()
        {
            yield return new WaitForSeconds(0.5f);

            float timer = 0f;
            
            if (deathPanel != null) 
            {
                deathPanel.SetActive(false);
            }
            
            while (timer < fadeDuration)
            {
                timer += Time.deltaTime;
                if (fadeOverlay != null)
                {
                    float alpha = Mathf.Lerp(1f, 0f, timer / fadeDuration);
                    fadeOverlay.color = new Color(0, 0, 0, alpha);
                }
                yield return null;
            }
            
            if (fadeOverlay != null)
            {
                fadeOverlay.color = new Color(0, 0, 0, 0);
                fadeOverlay.gameObject.SetActive(false);
            }
        }
    }
}