using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonScript : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("GameScene");
        Time.timeScale = 1.0f;
    }

    public void Title()
    {
        SceneManager.LoadScene("StartScene");
        Time.timeScale = 1.0f;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void InGameRetry()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1.0f;
        MinigameManager.Get().Destroy();
        Score.Get().Destroy();
        SceneManager.LoadScene("GameScene");
    }

    public void InGameTitle()
    {
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 1.0f;
        MinigameManager.Get().Destroy();
        Score.Get().Destroy();
        SceneManager.LoadScene("StartScene");
    }
}
