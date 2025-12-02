using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TypingGame : Minigame
{
    struct GameDifficult
    {
        public GameDifficult(int word, float time)
        {
            this.word = word;
            this.time = time;
        }

        public int word;               // 글자 개수
        public float time;
    }

    GameDifficult[] dif;

    [SerializeField]
    int maxWord;            // 최대 나올 수 있는 단어
    [SerializeField]    
    GameObject image;       // 복제할 이미지 GameObject
    
    [SerializeField]
    Text timeText;
    float curTime;
    float worldInterval;    // 글자간 간격
    float startX;

    [SerializeField]
    RectTransform cur;
    RectTransform targetWordPos;

    Image[] wordImages;
    Sprite[] words;
    string[] task;          // 주어지는 단어

    int wordCount;          // 현재 눌러야할 단어위치
    
    
    void Awake()
    {
        words = Resources.LoadAll<Sprite>("Image/Typing");
        wordImages = new Image[maxWord];
        for (int i = 0; i < maxWord; i++) 
        {
            wordImages[i] = Instantiate(image, transform).GetComponent<Image>();
        }
        task = new string[maxWord];
        worldInterval = 70.0f;
        startX = -250.0f;
        wordCount = 0;
        targetWordPos = null;
        cur.SetAsLastSibling();
        dif = new GameDifficult[3];
        dif[0] = new GameDifficult(4, 10);
        dif[1] = new GameDifficult(6, 9);
        dif[2] = new GameDifficult(8, 8);

        gameObject.SetActive(false);
    }

    public override void Init(MoveController con, MinigameObject obj)
    {
        base.Init(con, obj);
        curTime = dif[difficult].time;
        targetWordPos = wordImages[wordCount].GetComponent<RectTransform>();
        cur.anchoredPosition = targetWordPos.anchoredPosition;
        for (int i = 0; i < wordImages.Length; i++)
        {
            wordImages[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < dif[difficult].word; i++)
        {
            int temp = Random.Range(0, maxWord);
            wordImages[i].sprite = words[temp];
            wordImages[i].rectTransform.anchoredPosition = new Vector3(startX + i * worldInterval, 0.0f, 0.0f);
            wordImages[i].gameObject.SetActive(true);
            task[i] = wordImages[i].sprite.name;
        }
        cur.gameObject.SetActive(true);
        timeText.gameObject.SetActive(true);
    }

    void Update()
    {
        if(!isStart || Time.timeScale == 0.0f)
        {
            return;
        }

        if(Input.anyKeyDown && !Input.GetKeyDown(KeyCode.Escape))        // 무슨 키가 눌렸을 때
        {
            if (Input.inputString.ToUpper().Equals(task[wordCount]))   // 맞았을 때
            {
                NextWord();
            }
            else                                             // 틀렸을 때
            {
                Retry();
            }                   
        }

        Vector2 vel = Vector2.zero;
        cur.anchoredPosition = Vector2.SmoothDamp(cur.anchoredPosition, new Vector2(targetWordPos.anchoredPosition.x, cur.anchoredPosition.y), ref vel, 0.01f);

        TimeCheck();
    }

    void TimeCheck()
    {
        curTime -= Time.deltaTime;
        if(curTime <= 0.0f)
        {
            curTime = 0.0f;
            Fail();
        }

        timeText.text = "시간 : " + Mathf.Round(curTime);
    }

    public override void ResetGame()
    {
        if(isDraw)
        {
            return;
        }

        base.ResetGame();
        timeText.gameObject.SetActive(false);
        wordCount = 0;
    }

    void Retry()
    {
        for (int i = 0; i < wordCount; i++)
        {
            wordImages[i].gameObject.SetActive(true);
        }
        wordCount = 0;
        targetWordPos = wordImages[wordCount].GetComponent<RectTransform>();
    }

    void NextWord()
    {
        wordImages[wordCount].gameObject.SetActive(false);
        wordCount++;
        if (wordCount >= dif[difficult].word)
        {
            Clear();
        }
        else
        {
            targetWordPos = wordImages[wordCount].GetComponent<RectTransform>();
        }
    }

    public override void Fail()
    {
        base.Fail();
        resultText[1].SetActive(true);
        cur.gameObject.SetActive(false);
        GameStop();
        StartCoroutine(StopTimer());
    }


    public override void Clear()
    {
        base.Clear();
        resultText[0].SetActive(true);
        cur.gameObject.SetActive(false);
        GameStop();
        StartCoroutine(StopTimer());
    }
}
