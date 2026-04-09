using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DamageIndicatorManager : MonoBehaviour
{
    public static DamageIndicatorManager Instance { get; private set; }

    [Header("Indicator Settings")]
    [SerializeField] private int poolSize = 20;
    [SerializeField] private float floatSpeed = 50f;
    [SerializeField] private float fadeSpeed = 1f;
    [SerializeField] private float lifeTime = 1.0f;
    [SerializeField] private float stackOffset = 30f;
    [SerializeField] private Vector2 randomSpread = new Vector2(50f, 0f);

    [Header("Color Settings")]
    [SerializeField] private Color playerDamageColor = Color.white;
    [SerializeField] private Color enemyDamageColor = Color.yellow;
    [SerializeField] private Color criticalDamageColor = Color.red;
    [SerializeField] private Color healColor = Color.green;

    private Canvas _canvas;
    private GameObject _indicatorPrefab;
    private Transform _poolParent;
    
    private Queue<DamageIndicator> indicatorPool = new Queue<DamageIndicator>();
    private List<DamageIndicator> activeIndicators = new List<DamageIndicator>();

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        InitializeSystem();
        InitializePool();
    }
    
    private void InitializeSystem()
    {
        _canvas = FindObjectOfType<Canvas>();
        if (_canvas == null)
        {
            GameObject canvasGO = new GameObject("DamageIndicatorCanvas");
            _canvas = canvasGO.AddComponent<Canvas>();
            _canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            _canvas.sortingOrder = 100;

            CanvasScaler canvasScaler = canvasGO.AddComponent<CanvasScaler>();
            canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            canvasScaler.referenceResolution = new Vector2(1920, 1080);

            canvasGO.AddComponent<GraphicRaycaster>();

            DontDestroyOnLoad(canvasGO);
        }

        if (FindObjectOfType<EventSystem>() == null)
        {
            GameObject eventSystemGO = new GameObject("EventSystem");
            eventSystemGO.AddComponent<EventSystem>();
            eventSystemGO.AddComponent<StandaloneInputModule>();

            DontDestroyOnLoad(eventSystemGO);
        }
        
        _indicatorPrefab = new GameObject("DamageIndicator_Prefab_Auto", typeof(RectTransform));
        _indicatorPrefab.transform.SetParent(_canvas.transform, false);

        RectTransform rt = _indicatorPrefab.GetComponent<RectTransform>();
        rt.anchorMin = new Vector2(0.5f, 0.5f);
        rt.anchorMax = new Vector2(0.5f, 0.5f);
        rt.pivot = new Vector2(0f, 0f);
        rt.sizeDelta = new Vector2(200, 50);
        
        TextMeshProUGUI tmpText = _indicatorPrefab.AddComponent<TextMeshProUGUI>();
        tmpText.alignment = TextAlignmentOptions.Center;
        tmpText.fontSize = 24;
        tmpText.color = Color.white;
        tmpText.font = TMP_Settings.defaultFontAsset;
        
        DamageIndicator indicatorScript = _indicatorPrefab.AddComponent<DamageIndicator>();
        indicatorScript.damageText = tmpText;

        _indicatorPrefab.SetActive(false);

        _poolParent = new GameObject("DamageIndicatorPool").transform;
        _poolParent.SetParent(_canvas.transform, false);
        DontDestroyOnLoad(_poolParent.gameObject);
    }

    private void InitializePool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject indicatorGO = Instantiate(_indicatorPrefab, _poolParent);
            indicatorPool.Enqueue(indicatorGO.GetComponent<DamageIndicator>());
        }
    }
    
    public void ShowDamage(int damage, Vector3 worldPosition, bool targetIsPlayer, bool isCritical = false)
    {
        StackActiveIndicators();
        DamageIndicator indicator = GetIndicatorFromPool();
        if (indicator == null) return;
        
        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPosition);
        RectTransform canvasRect = _canvas.GetComponent<RectTransform>();
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPos, Camera.main, out localPoint);
        
        indicator.transform.SetParent(canvasRect, false);
        indicator.transform.localPosition = localPoint;
        indicator.transform.localPosition += new Vector3(
            Random.Range(-randomSpread.x / 2, randomSpread.x / 2),
            Random.Range(-randomSpread.y / 2, randomSpread.y / 2), 0f);

        Color displayColor;
        if (damage < 0)
        {
            displayColor = healColor;
            damage = Mathf.Abs(damage);
        }
        else
        {
            displayColor = isCritical ? criticalDamageColor : (targetIsPlayer ? playerDamageColor : enemyDamageColor);
        }
        
        indicator.SetDamage(damage, displayColor, isCritical);
        activeIndicators.Add(indicator);
        StartCoroutine(AnimateDamageIndicator(indicator, displayColor));
    }
    private void StackActiveIndicators()
    {
        for (int i = activeIndicators.Count - 1; i >= 0; i--)
        {
            if (activeIndicators[i] == null || !activeIndicators[i].IsActiveAndAnimating)
            {
                activeIndicators.RemoveAt(i);
                continue;
            }
            activeIndicators[i].transform.localPosition += Vector3.up * stackOffset;
        }
    }
    
    private IEnumerator AnimateDamageIndicator(DamageIndicator indicator, Color startColor)
    {
        float timer = lifeTime;
        Color currentColor = startColor;
        
        RectTransform rt = indicator.GetComponent<RectTransform>();

        while (timer > 0)
        {
            if (indicator == null) yield break;

            rt.localPosition += Vector3.up * (floatSpeed * Time.deltaTime);
            
            timer -= Time.deltaTime;
            float alpha = Mathf.Clamp01(timer / lifeTime);
            
            currentColor.a = alpha;
            indicator.damageText.color = currentColor;

            yield return null;
        }
        ReturnIndicatorToPool(indicator);
    }

    private DamageIndicator GetIndicatorFromPool()
    {
        if (indicatorPool.Count > 0)
        {
            return indicatorPool.Dequeue();
        }
        else
        {
            GameObject indicatorGO = Instantiate(_indicatorPrefab, _poolParent);
            return indicatorGO.GetComponent<DamageIndicator>();
        }
    }

    private void ReturnIndicatorToPool(DamageIndicator indicator)
    {
        if (indicator == null) return;
        
        indicator.ResetIndicator();
        activeIndicators.Remove(indicator);
        indicator.transform.SetParent(_poolParent, false);
        indicatorPool.Enqueue(indicator);
    }
}
