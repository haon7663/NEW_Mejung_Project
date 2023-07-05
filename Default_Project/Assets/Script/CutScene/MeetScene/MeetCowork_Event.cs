using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class MeetCowork_Event : SceneEvent
{
    public CinemachineVirtualCamera cinevirtual;
    public CinemachineConfiner2D mCinemachineConfiner;
    private CinemachineTransposer mCinemachineTransposer;

    private RadioDialogue m_RadioDialogue;
    private Move m_PlayerMove;
    private Animator m_PlayerAnimator;
    private SpriteRenderer m_PlayerSpriteRenderer;
    public Transform m_Sageia;
    public float m_SageiaSpeed;
    private Animator m_SageiaAnimator;
    private SpriteRenderer m_SageiaSpriteRenderer;
    public Transform m_Ian;
    public float m_IanSpeed;
    private Animator m_IanAnimator;
    private SpriteRenderer m_IanSpriteRenderer;
    public Transform m_Derrick;
    public float m_DerrickSpeed;
    private Animator m_DerrickAnimator;
    private SpriteRenderer m_DerrickSpriteRenderer;
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
        m_SageiaAnimator = m_Sageia.GetComponent<Animator>();
        m_SageiaSpriteRenderer = m_Sageia.GetComponent<SpriteRenderer>();

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
        m_PlayerMove.isCutScene = true;
        mCinemachineConfiner.enabled = false;
        yield return YieldInstructionCache.WaitForSeconds(1.2f);
        while(m_Sageia.position.x < 90f)
        {
            m_Sageia.Translate(new Vector3(m_SageiaSpeed, 0) * Time.deltaTime);
            m_SageiaAnimator.SetBool("isWalk", true);
            m_SageiaSpriteRenderer.flipX = false;
            yield return YieldInstructionCache.WaitForFixedUpdate;
        }
        m_SageiaAnimator.SetBool("isLookAround", true);
        m_SageiaAnimator.SetBool("isWalk", false);
        yield return YieldInstructionCache.WaitForSeconds(1.25f);
        m_PlayerSpriteRenderer.flipX = true;
        yield return YieldInstructionCache.WaitForSeconds(1.7f);
        m_SageiaAnimator.SetBool("isLookAround", false);

        yield return YieldInstructionCache.WaitForSeconds(2f);

        m_SageiaAnimator.SetTrigger("surprise");
        m_SageiaAnimator.SetBool("isRadio", true);

        yield return YieldInstructionCache.WaitForSeconds(7f);
        m_SageiaAnimator.SetBool("isRadio", false);
        yield return YieldInstructionCache.WaitForSeconds(1.5f);
        m_SageiaAnimator.SetBool("isLookAround", true);

        yield return YieldInstructionCache.WaitForSeconds(1.25f);

        while (m_Derrick.position.x < 90f)
        {
            m_Derrick.Translate(new Vector3(m_SageiaSpeed, 0) * Time.deltaTime);
            m_DerrickAnimator.SetBool("isWalk", true);
            m_DerrickSpriteRenderer.flipX = false;
            yield return YieldInstructionCache.WaitForFixedUpdate;
        }
        while (m_Sageia.position.x < 90f)
        {
            m_Sageia.Translate(new Vector3(m_SageiaSpeed, 0) * Time.deltaTime);
            m_SageiaAnimator.SetBool("isWalk", true);
            m_SageiaSpriteRenderer.flipX = false;
            yield return YieldInstructionCache.WaitForFixedUpdate;
        }

        /*m_PlayerMove.CinemacineSize = 6;
        cinevirtual.Follow = m_FollowPlayer.transform;

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
        yield return YieldInstructionCache.WaitForSeconds(3f);
        m_PlayerAnimator.SetBool("isTurnBack", false);

        StartRadio("½ºÆÀ´ë½¬È¹µæ");

        GameManager.GM.onRadio = true;
        while (GameManager.GM.onRadio)
        {
            yield return YieldInstructionCache.WaitForFixedUpdate;
        }*/
        m_PlayerMove.isCutScene = false;
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