using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorGameTarget : MonoBehaviour
{
    Color color;
    public void Init(Vector2 position)
    {
        GetComponent<RectTransform>().anchoredPosition = position;
        color = new Color(Mathf.Round(Random.Range(0.0f, 1.0f)), Mathf.Round(Random.Range(0.0f, 1.0f)), Mathf.Round(Random.Range(0.0f, 1.0f)), 1.0f);
        GetComponent<Image>().color = color;
        gameObject.SetActive(true);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag.Equals("Bullet"))
        {
            if (collision.gameObject.GetComponent<Image>().color == color)  // 같은 색
            {
                collision.gameObject.SetActive(false);
                gameObject.SetActive(false);
            }
            else    // 다른 색
            {
                collision.gameObject.SetActive(false);
            }
        }
    }
}
