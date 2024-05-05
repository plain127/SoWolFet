using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ChatBoxSizeFitLeft : MonoBehaviour
{
    public RectTransform LeftBoxSize;

    void Start()
    {
        LeftBoxFit(LeftBoxSize);

    }
    void LeftBoxFit(RectTransform BoxSize)
    {
        if (BoxSize.sizeDelta.x >= 80)
        {
            BoxSize.sizeDelta = new Vector2(80, BoxSize.sizeDelta.y);
        }

        //LayoutRebuilder.ForceRebuildLayoutImmediate(BoxSize);
    }

}
