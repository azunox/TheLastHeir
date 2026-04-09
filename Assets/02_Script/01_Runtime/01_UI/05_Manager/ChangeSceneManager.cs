using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ChangeSceneManager : MonoBehaviour
{
    public void OnChangedScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
