using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleMessage : MonoBehaviour
{
    [SerializeField]
    Image msg;

    bool toggle;

    public void Toggle()
    {
        toggle = !toggle;
        msg.gameObject.SetActive(toggle);
    }
}
