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

    [Space]
    [Header("Save")]
    public int savePoint;

    [Space]
    [Header("UI")]
    public Image mPause;
    public GameObject mDefaultPause;
    public GameObject mKeySetting;

    [Space]
    [Header("Bool")]
    public bool onPause = false;
    public bool onRadio = false;

    private Move m_PlayerMove;

    private void Awake()
    {
        GM = this;
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
    private void Start()
    {
        Application.targetFrameRate = 60;
        Fade.instance.FadeOut(0.5f);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !m_PlayerMove.isDeath)
        {
            Pause();
        }
    }
    public void Pause()
    {
        Time.timeScale = Time.timeScale == 0 ? 1 : 0;
        onPause = Time.timeScale == 0;
        mPause.gameObject.SetActive(onPause);
        mDefaultPause.SetActive(true);
        mKeySetting.SetActive(false);
    }
    public void Setting()
    {
        Debug.Log("Setting");
        mDefaultPause.SetActive(false);
        mKeySetting.SetActive(true);
    }
    public void CheckPoint()
    {
        Debug.Log("CheckPoint");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void Main()
    {
        Debug.Log("Main");
    }
    public void Exit()
    {
        Debug.Log("Exit");
        Application.Quit();
    }
}
