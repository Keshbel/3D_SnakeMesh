using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResetButton : MonoBehaviour
{
    public Button button;

    private void Awake()
    {
        button.onClick.AddListener(ResetGame);
    }

    private void ResetGame()
    {
        SceneManager.LoadSceneAsync(0);
    }
}
