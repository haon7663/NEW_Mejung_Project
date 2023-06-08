using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Fade : MonoBehaviour
{
    public static Fade instance;
    private Image m_Image;

    private void Awake()
    {
        instance = this;
        m_Image = GetComponent<Image>();
    }
    public void FadeIn(float time)
    {
        m_Image.enabled = true;
        m_Image.color = new Color(0, 0, 0, 0);
        m_Image.DOFade(1, time);
    }
    public void FadeOut(float time)
    {
        m_Image.enabled = true;
        m_Image.color = new Color(0, 0, 0, 1);
        m_Image.DOFade(0, time);
        //Invoke(nameof(DelayEnable), time);
    }
    private void DelayEnable() => m_Image.enabled = false;
}
