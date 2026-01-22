using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadManager : MonoBehaviour
{
    public void LoadScemeGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void LoadFinishScene()
    {
        SceneManager.LoadScene("FinishScene");
    }

    public void LoadMenuScene()
    {
        SceneManager.LoadScene("MenuScene");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
