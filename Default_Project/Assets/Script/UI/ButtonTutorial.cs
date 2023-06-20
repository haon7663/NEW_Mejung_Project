using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonTutorial : MonoBehaviour
{
    public static ButtonTutorial instance;
    public String m_KeyAction;

    private bool isPush;
    private Image m_Image;
    public Text m_Text;
    public Text m_TextDouble;
    private RectTransform m_Rect;
    private RectTransform m_TextRect;
    private RectTransform m_TextDoubleRect;

    public Image m_Space;
    private RectTransform m_SpaceRect;
    public Sprite NonPushSpace;
    public Sprite PushSpace;

    public bool isWorking;
    public Sprite m_NonPush;
    public Sprite m_Push;

    private Camera m_Camera;
    private GameObject m_Player;

    public string SettedKey;
    public int m_KeyCount;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        m_Camera = Camera.main;
        m_Player = GameObject.FindGameObjectWithTag("Player");
        m_Image = GetComponent<Image>();

        m_Rect = GetComponent<RectTransform>();
        m_TextRect = m_Text.GetComponent<RectTransform>();
        m_TextDoubleRect = m_TextDouble.GetComponent<RectTransform>();
    }

    public void SetKey(string key)
    {
        SettedKey = key;
        StartCoroutine(ShowedKey(true));
    }

    private IEnumerator ShowedKey(bool isWork)
    {
        isWorking = isWork;
        m_Image.enabled = isWork;
        if(isWork)
        {
            if (SettedKey == "Space")
            {
                m_Space.enabled = true;
            }
            else if(SettedKey.Length == 1)
            {
                m_Text.text = SettedKey;
                m_Text.enabled = true;
            }
            else
            {
                m_TextDouble.text = SettedKey;
                m_TextDouble.enabled = true;
            }
        }
        KeyAction key = (KeyAction)Enum.Parse(typeof(KeyAction), m_KeyAction);
        isPush = false;
        PushSet();
        for (float i = 0; i < 8; i ++)
        {
            for (float j = 0; j < 0.4f; j += Time.deltaTime)
            {
                transform.position = m_Camera.WorldToScreenPoint(m_Player.transform.position + new Vector3(0, 1.75f));
                if (Input.GetKey(KeySetting.keys[key])) break;
                yield return YieldInstructionCache.WaitForFixedUpdate;
            }
            isPush = !isPush;
            PushSet();

            if (Input.GetKey(KeySetting.keys[key])) break;
        }
        m_Image.enabled = false;
        m_Space.enabled = false;
        m_Text.enabled = false;
        m_TextDouble.enabled = false;
        isWorking = false;
    }

    private void PushSet()
    {
        float setColor = isPush ? 0.5f : 1;
        m_Image.sprite = isPush ? m_Push : m_NonPush;
        m_TextRect.localPosition = new Vector3(m_TextRect.localPosition.x, isPush ? -6.8f : 2.66f);
        m_Text.color = new Color(setColor, setColor, setColor, 1);
        m_TextDoubleRect.localPosition = new Vector3(m_TextDoubleRect.localPosition.x, isPush ? -4.61f : 4.75f);
        m_TextDouble.color = new Color(setColor, setColor, setColor, 1);
        m_Space.sprite = isPush ? PushSpace : NonPushSpace;
        m_Space.color = new Color(setColor, setColor, setColor, 1);
    }
}
