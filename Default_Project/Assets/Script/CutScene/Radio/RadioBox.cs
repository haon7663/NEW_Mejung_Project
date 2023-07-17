using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioBox : SceneEvent
{
    public static bool isCalled = false;

    private RadioDialogue m_RadioDialogue;
    public string eventName;
    private Move m_Player;
    private Animator m_PlayerAnimator;

    public bool canSetSize;
    public bool isPlayAnimation;
    public string m_AnimationString;
    public int m_Animatortime;
    public float camSize;

    private void Start()
    {
        m_Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Move>();
        m_PlayerAnimator = m_Player.GetComponent<Animator>();
        m_RadioDialogue = GameObject.FindGameObjectWithTag("Dialogue").GetComponent<RadioDialogue>();
    }

    public override void Event()
    {
        if(!isCalled)
        {
            isCalled = true;
            StartCoroutine(StartRadio());
        }
    }
    public IEnumerator StartRadio()
    {
        if (canSetSize) m_Player.CinemacineSize = camSize;
        m_Player.isCutScene = true;
        m_Player.isCalledScene = true;
        if (isPlayAnimation)
        {
            m_PlayerAnimator.SetBool(m_AnimationString, true);
            yield return YieldInstructionCache.WaitForSeconds(m_Animatortime);
            m_PlayerAnimator.SetBool(m_AnimationString, false);
        }
        m_RadioDialogue.StartDialogue(eventName);
        GameManager.GM.onRadio = true;
        while (GameManager.GM.onRadio)
        {
            yield return YieldInstructionCache.WaitForFixedUpdate;
        }
        m_Player.isCutScene = false;
        m_Player.isCalledScene = false;
        Destroy(gameObject);
    }
}
