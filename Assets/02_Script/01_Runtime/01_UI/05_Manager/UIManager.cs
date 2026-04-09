using System;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("All UIPanels")]
    [SerializeField] private List<MonoBehaviour> panelScripts = new List<MonoBehaviour>();

    private bool isTransitioning = false;
    
    private List<IUIPanel> panels = new List<IUIPanel>();
    private Stack<IUIPanel> panelStack = new Stack<IUIPanel>(); // Stack 기능 추가 -- 뒤로가기

    private void Awake()
    {

        foreach (var panel in panelScripts)
        {
            if (panel is IUIPanel uiPanel)
            {
                panels.Add(uiPanel);
            }
            else
            {
                Debug.LogWarning($"{panel.name} does not implement IUIPanel.");
            }
        }
    }

    
    /// 지정된 패털 표시, 다른 패널 비활성화
    public void ShowPanel<T>(bool force = false) where T : IUIPanel
    {
        if (isTransitioning)
        {
            return; // 전환 중이면 무시
        }


        // 현재 보여야 할 패널 찾기
        IUIPanel targetPanel = null;
        foreach(var panel in panels)
        {
            if (panel is T)
            {
                targetPanel = panel;
                break;
            }
        }

        if (targetPanel == null)
        {
            Debug.LogWarning($"[UIManager] 패널 {typeof(T).Name}을 찾을 수 없음");
            return;
        }

        if (!force && panelStack.Count > 0 && panelStack.Peek() == targetPanel)
        {
            var fade = ((MonoBehaviour)targetPanel).GetComponent<UIFadeController>();
            if (fade != null) fade.FadeIn();
            else
            {
                targetPanel.Show();
            }
            return;
        }

        isTransitioning = true;

        // 이전 패널 FadeOut
        if (panelStack.Count > 0)
        {
            var currentPanel = panelStack.Peek();
            var currentFade = ((MonoBehaviour)currentPanel).GetComponent<UIFadeController>();

            Action afterFadeOut = () =>
            {
                panelStack.Pop();
                panelStack.Push(targetPanel);

                var targetGO = ((MonoBehaviour)targetPanel).gameObject;
                
                // Fade In 오류 발생 -> 먼저 활성화
                if (!targetGO.activeSelf)
                {
                    targetGO.SetActive(true);
                }  
                
                // 활성화 후 컴포넌트 가져옴
                var targetFade = targetGO.GetComponent<UIFadeController>();
                
                // 불필요한 조건문 삭제
                if (targetFade != null)
                {
                    // FadeIn 호출, 콜백에서 플래그 해제
                    targetFade.FadeIn(() =>
                    {
                        isTransitioning = false;
                    });
                }
                else
                {
                    // FadeController가 없으면 Show()로 대체
                    Debug.LogWarning("[UIManager] UIFadeController 없음 → Show()로 대체");
                    targetPanel.Show();
                    isTransitioning = false;
                }
            };

            if (currentFade != null)
                currentFade.FadeOut(afterFadeOut);
            else
            {
                currentPanel.Hide();
                afterFadeOut();
            }
        }
        else
        {
            panelStack.Push(targetPanel);
            var targetGO = ((MonoBehaviour)targetPanel).gameObject;
            targetGO.SetActive(true);

            var targetFade = targetGO.GetComponent<UIFadeController>();
            
            if (targetFade != null)
            {
                targetFade.FadeIn(() =>
                {
                    isTransitioning = false; // 애니메이션 끝나면 플래그 해제
                });
            }
            else
            {
                targetPanel.Show();
                isTransitioning = false;
            }

        }
    }


    
    /// 모든 패널 비활성화
    public void HideAllPanels()
    {
        foreach (var panel in panels)
        {
            var fade = ((MonoBehaviour)panel).GetComponent<UIFadeController>();
            if (fade != null) fade.FadeOut();
            else panel.Hide();
        }
    }

    
    public void Back()
    {
        if (isTransitioning)
            return;

        if (panelStack.Count == 0)
            return;

        isTransitioning = true;

        var current = panelStack.Pop();
        var currentFade = ((MonoBehaviour)current).GetComponent<UIFadeController>();

        Action showPrevious = () =>
        {
            if (panelStack.Count == 0)
            {
                isTransitioning = false;
                return;
            }

            var previous = panelStack.Peek();
            var previousFade = ((MonoBehaviour)previous).GetComponent<UIFadeController>();

            if (previousFade != null)
                previousFade.FadeIn(() => { isTransitioning = false; });
            else
            {
                previous.Show();
                isTransitioning = false;
            }
        };

        if (currentFade != null)
            currentFade.FadeOut(showPrevious);
        else
        {
            current.Hide();
            showPrevious();
        }
    }



}