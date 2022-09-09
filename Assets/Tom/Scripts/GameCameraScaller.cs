using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCameraScaller : MonoBehaviour
{
    [SerializeField]
    private float m_widthToHightRatio;

    void Start()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, rectTransform.rect.height * m_widthToHightRatio);
    }
}
