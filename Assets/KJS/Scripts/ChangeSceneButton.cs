using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChangeSceneButton : MonoBehaviour
{
    public Button yourButton;
    public string sceneName; // 이동할 씬의 이름

    void Start()
    {
        yourButton.onClick.AddListener(ChangeScene);
    }

    void ChangeScene()
    {
        SceneManager.LoadScene(sceneName);
    }
}
