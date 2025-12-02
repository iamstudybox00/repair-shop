using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorGamePlayer : MonoBehaviour
{
    Image image;
    RectTransform rect;

    public void Init()
    { 
        ResetColor();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            image.color += Color.red;
            ClampColor();
        }

        if(Input.GetKeyDown(KeyCode.W))
        {
            image.color += Color.green;
            ClampColor();
        }

        if(Input.GetKeyDown(KeyCode.E))
        {
            image.color += Color.blue;
            ClampColor();
        }
    }

    public void ClampColor()
    {
        Color c = new Color(Mathf.Clamp01(image.color.r), Mathf.Clamp01(image.color.g), Mathf.Clamp01(image.color.b), 1.0f);
        image.color = c;
    }

    public void SetComponent()
    {
        image = GetComponent<Image>();
        rect = GetComponent<RectTransform>();
    }

    public Color GetColor()
    {
        return image.color;
    }

    public void ResetColor()
    {
        image.color = Color.black;
    }

    public RectTransform GetRect()
    {
        return rect;
    }
}
