using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutSceneManager : MonoBehaviour
{
    private Move m_PlayerMove;

    private void Start()
    {
        m_PlayerMove = GetComponent<Move>();
    }
    public void CutScene()
    {
        m_PlayerMove.isCutScene = true;
        m_PlayerMove.isCalledScene = true;
    }
}
