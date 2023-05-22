using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GetSteam_Event : SceneEvent
{
    public CinemachineVirtualCamera cinevirtual;
    public CinemachineConfiner2D mCinemachineConfiner;

    private RadioDialogue m_RadioDialogue;
    private Move m_PlayerMove;
    public Transform m_EventFollow;
    public string eventName;

    private void Start()
    {
        m_RadioDialogue = GameObject.FindGameObjectWithTag("Dialogue").GetComponent<RadioDialogue>();
        m_PlayerMove = GameObject.FindGameObjectWithTag("Player").GetComponent<Move>();
    }
    public override void Event()
    {
        StartCoroutine(PlayEvent());
    }

    public void StartRadio()
    {
        m_RadioDialogue.StartDialogue(eventName);
    }

    public IEnumerator PlayEvent()
    {
        m_PlayerMove.isCutScene = true;
        yield return YieldInstructionCache.WaitForSeconds(0.7f);
        cinevirtual.Follow = m_EventFollow;
        yield return YieldInstructionCache.WaitForSeconds(2f);
        cinevirtual.Follow = m_PlayerMove.transform;
        yield return YieldInstructionCache.WaitForSeconds(0.7f);

        StartRadio();

        GameManager.GM.onRadio = true;
        while (GameManager.GM.onRadio)
        {
            yield return YieldInstructionCache.WaitForFixedUpdate;
        }

        while (m_PlayerMove.transform.position.x >= 37.4f)
        {
            m_PlayerMove.CutWalk(true);
            yield return YieldInstructionCache.WaitForFixedUpdate;
        }
        m_PlayerMove.CutWalk(false);
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