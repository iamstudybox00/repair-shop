using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HoldGame : Minigame
{
    struct GameDifficult
    {
        public GameDifficult(float gaugeSpeed, int time, float loseSpeed, float targetMoveInterval)
        {
            this.gaugeSpeed = gaugeSpeed;
            this.time = time;
            this.loseSpeed = loseSpeed;
            this.targetMoveInterval = targetMoveInterval;
        }

        public float gaugeSpeed;            // 영역에 들어와 있을때 게이지 상승 속도
        public float time;                  // 제한 시간
        public float loseSpeed;             // 영역에 들어와 있지 않을 때 게이지 하락 속도
        public float targetMoveInterval;
    }

    GameDifficult[] dif;
    float curTime;
    [SerializeField]
    Slider gauge;
    [SerializeField]
    Text timeText;

    [SerializeField]
    MinigamePlayer player;
    Rigidbody2D playerMove;
    [SerializeField]
    RectTransform playerTarget;
    [SerializeField]
    RectTransform bar;

    float targetRange;        // 갈 수 있는 최대 x값 최소 : - 최대 : +
    float targetMovePos;
    float curTargetMoveX;     // 현재 타겟이 이동해야할 x값
    float MoveSpeed;

    Coroutine cor;
    void Awake()
    {
        dif = new GameDifficult[3];
        dif[0] = new GameDifficult(0.5f, 15, 0.05f, 1.0f);
        dif[1] = new GameDifficult(0.3f, 15, 0.1f, 0.8f);
        dif[2] = new GameDifficult(0.2f, 15, 0.1f, 0.6f);

        gameObject.SetActive(false);
        playerMove = player.GetComponent<Rigidbody2D>();
        MoveSpeed = 100.0f;

        targetRange = bar.rect.width * 0.5f - playerTarget.rect.width * 0.5f;
        targetMovePos = 100;
    }

    public override void Init(MoveController con, MinigameObject obj)
    {
        base.Init(con, obj);
        curTime = dif[difficult].time;
        timeText.gameObject.SetActive(true);
        player.Init(-400.0f, -500.0f);
        gauge.value = 0.0f;
        playerTarget.anchoredPosition = new Vector2(0.0f, -500.0f);
        curTargetMoveX = 0.0f;
    }

    void Update()
    {
        if (!isStart)
        {
            return;
        }

        if (player.GetCatchTarget())
        {
            gauge.value += dif[difficult].gaugeSpeed * Time.deltaTime;
            GaugeCheck();
        }
        else
        {
            gauge.value -= dif[difficult].loseSpeed * Time.deltaTime;
        }

        Vector2 vel = Vector2.zero;
        playerTarget.anchoredPosition = Vector2.SmoothDamp(playerTarget.anchoredPosition, new Vector2(curTargetMoveX, playerTarget.anchoredPosition.y), ref vel, 0.05f);
        TimeCheck();
    }

    void FixedUpdate()
    {
        if (isStart)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                playerMove.MovePosition(new Vector2(playerMove.position.x + (MoveSpeed + 200) * Time.deltaTime, playerMove.position.y));
            }
            else
            {
                playerMove.MovePosition(new Vector2(playerMove.position.x - (MoveSpeed + 200) * Time.deltaTime, playerMove.position.y));
            }
        }
    }

    IEnumerator TargetMove()
    {
        yield return new WaitForSeconds(dif[difficult].targetMoveInterval);
        if (Random.Range(0, 2) > 0)
        {
            float temp = curTargetMoveX - targetMovePos;
            if(temp < -targetRange)
            {
                temp = -targetRange;
            }

            curTargetMoveX = temp;
        }
        else
        {
            float temp = curTargetMoveX + targetMovePos;
            if (temp > targetRange)
            {
                temp = targetRange;
            }

            curTargetMoveX = temp;
        }
        
        StartCoroutine(TargetMove());
    }

    void GaugeCheck()
    {
        if (gauge.value >= 1.0f)
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

    public override void GameStart()
    {
        base.GameStart();
        cor = StartCoroutine(TargetMove());
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
        StopCoroutine(cor);
        StartCoroutine(StopTimer());
    }


    public override void Clear()
    {
        base.Clear();
        resultText[0].SetActive(true);
        GameStop();
        StopCoroutine(cor);
        StartCoroutine(StopTimer());
    }
}
