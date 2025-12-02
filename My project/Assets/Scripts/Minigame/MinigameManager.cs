using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinigameManager
{
    Minigame[] gameData = new Minigame[5];

    static MinigameManager instance = null;
    AudioSource sound;

    MinigameManager()
    {
        Init();
    }

    void Init()
    {
        GameObject soundObj = Resources.Load("Prefabs/ClearSound") as GameObject;   // Timing 미니게임 프리팹 생성
        soundObj.transform.position = new Vector3(0.0f, 0.0f, 0.0f);
        soundObj = Object.Instantiate(soundObj);
        sound = soundObj.GetComponent<AudioSource>();

        GameObject temp = Resources.Load("Prefabs/TimingGame") as GameObject;   // Timing 미니게임 프리팹 생성
        temp.transform.position = new Vector3(0.0f, 0.0f, 0.0f);
        temp = Object.Instantiate(temp);
        temp.GetComponent<TimingGame>().SetClearSound(sound);
        gameData[0] = temp.GetComponent<TimingGame>();

        temp = Resources.Load("Prefabs/TypingGame") as GameObject;   // Timing 미니게임 프리팹 생성
        temp.transform.position = new Vector3(0.0f, 0.0f, 0.0f);
        temp = Object.Instantiate(temp);
        temp.GetComponent<TypingGame>().SetClearSound(sound);
        gameData[1] = temp.GetComponent<TypingGame>();

        temp = Resources.Load("Prefabs/RollGame") as GameObject;   // Roll 미니게임 프리팹 생성
        temp.transform.position = new Vector3(0.0f, 0.0f, 0.0f);
        temp = Object.Instantiate(temp);
        temp.GetComponent<RollGame>().SetClearSound(sound);
        gameData[2] = temp.GetComponent<RollGame>();

        temp = Resources.Load("Prefabs/HoldGame") as GameObject;   // Hold 미니게임 프리팹 생성
        temp.transform.position = new Vector3(0.0f, 0.0f, 0.0f);
        temp = Object.Instantiate(temp);
        temp.GetComponent<HoldGame>().SetClearSound(sound);
        gameData[3] = temp.GetComponent<HoldGame>();

        temp = Resources.Load("Prefabs/ColorGame") as GameObject;   // Color 미니게임 프리팹 생성
        temp.transform.position = new Vector3(0.0f, 0.0f, 0.0f);
        temp = Object.Instantiate(temp);
        temp.GetComponent<ColorGame>().SetClearSound(sound);
        gameData[4] = temp.GetComponent<ColorGame>();
    }

    public static MinigameManager Get()
    {
        if (instance == null)
        {
            instance = new MinigameManager();
        }

        return instance;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="gameType">0:Timing 1:Typing 2:Roll 3:Hold 4:Color</param>
    /// <param name="gameDif">0:Easy 1:Middle 2:Hard </param>
    public void GameStart(MoveController con, MinigameObject obj, int gameType = -1)        // 게임 종류와 난이도를 받음
    {
        if(gameType == -1)
        {
            gameType = Random.Range(0, gameData.Length);
        }

        gameData[gameType].Init(con, obj);
        gameData[gameType].GameStart();
    }

    public void StopGame()
    {
        foreach(Minigame i in gameData)
        {
            if (i && i.gameObject.activeInHierarchy)
            {
                i.StopGame();
            }
        }
    }

    public void SetSoundVolume(Slider slider)
    {
        SetVolume(slider.value);
        slider.onValueChanged.AddListener(delegate { SetVolume(slider.value); });
    }

    public void SetVolume(float value)
    {
        sound.volume = value;
    }

    public void Destroy()
    {
        instance = null;
    }
}
