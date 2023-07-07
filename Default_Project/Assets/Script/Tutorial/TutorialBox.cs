using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialBox : MonoBehaviour
{
    private BoxCollider2D m_BoxCollider2D;

    public LayerMask m_PlayerLayer;
    public bool isThis = false;
    public TutorialBox saveTutorial;

    [Header("LEFT, RIGHT, DOWN, UP, JUMP")]
    [Header("DASH, STEAM, INTERACTION, KEYCOUNT")]
    [Space]
    public string[] m_FrontKeyActions;
    public ButtonTutorial[] m_FrontKeyScripts;
    public PlusTutorial m_PlusTutorial; 

    private void Start()
    {
        for (int i = 0; i < m_FrontKeyScripts.Length; i++)
        {
            m_FrontKeyScripts[i] = TutorialManager.instance.m_FrontKeys[i];
        }
        m_PlusTutorial = TutorialManager.instance.m_PlusKey;
    }
    public void Event()
    {
        for (int i = 0; i < m_FrontKeyScripts.Length; i++)
        {
            KeyAction keys = (KeyAction)Enum.Parse(typeof(KeyAction), m_FrontKeyActions[i]);
            for (int j = 0; j < m_FrontKeyScripts.Length; j++)
            {
                m_FrontKeyScripts[i].m_KeyAction[j] = m_FrontKeyActions[j];
            }
            for (int j = m_FrontKeyScripts.Length; j < 3; j++)
            {
                m_FrontKeyScripts[i].m_KeyAction[j] = "";
            }
            m_FrontKeyScripts[i].m_KeyCount = i;
            m_FrontKeyScripts[i].m_Lenth = m_FrontKeyScripts.Length;
            m_FrontKeyScripts[i].SetKey(KeySetting.keys[keys].ToString());
        }
        m_PlusTutorial.m_Lenth = m_FrontKeyScripts.Length;
        m_PlusTutorial.SetPlus();
    }
}
