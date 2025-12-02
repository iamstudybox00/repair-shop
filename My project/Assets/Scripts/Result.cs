using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Result : MonoBehaviour
{
    Text text;
    int maxScore, curScore;

    [SerializeField]
    GameObject background;

    Coroutine resultCor;

    void Awake()
    {
        maxScore = 0;
        curScore = 0;
        text = GetComponent<Text>();
        text.text = "점수 : " + curScore;
    }

    public void ShowResult(int score)
    {
        background.SetActive(true);
        maxScore = score;
        StartCoroutine(IncreaseScore(10));
    }

    IEnumerator IncreaseScore(int scoreUnit)
    {
        while (true)
        {
            yield return new WaitForSeconds(0.001f);
            curScore += scoreUnit;
            if (curScore >= maxScore)
            {
                text.text = "점수 : " + maxScore;
                StopCoroutine(IncreaseScore(50));
                break;
            }
            text.text = "점수 : " + curScore;
        }
    }

    public void Retry()
    {
        background.SetActive(false);
        curScore = 0;
        maxScore = 0;
    }

    public void Title()
    {
        MinigameManager.Get().Destroy();
        Score.Get().Destroy();
        SceneManager.LoadScene("StartScene");
    }
}
