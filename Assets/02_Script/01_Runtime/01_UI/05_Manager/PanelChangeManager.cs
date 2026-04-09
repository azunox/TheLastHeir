using UnityEngine;
using System;
public class PanelChangeManager : MonoBehaviour
{
    [Header("패널 변경 설정")] 
    [SerializeField] private GameObject currentPanel;
    [SerializeField] private GameObject changePanel;
    
    public void OnPanelChangeClicked()
    {  
        // 현재 활성화된 패널을 비활성화
        if (currentPanel != null)
        {
            currentPanel.SetActive(false);
        }
        
        // 변경할 패널을 활성화
        if (changePanel != null)
        {
            changePanel.SetActive(true);
        }
        else
        {
            Debug.LogWarning($"Change panel '{changePanel}' not found.");
        }
    }
}
