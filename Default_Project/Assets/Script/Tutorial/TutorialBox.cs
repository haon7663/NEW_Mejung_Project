using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialBox : SceneEvent
{
    public bool isSingle = true;
    [Header("LEFT, RIGHT, DOWN, UP, JUMP")]
    [Header("DASH, STEAM, INTERACTION, KEYCOUNT")]
    [Space]
    public string[] m_FrontKeyActions;
    public ButtonTutorial[] m_FrontKeyScripts;

    private void Start()
    {
        if (!isSingle)
        {
            for (int i = 0; i < m_FrontKeyScripts.Length; i++)
            {
                m_FrontKeyScripts[i] = TutorialManager.instance.m_FrontKeys[i];
            }
        }
    }
    public override void Event()
    {
        if (!m_FrontKeyScripts[0].isWorking)
        {
            for (int i = 0; i < m_FrontKeyScripts.Length; i++)
            {
                KeyAction keys = (KeyAction)Enum.Parse(typeof(KeyAction), m_FrontKeyActions[i]);
                for (int j = 0; j < m_FrontKeyScripts.Length; j++)
                {
                    m_FrontKeyScripts[i].m_KeyAction[j] = m_FrontKeyActions[j];
                }
                m_FrontKeyScripts[i].m_KeyCount = i;
                m_FrontKeyScripts[i].m_Lenth = m_FrontKeyScripts.Length;
                m_FrontKeyScripts[i].SetKey(KeySetting.keys[keys].ToString());
            }
        }
    }
}
