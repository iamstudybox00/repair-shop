using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimingGame : Minigame
{
    struct GameDifficult
    { 
        public GameDifficult(int speed, float targetSize, float targetPadding)
        {
            this.speed = speed;
            this.targetSize = targetSize;
            this.targetPadding = targetPadding;
        }

        public int speed;              // 바가 움직이는 속도
        public float targetSize;
        public float targetPadding;    // 시작점으로부터 타겟이 떨어져있어야하는 최소 거리
    }

    GameDifficult[] dif;

    [SerializeField]
    RectTransform bar;
    [SerializeField]
    MinigamePlayer player;
    Rigidbody2D playerMove;
    [SerializeField]
    RectTransform target;

    float rangeX;
    float targetRangeX;

    void Awake()
    {
        dif = new GameDifficult[3];
        dif[0] = new GameDifficult(180, 80, -bar.rect.width * 0.5f + bar.rect.width * 0.6f);
        dif[1] = new GameDifficult(180, 60, -bar.rect.width * 0.5f + bar.rect.width * 0.4f);
        dif[2] = new GameDifficult(180, 40, -bar.rect.width * 0.5f + bar.rect.width * 0.2f);

        playerMove = player.GetComponent<Rigidbody2D>();

        gameObject.SetActive(false);
    }

    public override void Init(MoveController con, MinigameObject obj)
    {
        base.Init(con, obj);
        player.Init(0.0f, 0.0f);

        target.sizeDelta = new Vector2(dif[difficult].targetSize, target.sizeDelta.y);
        target.GetComponent<BoxCollider2D>().size = new Vector2(target.sizeDelta.x, target.sizeDelta.y);

        rangeX = bar.rect.width * 0.5f - player.GetRect().rect.width * 0.5f;
        targetRangeX = bar.rect.width * 0.5f - target.sizeDelta.x * 0.5f;
        player.GetRect().anchoredPosition = new Vector2(-rangeX, 0.0f);

        float targetX = Random.Range(dif[difficult].targetPadding, targetRangeX);  // 타이밍 타켓을 둘 위치를 랜덤생성
        target.anchoredPosition = new Vector2(targetX, 0.0f);
    }

    public override void ResetGame()
    {
        if(isDraw)
        {
            return;
        }

        base.ResetGame();
    }

    void Update()
    {
        if (isStart)
        {
            if (Input.GetKeyDown(KeyCode.Space))        // 눌렀을 때 타이밍 체크
            {
                Check();
            }
            else
            {
                MoveBar();
            }

        }
    }

    void FixedUpdate()
    {
        if (isStart)
        {
            playerMove.MovePosition(new Vector2(playerMove.position.x + dif[difficult].speed * Time.smoothDeltaTime * 5, playerMove.position.y));
        }
    }

    void MoveBar()      // 타이밍 바 움직임
    {
        if(player.GetRect().anchoredPosition.x >= rangeX)
        {
           player.GetRect().anchoredPosition = new Vector2(-rangeX, player.GetRect().anchoredPosition.y);
        }
    }

    public void Check()
    {
        if(player.GetCatchTarget())
        {
            Clear();
            resultText[0].SetActive(true);
        }
        else
        {
            resultText[1].SetActive(true);
            Fail();
        }
        GameStop();
        StartCoroutine(StopTimer());
    }

}
