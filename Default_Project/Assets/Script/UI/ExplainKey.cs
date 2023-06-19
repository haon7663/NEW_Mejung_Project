using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExplainKey : MonoBehaviour
{
    private Image m_Image;
    public bool isWorking;
    public Sprite m_NonPush;
    public Sprite m_Push;

    private Camera m_Camera;
    private GameObject m_Player;

    private void Start()
    {
        m_Camera = Camera.main;
        m_Player = GameObject.FindGameObjectWithTag("Player");
        m_Image = GetComponent<Image>();

        StartCoroutine(ShowKey(true));
    }
    public IEnumerator ShowKey(bool isWork)
    {
        isWorking = isWork;
        while(isWorking)
        {
            for (float i = 0; i < 0.4f; i += Time.deltaTime)
            {
                transform.position = m_Camera.WorldToScreenPoint(m_Player.transform.position + new Vector3(0, 1.75f));
                if (!isWorking) break;
                yield return YieldInstructionCache.WaitForFixedUpdate;
            }
            m_Image.sprite = m_Image.sprite == m_NonPush ? m_Push : m_NonPush;
        }
        yield return null;
    }
}
