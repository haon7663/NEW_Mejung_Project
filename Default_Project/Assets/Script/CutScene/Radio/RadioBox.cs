using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioBox : SceneEvent
{
    private RadioDialogue m_RadioDialogue;
    public string eventName;
    private Move m_Player;

    public bool canSetSize;
    public float camSize;

    private void Start()
    {
        m_Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Move>();
        m_RadioDialogue = GameObject.FindGameObjectWithTag("Dialogue").GetComponent<RadioDialogue>();
    }

    public override void Event()
    {
        StartCoroutine(StartRadio());
    }
    public IEnumerator StartRadio()
    {
        if (canSetSize) m_Player.CinemacineSize = camSize;
        m_Player.isCutScene = true;
        m_Player.isCalledScene = true;
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
