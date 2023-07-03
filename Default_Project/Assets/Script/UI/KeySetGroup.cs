using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeySetGroup : MonoBehaviour
{
    private RectTransform m_RectTransform;

    public int m_KeyCount = 0;
    private float posY;
    private void Start()
    {
        m_RectTransform = GetComponent<RectTransform>();
    }
    private void Update()
    {
        posY = m_KeyCount * 160 - 560;
        if (posY < -240) posY = -240;
        if (posY > 240) posY = 240;
        m_RectTransform.anchoredPosition = Vector3.Lerp(m_RectTransform.anchoredPosition, new Vector3(-345, posY), Time.deltaTime*5);
    }
}
