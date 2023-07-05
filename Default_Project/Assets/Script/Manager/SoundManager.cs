using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public GameObject m_DefaultSetting;
    public GameObject m_SoundSetting;

    public void OnSoundSetting()
    {
        m_DefaultSetting.SetActive(!m_DefaultSetting.activeSelf);
        m_SoundSetting.SetActive(!m_SoundSetting.activeSelf);
    }
}
