using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonTutorial : MonoBehaviour
{
    public static ButtonTutorial instance;
    public String[] m_KeyAction;

    public bool isPush;
    public bool onSetDown = false;
    public Image m_Image;
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
    public bool isWorkPro;
    public Sprite m_NonPush;
    public Sprite m_Push;

    private Camera m_Camera;
    private GameObject m_Player;
    private Interaction m_PlayerInteraction;

    public string SettedKey;
    public int m_KeyCount;
    public int m_Lenth;
    public float m_KeyPos;

    private string saveKey;

    private float bottonUpTIme;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        m_Camera = Camera.main;
        m_Player = GameObject.FindGameObjectWithTag("Player");
        m_PlayerInteraction = m_Player.GetComponent<Interaction>();
        m_Image = GetComponent<Image>();

        m_Rect = GetComponent<RectTransform>();
        m_TextRect = m_Text.GetComponent<RectTransform>();
        m_TextDoubleRect = m_TextDouble.GetComponent<RectTransform>();
    }

    private void Update()
    {
        var isTutorial = m_PlayerInteraction.tutorial;

        if(isTutorial != null && !m_PlayerInteraction.tutorialWait && m_KeyAction[0] != "")
        {
            if (m_Lenth == 1) m_KeyPos = 0;
            else if (m_Lenth == 2)
            {
                m_KeyPos = m_KeyCount == 0 ? 0.75f : -0.75f;
            }
            else if (m_Lenth == 3)
            {
                m_KeyPos = m_KeyCount == 0 ? 1f : m_KeyCount == 1 ? -1 : -0.25f;
            }
            m_Image.enabled = true;
            if (SettedKey == "Space")
            {
                m_Space.enabled = true;
            }
            else if (SettedKey.Length == 1)
            {
                m_Text.text = SettedKey;
                m_Text.enabled = true;
            }
            else
            {
                m_TextDouble.text = SettedKey;
                m_TextDouble.enabled = true;
            }

            bottonUpTIme -= Time.deltaTime;
            if(bottonUpTIme < 0)
            {
                bottonUpTIme = 0.4f;
                isPush = !isPush;
                PushSet();
            }
            if (m_Lenth == 1)
            {
                KeyAction key = (KeyAction)Enum.Parse(typeof(KeyAction), m_KeyAction[0]);
                transform.position = m_Camera.WorldToScreenPoint(m_Player.transform.position + new Vector3(m_KeyPos, 1.75f));
                if (Input.GetKey(KeySetting.keys[key])) m_PlayerInteraction.tutorialWait = true;
            }
            else if (m_Lenth == 2)
            {
                KeyAction key = (KeyAction)Enum.Parse(typeof(KeyAction), m_KeyAction[0]);
                KeyAction key1 = (KeyAction)Enum.Parse(typeof(KeyAction), m_KeyAction[1]);
                transform.position = m_Camera.WorldToScreenPoint(m_Player.transform.position + new Vector3(m_KeyPos, 1.75f));
                if (Input.GetKey(KeySetting.keys[key]) && Input.GetKey(KeySetting.keys[key1])) m_PlayerInteraction.tutorialWait = true;
            }
            else if (m_Lenth == 3)
            {
                KeyAction key = (KeyAction)Enum.Parse(typeof(KeyAction), m_KeyAction[0]);
                KeyAction key1 = (KeyAction)Enum.Parse(typeof(KeyAction), m_KeyAction[1]);
                KeyAction key2 = (KeyAction)Enum.Parse(typeof(KeyAction), m_KeyAction[2]);
                transform.position = m_Camera.WorldToScreenPoint(m_Player.transform.position + new Vector3(m_KeyPos, 1.75f));
                if (Input.GetKey(KeySetting.keys[key]) && Input.GetKey(KeySetting.keys[key1]) && Input.GetKey(KeySetting.keys[key2])) m_PlayerInteraction.tutorialWait = true;
            }
        }
        else
        {
            for(int i = 0; i < 3; i++)
            {
                m_KeyAction[i] = "";
            }
            onSetDown = false;
            m_Image.enabled = false;
            m_Space.enabled = false;
            m_Text.enabled = false;
            m_TextDouble.enabled = false;
            isWorking = false;
            isPush = true;
            bottonUpTIme = -0.1f;
        }
    }

    public void SetKey(string key)
    {
        SettedKey = key;
    }

    /*private IEnumerator ShowedKey(bool isWork)
    {
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
        yield return YieldInstructionCache.WaitForFixedUpdate;
        yield return YieldInstructionCache.WaitForFixedUpdate;
        isWorking = true;
        isWorkPro = false;

        isPush = false;
        PushSet();
        while (isWorking)
        {
            if (m_Lenth == 1)
            {
                KeyAction key = (KeyAction)Enum.Parse(typeof(KeyAction), m_KeyAction[0]);

                for (float j = 0; j < 0.4f; j += Time.deltaTime)
                {
                    transform.position = m_Camera.WorldToScreenPoint(m_Player.transform.position + new Vector3(m_KeyPos, 1.75f));
                    if (Input.GetKey(KeySetting.keys[key])) break;
                    yield return YieldInstructionCache.WaitForFixedUpdate;
                }
                isPush = !isPush;
                PushSet();
                if (Input.GetKey(KeySetting.keys[key])) break;
            }
            else if (m_Lenth == 2)
            {
                KeyAction key = (KeyAction)Enum.Parse(typeof(KeyAction), m_KeyAction[0]);
                KeyAction key1 = (KeyAction)Enum.Parse(typeof(KeyAction), m_KeyAction[1]);

                for (float j = 0; j < 0.4f; j += Time.deltaTime)
                {
                    transform.position = m_Camera.WorldToScreenPoint(m_Player.transform.position + new Vector3(m_KeyPos, 1.75f));
                    if (Input.GetKey(KeySetting.keys[key]) && Input.GetKey(KeySetting.keys[key1])) break;
                    yield return YieldInstructionCache.WaitForFixedUpdate;
                }
                isPush = !isPush;
                PushSet();
                if (Input.GetKey(KeySetting.keys[key]) && Input.GetKey(KeySetting.keys[key1])) break;
            }
            else if (m_Lenth == 3)
            {
                KeyAction key = (KeyAction)Enum.Parse(typeof(KeyAction), m_KeyAction[0]);
                KeyAction key1 = (KeyAction)Enum.Parse(typeof(KeyAction), m_KeyAction[1]);
                KeyAction key2 = (KeyAction)Enum.Parse(typeof(KeyAction), m_KeyAction[2]);

                for (float j = 0; j < 0.4f; j += Time.deltaTime)
                {
                    transform.position = m_Camera.WorldToScreenPoint(m_Player.transform.position + new Vector3(m_KeyPos, 1.75f));
                    if (Input.GetKey(KeySetting.keys[key]) && Input.GetKey(KeySetting.keys[key1]) && Input.GetKey(KeySetting.keys[key2])) break;
                    yield return YieldInstructionCache.WaitForFixedUpdate;
                }
                isPush = !isPush;
                PushSet();
                if (Input.GetKey(KeySetting.keys[key]) && Input.GetKey(KeySetting.keys[key1]) && Input.GetKey(KeySetting.keys[key2])) break;
            }
        }
        onSetDown = false;
        m_Image.enabled = false;
        m_Space.enabled = false;
        m_Text.enabled = false;
        m_TextDouble.enabled = false;
        isWorking = false;
        isPush = false;

        yield return null;
    }*/

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
