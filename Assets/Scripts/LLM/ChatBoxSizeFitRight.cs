using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChatBoxSizeFitRight : MonoBehaviour
{
    public RectTransform rightBox;
    public RectTransform msg;

    void Update()
    {
        if (msg.sizeDelta.x > 0)
        {
            rightBox.sizeDelta = new Vector2(msg.sizeDelta.x, rightBox.sizeDelta.y);
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(rightBox);
    }

}
