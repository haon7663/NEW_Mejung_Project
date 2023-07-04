using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlusTutorial : MonoBehaviour
{
    private Image m_Image;
    public int m_Lenth;

    private Camera m_Camera;
    private GameObject m_Player;

    private float m_KeyPos;

    private void Start()
    {
        m_Image = GetComponent<Image>();
        m_Camera = Camera.main;
        m_Player = GameObject.FindGameObjectWithTag("Player");
    }
    public void SetPlus()
    {
        if (m_Lenth == 1) return;
        else if (m_Lenth == 2)
        {
            m_KeyPos = 0;
        }
        else if (m_Lenth == 3)
        {
            m_KeyPos = 0.375f;
        }
    }

    private void Update()
    {
        m_Image.enabled = TutorialManager.instance.m_FrontKeys[1].m_Image.enabled;
        transform.position = m_Camera.WorldToScreenPoint(m_Player.transform.position + new Vector3(m_KeyPos, 1.6f));
    }
}
