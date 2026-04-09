using System;
using UnityEngine;

public class SettingResolution : MonoBehaviour
{
    private void Start()
    {
        Screen.SetResolution(2560, 1440, Screen.fullScreen);
    }
}