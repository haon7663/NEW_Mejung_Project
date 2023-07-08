using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class KeySetGroup : MonoBehaviour
{
    private RectTransform m_RectTransform;

    public Color m_NormalColor;
    public Color m_TouchColor;

    public int m_KeyCount = 0;
    public float posY;

    public Text[] textBundle = new Text[8];
    private void Start()
    {
        m_RectTransform = GetComponent<RectTransform>();
        for(int i = 0; i < 8; i++)
        {
            textBundle[i] = transform.GetChild(i).GetComponent<Text>();
        }
        SetColor();
    }
    private void Update()
    {
        if(!KeyManager.instance.isKeySetting)
        {
            if ((Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) && m_KeyCount < 7)
            {
                m_KeyCount += 1;
                SetColor();
            }
            else if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && m_KeyCount > 0)
            {
                m_KeyCount -= 1;
                SetColor();
            }
        }

        posY = m_KeyCount * 160 - 560;
        if (posY < -240) posY = -239.99f;
        if (posY > 240) posY = 240;
        m_RectTransform.anchoredPosition = Vector3.Lerp(m_RectTransform.anchoredPosition, new Vector3(-345, posY), 0.2f);

        if(Input.GetKeyDown(KeyCode.Return))
        {
            KeyManager.instance.ChangeKey(m_KeyCount);
        }
    }

    private void SetColor()
    {
        for (int i = 0; i < 8; i++)
        {
            Debug.Log(m_KeyCount == i);
            textBundle[i].color = m_KeyCount == i ? m_TouchColor : m_NormalColor;
        }
    }
    public void SetPosY(int pos)
    {
        m_KeyCount = pos;
        SetColor();
    }
}
