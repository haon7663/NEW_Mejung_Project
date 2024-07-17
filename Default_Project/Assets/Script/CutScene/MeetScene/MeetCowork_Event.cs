using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;
using UnityEngine.UI;

public class MeetCowork_Event : SceneEvent
{
    public CinemachineVirtualCamera cinevirtual;
    public CinemachineConfiner2D mCinemachineConfiner;
    private CinemachineTransposer mCinemachineTransposer;

    private RadioDialogue m_RadioDialogue;
    private TextboxDialog m_TextboxDialogue;
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

    private bool skipAble = false;
    public Text interSkip;

    private void Start()
    {
        m_RadioDialogue = GameObject.FindGameObjectWithTag("Dialogue").GetComponent<RadioDialogue>();
        m_TextboxDialogue= GameObject.FindGameObjectWithTag("Dialogue").GetComponent<TextboxDialog>();
        m_PlayerMove = GameObject.FindGameObjectWithTag("Player").GetComponent<Move>();
        m_PlayerAnimator = m_PlayerMove.GetComponent<Animator>();
        m_PlayerSpriteRenderer = m_PlayerMove.GetComponent<SpriteRenderer>();
        m_SageiaAnimator = m_Sageia.GetComponent<Animator>();
        m_SageiaSpriteRenderer = m_Sageia.GetComponent<SpriteRenderer>();
        m_IanAnimator = m_Ian.GetComponent<Animator>();
        m_IanSpriteRenderer = m_Ian.GetComponent<SpriteRenderer>();
        m_DerrickAnimator = m_Derrick.GetComponent<Animator>();
        m_DerrickSpriteRenderer = m_Derrick.GetComponent<SpriteRenderer>();

        m_LetterBox = GameManager.GM.GetComponent<LetterBoxManager>();


        mCinemachineTransposer = cinevirtual.GetCinemachineComponent<CinemachineTransposer>();
    }

    private void Update()
    {
        if (skipAble && Input.GetKeyDown(KeyCode.E))
        {
            skipAble = false;
            GameManager.GM.savePoint++;
            GameManager.GM.gameObject.GetComponent<DataManager>().JsonSave();
            Fade.instance.FadeIn(0.5f);
            RadioBox.isCalled = false;
            Invoke(nameof(MoveScene), 0.51f);
        }
    }

    private void MoveScene()
    {
        SceneManager.LoadScene("Tutorial");
    }

    public override void Event()
    {
        StartCoroutine(PlayEvent());
        skipAble = true;
        interSkip.enabled = true;
        interSkip.color = Color.white;
        interSkip.rectTransform.SetAsLastSibling();
    }

    public void StartRadio(string eventName)
    {
        m_RadioDialogue.StartDialogue(eventName);
    }
    public void StartTextBox(string eventName)
    {
        m_TextboxDialogue.StartDialogue(eventName);
    }

    public IEnumerator PlayEvent()
    {
        cinevirtual.Follow = m_EventFollow;
        mCinemachineConfiner.m_Damping = 4;
        mCinemachineTransposer.m_XDamping = 3;
        mCinemachineTransposer.m_YDamping = 3;
        m_LetterBox.LetterIn();
        m_FollowChange.SetActive(true);
        m_PlayerMove.isCutScene = true;
        m_PlayerMove.isCalledScene = true;
        mCinemachineConfiner.enabled = false;
        yield return YieldInstructionCache.WaitForSeconds(1.2f);
        while(m_Sageia.position.x < 90.75f)
        {
            m_Sageia.Translate(new Vector3(m_SageiaSpeed, 0) * Time.deltaTime);
            m_SageiaAnimator.SetBool("isWalk", true);
            m_SageiaSpriteRenderer.flipX = false;
            yield return YieldInstructionCache.WaitForFixedUpdate;
        }
        StartTextBox("컷신1");
        m_TextboxDialogue.onText = true;
        m_SageiaAnimator.SetBool("isLookAround", true);
        m_SageiaAnimator.SetBool("isWalk", false);
        yield return YieldInstructionCache.WaitForSeconds(1.25f);
        m_PlayerSpriteRenderer.flipX = true;

        yield return YieldInstructionCache.WaitForSeconds(1.75f);
        m_TextboxDialogue.onText = false;
        yield return YieldInstructionCache.WaitForSeconds(0.6f);
        m_TextboxDialogue.m_ImageKinds = 1;
        m_TextboxDialogue.onText = true;
        yield return YieldInstructionCache.WaitForSeconds(0.15f);
        m_SageiaAnimator.SetBool("isLookAround", false);
        m_SageiaAnimator.SetTrigger("surprise");
        m_SageiaAnimator.SetBool("isRadio", true);

        yield return YieldInstructionCache.WaitForSeconds(2.5f);
        m_TextboxDialogue.onText = false;
        yield return YieldInstructionCache.WaitForSeconds(0.1f);
        m_TextboxDialogue.m_ImageKinds = 0;
        m_TextboxDialogue.onText = true;
        yield return YieldInstructionCache.WaitForSeconds(3f);
        m_TextboxDialogue.onText = false;
        yield return YieldInstructionCache.WaitForSeconds(0.1f);
        m_TextboxDialogue.m_ImageKinds = 1;
        m_TextboxDialogue.onText = true;
        yield return YieldInstructionCache.WaitForSeconds(4f);
        m_TextboxDialogue.onText = false;
        yield return YieldInstructionCache.WaitForSeconds(0.1f);
        m_TextboxDialogue.onText = true;

        yield return YieldInstructionCache.WaitForSeconds(2f);
        m_TextboxDialogue.onText = false;
        yield return YieldInstructionCache.WaitForSeconds(0.9f);
        m_SageiaAnimator.SetBool("isRadio", false);
        yield return YieldInstructionCache.WaitForSeconds(1.4f);
        m_TextboxDialogue.m_ImageKinds = 0;
        m_TextboxDialogue.onText = true;
        m_SageiaAnimator.SetBool("isLookAround", true);

        yield return YieldInstructionCache.WaitForSeconds(2f);
        m_TextboxDialogue.onText = false;

        m_EventFollow.transform.position += new Vector3(-3, 0);
        while (m_Derrick.position.x < 88)
        {
            m_Derrick.Translate(new Vector3(m_DerrickSpeed, 0) * Time.deltaTime);
            m_DerrickAnimator.SetBool("isWalk", true);
            m_DerrickSpriteRenderer.flipX = false;
            m_Ian.Translate(new Vector3(m_IanSpeed, 0) * Time.deltaTime);
            m_IanAnimator.SetBool("isWalk", true);
            m_IanSpriteRenderer.flipX = false;
            yield return YieldInstructionCache.WaitForFixedUpdate;
        }
        m_DerrickAnimator.SetBool("isWalk", false);
        StartTextBox("컷신2");
        m_TextboxDialogue.onText = true;


        while (m_Ian.position.x < 86)
        {
            m_Ian.Translate(new Vector3(m_IanSpeed, 0) * Time.deltaTime);
            m_IanAnimator.SetBool("isWalk", true);
            m_IanSpriteRenderer.flipX = false;
            yield return YieldInstructionCache.WaitForFixedUpdate;
        }
        m_IanAnimator.SetBool("isWalk", false);

        yield return YieldInstructionCache.WaitForSeconds(2f);
        m_TextboxDialogue.onText = false;
        m_SageiaAnimator.SetBool("isLookAround", false);
        yield return YieldInstructionCache.WaitForSeconds(0.5f);
        m_TextboxDialogue.onText = true;
        yield return YieldInstructionCache.WaitForSeconds(3f);
        m_TextboxDialogue.onText = false;
        yield return YieldInstructionCache.WaitForSeconds(0.01f);
        m_TextboxDialogue.onText = true;
        yield return YieldInstructionCache.WaitForSeconds(4.7f);
        m_TextboxDialogue.onText = false;
        yield return YieldInstructionCache.WaitForSeconds(0.01f);
        m_TextboxDialogue.onText = true;
        yield return YieldInstructionCache.WaitForSeconds(3.25f);
        m_TextboxDialogue.onText = false;
        yield return YieldInstructionCache.WaitForSeconds(0.01f);
        m_TextboxDialogue.onText = true;
        yield return YieldInstructionCache.WaitForSeconds(2.85f);
        m_TextboxDialogue.onText = false;


        yield return YieldInstructionCache.WaitForSeconds(0.5f);
        m_IanAnimator.SetBool("isTakeout", true);
        yield return YieldInstructionCache.WaitForSeconds(0.2f);
        m_TextboxDialogue.onText = true;
        yield return YieldInstructionCache.WaitForSeconds(5f);
        m_TextboxDialogue.onText = false;
        yield return YieldInstructionCache.WaitForSeconds(0.01f);
        m_TextboxDialogue.onText = true;
        yield return YieldInstructionCache.WaitForSeconds(3f);
        m_TextboxDialogue.onText = false;

        yield return YieldInstructionCache.WaitForSeconds(2);
        m_DerrickAnimator.SetTrigger("takebreath");
        yield return YieldInstructionCache.WaitForSeconds(0.5f);
        m_TextboxDialogue.onText = true;
        m_IanAnimator.SetBool("isTakeout", false);
        yield return YieldInstructionCache.WaitForSeconds(3.5f);
        m_TextboxDialogue.onText = false;
        yield return YieldInstructionCache.WaitForSeconds(0.35f);
        m_TextboxDialogue.onText = true;
        yield return YieldInstructionCache.WaitForSeconds(1.2f);
        m_TextboxDialogue.onText = false;
        yield return YieldInstructionCache.WaitForSeconds(0.2f);
        m_TextboxDialogue.onText = true;
        yield return YieldInstructionCache.WaitForSeconds(1.2f);
        m_TextboxDialogue.onText = false;
        yield return YieldInstructionCache.WaitForSeconds(0.5f);

        m_TextboxDialogue.onText = true;
        yield return YieldInstructionCache.WaitForSeconds(3.5f);
        m_TextboxDialogue.onText = false;
        yield return YieldInstructionCache.WaitForSeconds(0.01f);
        m_TextboxDialogue.onText = true;
        yield return YieldInstructionCache.WaitForSeconds(3.5f);
        m_TextboxDialogue.onText = false;
        yield return YieldInstructionCache.WaitForSeconds(0.01f);
        m_TextboxDialogue.onText = true;
        yield return YieldInstructionCache.WaitForSeconds(3f);
        m_TextboxDialogue.onText = false;
        yield return YieldInstructionCache.WaitForSeconds(0.01f);
        m_TextboxDialogue.onText = true;
        yield return YieldInstructionCache.WaitForSeconds(3.5f);
        m_TextboxDialogue.onText = false;
        yield return YieldInstructionCache.WaitForSeconds(0.01f);
        m_TextboxDialogue.onText = true;
        yield return YieldInstructionCache.WaitForSeconds(3.5f);
        m_TextboxDialogue.onText = false;
        yield return YieldInstructionCache.WaitForSeconds(0.01f);

        for (float i = 0; i < 1.75f; i += Time.deltaTime)
        {
            m_PlayerMove.isWalk = true;
            m_PlayerMove.m_CutX = 1;
            yield return YieldInstructionCache.WaitForFixedUpdate;
        }
        m_DerrickSpriteRenderer.flipX = true;
        while (m_PlayerMove.transform.position.x < 108)
        {
            m_Sageia.Translate(new Vector3(m_SageiaSpeed, 0) * Time.deltaTime);
            m_SageiaAnimator.SetBool("isWalk", true);
            m_SageiaSpriteRenderer.flipX = false;
            yield return YieldInstructionCache.WaitForFixedUpdate;
        }
        GameManager.GM.savePoint++;
        GameManager.GM.gameObject.GetComponent<DataManager>().JsonSave();
        Fade.instance.FadeIn(0.5f);
        RadioBox.isCalled = false;
        yield return YieldInstructionCache.WaitForSeconds(0.5f);

        SceneManager.LoadScene("Tutorial");
    }
}