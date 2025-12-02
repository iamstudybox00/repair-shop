using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigamePlayer : MonoBehaviour
{
    bool isCatchTarget;
    RectTransform rect;

    public void Init(float x = 0.0f, float y = 0.0f)
    {
        isCatchTarget = false;
        rect = GetComponent<RectTransform>();
        rect.anchoredPosition = new Vector2(x, y);
    }

    public bool GetCatchTarget()
    {
        return isCatchTarget;
    }

    public RectTransform GetRect()
    {
        return rect;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag.Equals("Target"))
        {
            isCatchTarget = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag.Equals("Target"))
        {
            isCatchTarget = false;
        }
    }
}
