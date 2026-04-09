using UnityEngine;
using UnityEngine.UI;

public class ResolutionSelector : MonoBehaviour
{
    public Dropdown resolutionDropdown; 

    public void OnResolutionChanged(int index)
    {
        string option = resolutionDropdown.options[index].text;
        
        string[] parts = option.Split('*');
        if (parts.Length != 2)
        {
            return;
        }

        // 왼쪽과 오른쪽 각각 숫자만 뽑기 위해 Trim하고, 괄호 부분 제거
        string widthStr = parts[0].Trim(); 
        string rightPart = parts[1].Trim(); 

        // 오른쪽 부분에서 숫자만 뽑기 (공백 기준으로 자르고 첫 번째만)
        string[] rightSplit = rightPart.Split(' ');
        string heightStr = rightSplit[0]; 

        // 숫자로 변환 시도
        if (int.TryParse(widthStr, out int width) && int.TryParse(heightStr, out int height))
        {
            Screen.SetResolution(width, height, Screen.fullScreen);
        }
        
    }
}