using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeySetGroup : MonoBehaviour
{
    private RectTransform m_RectTransform;

    public Color m_NormalColor;
    public Color m_TouchColor;

    public int m_KeyCount = 0;
    public float posY;
    private void Start()
    {
        m_RectTransform = GetComponent<RectTransform>();
    }
    private void Update()
    {
        if ((Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) && m_KeyCount < 7) m_KeyCount += 1;
        else if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && m_KeyCount > 0) m_KeyCount -= 1;

        posY = m_KeyCount * 160 - 560;
        if (posY < -240) posY = -239.99f;
        if (posY > 240) posY = 240;
        m_RectTransform.anchoredPosition = Vector3.Lerp(m_RectTransform.anchoredPosition, new Vector3(-345, posY), 0.2f);
    }
    public void SetPosY(int pos)
    {
        m_KeyCount = pos;
    }
}
