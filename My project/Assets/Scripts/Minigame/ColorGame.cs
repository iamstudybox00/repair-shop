using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorGame : Minigame
{
    struct GameDifficult
    {
        public GameDifficult(int count, float time)
        {
            this.count = count;
            this.time = time;
        }

        public int count;
        public float time;
    }

    GameDifficult[] dif;

    [SerializeField]
    Text timeText;
    float curTime;

    [SerializeField]
    GameObject targetOrigin;
    ColorGameTarget[] targets;
    int curTarget;

    [SerializeField]
    GameObject bulletOrigin;
    BulletManager bm;

    [SerializeField]
    ColorGamePlayer player;
    void Awake()
    {
        dif = new GameDifficult[3];
        dif[0] = new GameDifficult(1, 15);
        dif[1] = new GameDifficult(2, 15);
        dif[2] = new GameDifficult(3, 15);

        targets = new ColorGameTarget[dif[dif.Length - 1].count];
        for (int i = 0; i < targets.Length; i++)
        {
            GameObject temp = Object.Instantiate(targetOrigin, transform);
            targets[i] = temp.GetComponent<ColorGameTarget>();
        }
        gameObject.SetActive(false);
        bm = new BulletManager(5, bulletOrigin, transform);
        player.SetComponent();
    }

    public override void Init(MoveController con, MinigameObject obj)
    {
        base.Init(con, obj);
        player.Init();
        curTime = dif[difficult].time;
        timeText.gameObject.SetActive(true);
        for (int i = 0; i < targets.Length; i++)            // 이전에 있던 타겟 초기화
        {
            targets[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < dif[difficult].count; i++) 
        {
            targets[i].Init(new Vector2(0, 100 * i));
        }
        curTarget = 0;
    }

    void Update()
    {
        if (!isStart)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            bm.Shoot(player.GetRect().anchoredPosition, new Vector2(0.0f, 1.0f), player.GetColor());
            player.ResetColor();
        }

        TargetCheck();
        TimeCheck();
    }

    void TargetCheck()
    {
        if (!targets[curTarget].gameObject.activeSelf)
        {
            if (curTarget >= dif[difficult].count - 1)
            {
                Clear();
            }
            else
            {
                curTarget++;
            }
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
        bm.ResetBullet();
        timeText.gameObject.SetActive(false);
    }

    public override void Fail()
    {
        base.Fail();
        resultText[1].SetActive(true);
        bm.ResetBullet();
        GameStop();
        StartCoroutine(StopTimer());
    }


    public override void Clear()
    {
        base.Clear();
        resultText[0].SetActive(true);
        bm.ResetBullet();
        GameStop();
        StartCoroutine(StopTimer());
    }
}