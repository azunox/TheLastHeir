using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemNoti : MonoBehaviour
{
    [Header("Item Notification Settings")]
    [SerializeField] private Vector2 startPos;
    [SerializeField] private Vector2 finalPos;
    [SerializeField] private float moveDuration = 0.3f;
    [SerializeField] private float displayDuration = 3f; 
    [SerializeField] private float fadeDuration = 0.5f;
    
    [Header("UI References")]
    public TextMeshProUGUI itemNameDisplay;
    public Image itemImage;
    
    [Header("Effects References")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip EffectClip;

    private RectTransform rectTransform;
    private Coroutine fadeOutCoroutine;
    private Coroutine delayCoroutine;

    void Awake() 
    {
        rectTransform = GetComponent<RectTransform>();
    }
    
    public void GetItemNoti(Sprite img, string name)
    {
        itemImage.sprite = img;
        itemNameDisplay.text = name;

        rectTransform.anchoredPosition = startPos;

        if (fadeOutCoroutine != null)
        {
            StopCoroutine(fadeOutCoroutine);
            fadeOutCoroutine = null;
        }
        if (delayCoroutine != null)
        {
            StopCoroutine(delayCoroutine);
            delayCoroutine = null;
        }

        Color imgColor = itemImage.color;
        Color textColor = itemNameDisplay.color;
        imgColor.a = 1f;
        textColor.a = 1f;
        itemImage.color = imgColor;
        itemNameDisplay.color = textColor;

        StartCoroutine(MoveUI(rectTransform, finalPos, moveDuration));
    }

    private IEnumerator MoveUI(RectTransform rt, Vector2 targetPos, float duration)
    {
        Vector2 start = rt.anchoredPosition;
        audioSource.PlayOneShot(EffectClip);
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            rt.anchoredPosition = Vector2.Lerp(start, targetPos, elapsed / duration);
            yield return null;
        }

        rt.anchoredPosition = targetPos;
        delayCoroutine = StartCoroutine(DelayFadeOut());
    }

    private IEnumerator DelayFadeOut()
    {
        yield return new WaitForSeconds(displayDuration);
        fadeOutCoroutine = StartCoroutine(FadeOutUI(fadeDuration));
    }
    
    private IEnumerator FadeOutUI(float duration)
    {
        float elapsed = 0f;

        Color imgColor = itemImage.color;
        Color textColor = itemNameDisplay.color;

        float startImgAlpha = imgColor.a;
        float startTextAlpha = textColor.a;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            imgColor.a = Mathf.Lerp(startImgAlpha, 0f, t);
            textColor.a = Mathf.Lerp(startTextAlpha, 0f, t);

            itemImage.color = imgColor;
            itemNameDisplay.color = textColor;

            yield return null;
        }

        imgColor.a = 0f;
        textColor.a = 0f;
        itemImage.color = imgColor;
        itemNameDisplay.color = textColor;

        rectTransform.anchoredPosition = startPos;

        imgColor.a = 1f;
        textColor.a = 1f;
        itemImage.color = imgColor;
        itemNameDisplay.color = textColor;
    }
}