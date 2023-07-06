using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

public class BreakBridgeEvent : SceneEvent
{
    public CinemachineVirtualCamera cinevirtual;
    public CinemachineConfiner2D mCinemachineConfiner;
    private CinemachineTransposer mCinemachineTransposer;

    private RadioDialogue m_RadioDialogue;
    private Move m_PlayerMove;
    private Animator m_PlayerAnimator;
    private SpriteRenderer m_PlayerSpriteRenderer;
    public Transform m_EventFollow;
    public GameObject m_FollowChange;
    public GameObject m_FollowPlayer;
    private LetterBoxManager m_LetterBox;

    public GameObject[] Wreck;

    private bool isCalled = false;
    private void Start()
    {
        m_RadioDialogue = GameObject.FindGameObjectWithTag("Dialogue").GetComponent<RadioDialogue>();
        m_PlayerMove = GameObject.FindGameObjectWithTag("Player").GetComponent<Move>();
        m_PlayerAnimator = m_PlayerMove.GetComponent<Animator>();
        m_PlayerSpriteRenderer = m_PlayerMove.GetComponent<SpriteRenderer>();
        m_LetterBox = GameManager.GM.GetComponent<LetterBoxManager>();

        mCinemachineTransposer = cinevirtual.GetCinemachineComponent<CinemachineTransposer>();
    }
    public override void Event()
    {
        if(!isCalled)
        {
            isCalled = true;
            StartCoroutine(PlayEvent());
        }
    }

    public void StartRadio(string eventName)
    {
        m_RadioDialogue.StartDialogue(eventName);
    }

    public IEnumerator PlayEvent()
    {
        Debug.Log("ds");
        m_PlayerMove.CinemacineSize = 8;
        m_LetterBox.LetterIn();
        m_FollowChange.SetActive(true);
        CinemachineShake.Instance.ShakeCamera(7, 0.7f);
        yield return YieldInstructionCache.WaitForSeconds(0.2f);
        m_PlayerMove.isRun = false;
        mCinemachineConfiner.m_Damping = 4;
        mCinemachineTransposer.m_XDamping = 3;
        mCinemachineTransposer.m_YDamping = 3;
        m_FollowPlayer.transform.localPosition = new Vector3(0, 0, 0);

        m_PlayerMove.CinemacineSize = 6;

        m_PlayerMove.isCalledScene = true;
        StartRadio("Áøµ¿");
        GameManager.GM.onRadio = true;
        while (GameManager.GM.onRadio)
        {
            m_PlayerMove.CinemacineSize = 6;
            yield return YieldInstructionCache.WaitForFixedUpdate;
        }

        m_PlayerMove.isRun = true;
        m_PlayerMove.m_CutX = 1;
        float t = 0;
        for (float j = 0; j < 6.5f; j += Time.deltaTime)
        {
            m_FollowPlayer.transform.localPosition = new Vector3(-2, 0, 0);
            t += Time.deltaTime;
            if (t > 0.6f)
            {
                t = 0;
                Instantiate(Wreck[Random.Range(0, 6)], m_PlayerMove.transform.position + new Vector3(0, 40), Quaternion.identity);
            }
            yield return YieldInstructionCache.WaitForFixedUpdate;
        }
        for (float j = 0; j < 1.5f; j += Time.deltaTime)
        {
            m_FollowPlayer.transform.localPosition = new Vector3(4, 0, 0);
            yield return YieldInstructionCache.WaitForFixedUpdate;
        }
        m_PlayerMove.isRun = false;
        m_PlayerMove.m_CutX = 1;
        m_PlayerAnimator.SetTrigger("break");

        StartRadio("³«ÇÏ");
        GameManager.GM.onRadio = true;
        while (GameManager.GM.onRadio)
        {
            m_FollowPlayer.transform.localPosition = new Vector3(9, 0, 0);
            CinemachineShake.Instance.ShakeCamera(3, Time.deltaTime*2);
            m_PlayerMove.CinemacineSize = 5;
            yield return YieldInstructionCache.WaitForFixedUpdate;
            m_FollowPlayer.transform.localPosition = new Vector3(9, 0, 0);
        }
        m_PlayerMove.Jump();
        m_PlayerMove.m_CutX = 1;
        m_PlayerMove.isRun = true;

        for (float j = 0; j < 1.2f; j += Time.deltaTime)
        {
            m_FollowPlayer.transform.localPosition = new Vector3(3, 0, 0);
            CinemachineShake.Instance.ShakeCamera(3, Time.deltaTime * 2);
            yield return YieldInstructionCache.WaitForFixedUpdate;
        }
        Fade.instance.FadeIn(0.5f);
        for (float j = 0; j < 0.5f; j += Time.deltaTime)
        {
            m_FollowPlayer.transform.localPosition = new Vector3(3, 0, 0);
            CinemachineShake.Instance.ShakeCamera(3, Time.deltaTime * 2);
            yield return YieldInstructionCache.WaitForFixedUpdate;
        }
        SceneManager.LoadScene("Map_2");
    }
}