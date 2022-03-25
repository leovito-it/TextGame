using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SizeFitter : MonoBehaviour
{
    public RectTransform[] objects;
    public RectTransform parent;
    public RectTransform[] modify;

    public ResizeType resizeType = ResizeType.Minus;

    Vector2 size = Vector2.zero;
    void Start()
    {
        Resize();
    }

    public void Resize()
    {
        size = parent.sizeDelta;
        foreach ( var obj in modify)
        {
            if (obj == null)
                continue;

            size = resizeType == ResizeType.Minus ? (size- obj.sizeDelta) : (size + obj.sizeDelta);
        }

        if (size.x < 1 || size.y < 1)
        {
            Debug.Log("No resizing available");
            return;
        }

        foreach (var obj in objects)
        {
            if (obj == parent || obj == null)
            {
                continue;
            }
            obj.sizeDelta = size;
        }
    }    

    public enum ResizeType { Add, Minus };
}
