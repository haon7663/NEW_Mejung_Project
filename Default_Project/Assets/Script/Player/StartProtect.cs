using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartProtect : MonoBehaviour
{
    private Move m_PlayerMove;
    private BoxCollider2D m_BoxCollider2D;

    public Vector3 size;
    public Vector3 offset;
    public LayerMask m_PlayerLayer;
    public bool isRight = true;

    private void Start()
    {
        m_BoxCollider2D = GetComponent<BoxCollider2D>();
        m_PlayerMove = GameObject.FindGameObjectWithTag("Player").GetComponent<Move>();
        StartCoroutine(CheckBox());
    }
    private IEnumerator CheckBox()
    {
        yield return YieldInstructionCache.WaitForFixedUpdate;
        var isHit = Physics2D.OverlapBox(transform.position + offset, size, 0, m_PlayerLayer);
        bool onHit = isHit;
        while (isHit)
        {
            m_PlayerMove.isCutScene = true;
            m_PlayerMove.isCalledScene = true;
            isHit = Physics2D.OverlapBox(transform.position + offset, size, 0, m_PlayerLayer);
            m_PlayerMove.isRun = true;
            m_PlayerMove.m_CutX = isRight ? 1 : -1;
            yield return YieldInstructionCache.WaitForFixedUpdate;
        }
        if(onHit)
        {
            for (float i = 0; i < 0.25f; i += Time.deltaTime)
            {
                m_PlayerMove.isCutScene = true;
                m_PlayerMove.isCalledScene = true;
                m_PlayerMove.isRun = true;
                m_PlayerMove.m_CutX = isRight ? 1 : -1;
                yield return YieldInstructionCache.WaitForFixedUpdate;
            }
        }
        m_BoxCollider2D.enabled = true;
        m_PlayerMove.isCutScene = false;
        m_PlayerMove.isCalledScene = false;
        m_PlayerMove.isRun = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(transform.position + offset, size);
    }
}
