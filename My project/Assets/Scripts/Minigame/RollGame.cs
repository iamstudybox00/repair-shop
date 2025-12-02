using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RollGame : Minigame
{
    struct GameDifficult
    {
        public GameDifficult(float maxHit, float time, float lose)
        {
            this.maxHit = maxHit;
            this.time = time;
            this.lose = lose;
        }

        public float maxHit;            // 키를 눌렀을 때 채워지는 비율 ex)20일 경우 1/20만큼 게이지가 오름
        public float time;              // 바가 움직이는 속도
        public float lose;              // 키를 계속 누르고 있지 않을 때 떨어지는 게이지 비율 Time delta
    }

    GameDifficult[] dif;
    float curTime;
    [SerializeField]
    Slider gauge;
    [SerializeField]
    Text timeText;

    void Awake()
    {
        dif = new GameDifficult[3];
        dif[0] = new GameDifficult(5, 5, 15);
        dif[1] = new GameDifficult(10, 5, 10);
        dif[2] = new GameDifficult(10, 5, 5);

        gameObject.SetActive(false);
    }

    public override void Init(MoveController con, MinigameObject obj)
    {
        base.Init(con, obj);
        curTime = dif[difficult].time;
        timeText.gameObject.SetActive(true);
        gauge.value = 0.0f;
    }

    void Update()
    {
        if (!isStart)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            float data = 1 / dif[2].maxHit;
            gauge.value += data;
            GaugeCheck();
        }
        else
        {
            gauge.value -= Time.deltaTime / dif[2].lose;
        }
        
        TimeCheck();
    }

    void GaugeCheck()
    {
        if(gauge.value >= 1.0f)
        {
            Clear();
        }
    }

    void TimeCheck()
    {
        curTime -= Time.deltaTime;
        if (curTime <= 0.0f)
        {
            curTime = 0.0f;
            Fail();
        }

        timeText.text = "시간 : " + Mathf.Round(curTime);
    }

    public override void ResetGame()
    {
        if (isDraw)
        {
            return;
        }

        base.ResetGame();
    }

    public override void Fail()
    {
        base.Fail();
        resultText[1].SetActive(true);
        GameStop();
        StartCoroutine(StopTimer());
    }


    public override void Clear()
    {
        base.Clear();
        resultText[0].SetActive(true);
        GameStop();
        StartCoroutine(StopTimer());
    }
}
