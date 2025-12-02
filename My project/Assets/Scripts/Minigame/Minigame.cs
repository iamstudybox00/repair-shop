using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minigame : MonoBehaviour
{
    protected bool isStart;
    protected bool isDraw;
    protected int difficult;
    MinigameObject minigameTarget;

    MoveController con;         // 미니게임중 키 입력이 다른 곳에 영향가지 않기 위해

    [SerializeField]
    Particle particle;
    AudioSource clearSound;

    [SerializeField]
    protected GameObject[] resultText;// 성공 실패 텍스트 0 : 성공 1 : 실패

    public virtual void Init(MoveController con, MinigameObject obj)
    {
        if(isDraw)
        {
            return;
        }    

        difficult = obj.GetDif();
        isStart = false;
        ResetGame();
        minigameTarget = obj;
        this.con = con;
    }

    public virtual void ResetGame()
    {
        minigameTarget = null;
        resultText[0].SetActive(false);
        resultText[1].SetActive(false);
        particle.gameObject.SetActive(false);

    }

    public virtual void GameStart()
    {
        if(isDraw)
        {
            return;
        }

        isStart = true;
        isDraw = true;
        gameObject.SetActive(true);
        con.Pause();
    }
    public void GameStop()
    {
        isStart = false;
    }
    public virtual void Clear()
    {
        if(minigameTarget)
        {
            particle.gameObject.SetActive(true);
            particle.StartParticle(minigameTarget.transform.position);
            isStart = false;
            clearSound.Play();
            minigameTarget.Clear();
        }
    }
    public virtual void Fail()
    {
        if(minigameTarget)
        {
            minigameTarget.Fail();
            isStart = false;
        }
    }

    public virtual IEnumerator StopTimer()
    {
        yield return new WaitForSeconds(0.5f);

        con.Resume();
        gameObject.SetActive(false);
        isDraw = false;
        clearSound.Stop();
        StopCoroutine(StopTimer());
    }

    public void StopGame()
    {
        con.Resume();
        ResetGame();
        gameObject.SetActive(false);
        isDraw = false;
    }

    public void SetClearSound(AudioSource a)
    {
        clearSound = a;
    }
}
