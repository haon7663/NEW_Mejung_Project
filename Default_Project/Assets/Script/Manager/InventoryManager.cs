using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager IM;

    public int maxKey = 3;
    public int key;

    public Image m_KeyBarImage;
    public Image m_KeyImage;
    public Text m_KeyText;

    private void Awake()
    {
        IM = this;
    }

    public void GetKey()
    {
        AllDOFade(1);

        m_KeyText.text = key + "/" + maxKey;
    }

    public void OpenDoor()
    {
        AllDOFade(0);
    }

    private void AllDOFade(float a)
    {
        m_KeyBarImage.DOFade(a, 0.5f);
        m_KeyImage.DOFade(a, 0.5f);
        m_KeyText.DOFade(a, 0.5f);
    }
}
