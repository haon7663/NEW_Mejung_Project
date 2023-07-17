using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GetSteam_Event : SceneEvent
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
        StartCoroutine(PlayEvent());
    }

    public void StartRadio(string eventName)
    {
        m_RadioDialogue.StartDialogue(eventName);
    }

    public IEnumerator PlayEvent()
    {
        mCinemachineConfiner.m_Damping = 4;
        mCinemachineTransposer.m_XDamping = 3;
        mCinemachineTransposer.m_YDamping = 3;
        m_LetterBox.LetterIn();
        m_FollowChange.SetActive(true);
        mCinemachineConfiner.enabled = false;
        m_PlayerMove.CinemacineSize = 6;
        m_PlayerMove.isCutScene = true;
        m_PlayerMove.isCalledScene = true;
        yield return YieldInstructionCache.WaitForSeconds(1.75f);
        m_PlayerMove.CinemacineSize = 9;
        cinevirtual.Follow = m_EventFollow;
        yield return YieldInstructionCache.WaitForSeconds(3f);
        m_PlayerMove.CinemacineSize = 6;
        cinevirtual.Follow = m_FollowPlayer.transform;
        yield return YieldInstructionCache.WaitForSeconds(1.5f);

        StartRadio("½ºÆÀ´ë½¬");

        GameManager.GM.onRadio = true;
        while (GameManager.GM.onRadio)
        {
            m_PlayerMove.CinemacineSize = 6;
            yield return YieldInstructionCache.WaitForFixedUpdate;
        }

        while (m_PlayerMove.transform.position.x >= 36.5f)
        {
            m_PlayerMove.isWalk = true;
            m_PlayerMove.m_CutX = -1;
            yield return YieldInstructionCache.WaitForFixedUpdate;
        }
        m_PlayerMove.isWalk = false;
        m_PlayerMove.m_CutX = 0;

        for (float j = 0; j < 0.7f; j += Time.deltaTime)
        {
            m_PlayerSpriteRenderer.flipX = false;
            yield return YieldInstructionCache.WaitForFixedUpdate;
        }
        m_PlayerAnimator.SetBool("isTurnBack", true);
        m_PlayerAnimator.SetTrigger("back");
        yield return YieldInstructionCache.WaitForSeconds(2f);
        m_PlayerAnimator.SetTrigger("pluck");
        yield return YieldInstructionCache.WaitForSeconds(2.25f);
        m_PlayerAnimator.SetBool("isTurnBack", false);

        StartRadio("½ºÆÀ´ë½¬È¹µæ");

        GameManager.GM.onRadio = true;
        while (GameManager.GM.onRadio)
        {
            yield return YieldInstructionCache.WaitForFixedUpdate;
        }
        m_PlayerMove.isCutScene = false;
        m_PlayerMove.isCalledScene = false;
        m_PlayerMove.CinemacineSize = 10;
        m_LetterBox.LetterOut();

        mCinemachineTransposer.m_XDamping = 1;
        mCinemachineTransposer.m_YDamping = 0.6f;
        mCinemachineConfiner.enabled = true;
        m_FollowChange.SetActive(false);
        mCinemachineConfiner.m_Damping = 0.6f;
        Destroy(gameObject);
    }
}

internal static class YieldInstructionCache
{
    public static readonly WaitForEndOfFrame WaitForEndOfFrame = new WaitForEndOfFrame();
    public static readonly WaitForFixedUpdate WaitForFixedUpdate = new WaitForFixedUpdate();
    private static readonly Dictionary<float, WaitForSeconds> waitForSeconds = new Dictionary<float, WaitForSeconds>();

    public static WaitForSeconds WaitForSeconds(float seconds)
    {
        WaitForSeconds wfs;
        if (!waitForSeconds.TryGetValue(seconds, out wfs))
            waitForSeconds.Add(seconds, wfs = new WaitForSeconds(seconds));
        return wfs;
    }
}