using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameObject : MonoBehaviour
{
    protected int diffcult;
    protected int gameType;

    public virtual void Clear()     // 미니게임을 성공했을 때 실행
    {

    }

    public virtual void Fail()      // 실패했을때
    {

    }

    public int GetDif()
    {
        return diffcult;
    }

    public int GetGameType()
    {
        return gameType;
    }
}
