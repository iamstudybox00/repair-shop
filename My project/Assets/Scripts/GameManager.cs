using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    Text time;
    MoveController playerControl;
    bool playMinigame;              // 미니 게임 플레이중 멈춤
    ConsumerManager con;
    [SerializeField]
    float startTime;
    float curTime;

    [SerializeField]
    Result result;
    [SerializeField]
    GameObject[] resultBtn;
    [SerializeField]
    GameObject scoreText;
    [SerializeField]
    AudioSource bgm;

    GameObject pauseMenu;

    bool isGameStart;
    void Start()
    {
        playerControl = GameObject.Find("Player").GetComponent<MoveController>();
        con = GameObject.Find("ConsumerManager").GetComponent<ConsumerManager>();

        GameObject temp = Resources.Load("Prefabs/PauseMenu") as GameObject;   // Timing 미니게임 프리팹 생성
        temp.transform.position = new Vector3(0.0f, 0.0f, 0.0f);
        temp = Object.Instantiate(temp);
        pauseMenu = temp;
        pauseMenu.GetComponentInChildren<Button>().onClick.AddListener(delegate { ResetGame(); });
        Slider[] sliders = pauseMenu.GetComponentsInChildren<Slider>();
        SetVolume(sliders[0].value);
        sliders[0].onValueChanged.AddListener(delegate { SetVolume(sliders[0].value); });
        MinigameManager.Get().SetSoundVolume(sliders[0]);

        sliders[1].onValueChanged.AddListener(delegate { playerControl.SetMouseSensitivity(sliders[1].value); });
        playerControl.SetMouseSensitivity(sliders[1].value);

        resultBtn[0].GetComponent<Button>().onClick.AddListener(delegate { ResetGame(); });

        time = GetComponent<Text>();
        time.text = "남은 시간 : " + Mathf.Round(curTime);
        curTime = startTime;
        isGameStart = true;
        pauseMenu.SetActive(false);

        con.SommonStart();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (isGameStart)
            {
                Time.timeScale = 0.0f;
                pauseMenu.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                isGameStart = false;
                playMinigame = playerControl.GetIsPause();
                playerControl.Pause();
            }
            else
            {
                Time.timeScale = 1.0f;
                pauseMenu.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
                isGameStart = true;
                if (!playMinigame)
                {
                    playerControl.Resume();
                }
                playMinigame = false;
            }
        }

        if (isGameStart)
        {
            curTime -= Time.deltaTime;
            time.text = "남은 시간 : " + Mathf.Round(curTime);


            TimeCheck();
        }
    }

    void TimeCheck()
    {
        if (curTime <= 0.0f)
        {
            curTime = 0.0f;
            result.gameObject.SetActive(true);
            MinigameManager.Get().StopGame();
            playerControl.Pause();
            Cursor.lockState = CursorLockMode.None;
            isGameStart = false;
            scoreText.SetActive(false);
            for (int i = 0; i < resultBtn.Length; i++)
            {
                resultBtn[i].SetActive(true);
            }
            con.SommonStop();
            result.ShowResult(Score.Get().GetScore());
        }
    }

    public void SetVolume(float value)
    {
        bgm.volume = value;
    }

    public void ResetGame()
    {
        curTime = startTime;
        isGameStart = true;
        pauseMenu.SetActive(false);
        result.gameObject.SetActive(false);
        for (int i = 0; i < resultBtn.Length; i++)
        {
            resultBtn[i].SetActive(false);
        }
        scoreText.SetActive(true);
        result.Retry();
        Score.Get().Decrease(Score.Get().GetScore());
        MinigameManager.Get().StopGame();
        playerControl.ResetPlayer();
        con.ReloadConsumer();
        Time.timeScale = 1.0f;
    }
}
