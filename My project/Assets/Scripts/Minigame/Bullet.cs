using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bullet : MonoBehaviour
{
    Vector2 dir;
    float speed;
    RectTransform rect;

    public void Init(Vector2 position, Vector2 dir, Color color, float speed)
    {
        GetComponent<Image>().color = color;
        rect = GetComponent<RectTransform>();
        rect.anchoredPosition = position;
        this.dir = dir.normalized;
        this.speed = speed;
    }

    void Update()
    {
        rect.anchoredPosition = rect.anchoredPosition + dir * speed * Time.deltaTime * 10;
    }
}
