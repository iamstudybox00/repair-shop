using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score
{
    Text data;
    static Score instance = null;
    
    int curScore;
    int[] gameScore;

    Score()
    {
        Init();
        curScore = 0;
        gameScore = new int[5];
        gameScore[0] = 200;
        gameScore[1] = 200;
        gameScore[2] = 200;
        gameScore[3] = 300;
        gameScore[4] = 200;
    }

    void Init()
    {
        GameObject temp = Resources.Load("Prefabs/Score") as GameObject;
        temp.transform.position = new Vector3(0.0f, 0.0f, 0.0f);
        temp = Object.Instantiate(temp);
        data = temp.GetComponentInChildren<Text>();
        data.text = "점수 : " + curScore;
    }

    public void Increase(int gameType, int gameDif)
    {
        curScore += gameScore[gameType] * (gameDif + 1);
        data.text = "점수 : " + curScore;
    }

    public void Decrease(int gameType, int gameDif)
    {
        curScore -= gameScore[gameType] * (gameDif + 1);
        data.text = "점수 : " + curScore;
    }

    public void Decrease(int score)
    {
        curScore -= score;
        data.text = "점수 : " + curScore;
    }

    public int GetScore()
    {
        return curScore;
    }

    public static Score Get()
    {
        if (instance == null)
        {
            instance = new Score();
        }

        return instance;
    }

    public void Destroy()
    {
        instance = null;
    }
}
