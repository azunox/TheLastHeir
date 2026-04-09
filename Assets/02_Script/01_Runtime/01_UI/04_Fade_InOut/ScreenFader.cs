using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class ScreenFader : MonoBehaviour
{
    [Header("UI References")]
    public Image fadeImage;
    public Sprite[] loadingSprites; 
    public Image loadingAnimImage;
    public TMP_Text loadingText;
    public TMP_Text ChapterText;

    [Header("Fade Settings")]
    public float fadeDuration;
    public float blackScreenDuration;
    public float spriteChangeInterval;

    [Header("Fade Options")] 
    public bool SceneFade;
    
    public IEnumerator PlayLoadingSpriteAnim()
    {
        loadingText.gameObject.SetActive(true);
        int index = 0;
        while (true)
        {
            loadingAnimImage.sprite = loadingSprites[index];
            index = (index + 1) % loadingSprites.Length;
            yield return new WaitForSeconds(spriteChangeInterval);
        }
    }

    public IEnumerator FadeOut()
    {
        fadeImage.gameObject.SetActive(true);
        float elapsed = 0f;
        Color c = fadeImage.color;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / fadeDuration);
            c.a = Mathf.SmoothStep(0f, 1f, t);
            fadeImage.color = c;
            yield return null;
        }
        c.a = 1f;
        fadeImage.color = c;
    }

    public IEnumerator FadeIn()
    {
        loadingText.gameObject.SetActive(false);
        float elapsed = 0f;
        Color c = fadeImage.color;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / fadeDuration);
            c.a = Mathf.SmoothStep(1f, 0f, t);
            fadeImage.color = c;
            yield return null;
        }
        c.a = 0f;
        fadeImage.color = c;
        fadeImage.gameObject.SetActive(false);
    }
}