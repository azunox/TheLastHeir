using UnityEngine;

public class DontDestoryLoad : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
}
