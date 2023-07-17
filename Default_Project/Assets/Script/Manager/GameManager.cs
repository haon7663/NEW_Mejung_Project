using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager GM;
    public static bool isCalled;

    private KeyManager m_KeyManager;
    private DialogueParse m_DialogueParse;

    public AudioOptions m_Asdu;

    [Space]
    [Header("Save")]
    public int savePoint;

    [Space]
    [Header("UI")]
    public Image mPause;
    public GameObject mDefaultPause;
    public GameObject mSoundSetting;
    public GameObject mKeySetting;
    public float m_FadeTime = 0.5f;

    [Space]
    [Header("Bool")]
    public bool canPause = true;
    public bool onPause = false;
    public bool onRadio = false;

    private Move m_PlayerMove;

    private void Awake()
    {
        GM = this;
        if(canPause)
        {
            m_KeyManager = GetComponent<KeyManager>();
            m_DialogueParse = GetComponent<DialogueParse>();
            m_PlayerMove = GameObject.FindGameObjectWithTag("Player").GetComponent<Move>();

            if (!isCalled)
            {
                m_DialogueParse.SetTalkDictionary();
                m_KeyManager.SetKey();
                isCalled = true;
            }
        }
    }
    private void Start()
    {
        Application.targetFrameRate = 60;
        Fade.instance.FadeOut(m_FadeTime);

        if (canPause)
        {
            m_Asdu.MasterSlider.value = PlayerPrefs.GetFloat("Master");
            m_Asdu.BgmSlider.value = PlayerPrefs.GetFloat("BGM");
            m_Asdu.SfxSlider.value = PlayerPrefs.GetFloat("SFX");
            m_Asdu.SetMasterVolme();
            m_Asdu.SetBgmVolme();
            m_Asdu.SetSFXVolme();
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !m_PlayerMove.isDeath && canPause)
        {
            Pause();
        }
    }
    public void Pause()
    {
        if(mDefaultPause.activeSelf)
        {
            Time.timeScale = Time.timeScale == 0 ? 1 : 0;
            onPause = Time.timeScale == 0;
            mPause.gameObject.SetActive(onPause);
            mDefaultPause.SetActive(true);
            mSoundSetting.SetActive(false);
            mKeySetting.SetActive(false);
        }
        else
        {
            mDefaultPause.SetActive(true);
            mSoundSetting.SetActive(false);
            mKeySetting.SetActive(false);
        }
    }
    public void Setting()
    {
        Debug.Log("Setting");
        mDefaultPause.SetActive(false);
        mSoundSetting.SetActive(false);
        mKeySetting.SetActive(true);
    }
    public void SoundSetting()
    {
        Debug.Log("SoundSetting");
        mDefaultPause.SetActive(false);
        mSoundSetting.SetActive(true);
        mKeySetting.SetActive(false);
    }
    public void CheckPoint()
    {
        Debug.Log("CheckPoint");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void Main()
    {
        Debug.Log("Main");
        Time.timeScale = 1;
        SceneManager.LoadScene("Title");
    }
    public void Exit()
    {
        Debug.Log("Exit");
        Application.Quit();
    }
    public void ResetGame()
    {
        GameManager.GM.savePoint = -1;
        DataManager.instance.JsonSave();
    }
}
