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
    public string m_KeyAction;
    public string[] m_FrontKeyActions;

    private ButtonTutorial m_KeyScript;
    private ButtonTutorial[] m_FrontKeyScripts;

    private void Start()
    {
        m_KeyScript = TutorialManager.instance.m_Key;
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
        if (!m_KeyScript.isWorking)
        {
            KeyAction key = (KeyAction)Enum.Parse(typeof(KeyAction), m_KeyAction);
            m_KeyScript.m_KeyAction = m_KeyAction;
            m_KeyScript.m_KeyCount = 0;
            m_KeyScript.SetKey(KeySetting.keys[key].ToString());
            if (!isSingle)
            {
                for (int i = 0; i < m_FrontKeyScripts.Length; i++)
                {
                    KeyAction keys = (KeyAction)Enum.Parse(typeof(KeyAction), m_FrontKeyActions[i]);
                    m_FrontKeyScripts[i].m_KeyAction = m_FrontKeyActions[i];
                    m_FrontKeyScripts[i].m_KeyCount = i + 1;
                    m_FrontKeyScripts[i].SetKey(KeySetting.keys[keys].ToString());
                }
            }
        }
    }
}
