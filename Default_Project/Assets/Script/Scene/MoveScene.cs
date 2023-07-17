using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveScene : MonoBehaviour
{
    private Transform m_Player;
    private Move m_PlayerMove;

    public string SceneCount;
    public Vector2 SetPostion;

    public bool isRight;
    private bool isCalled = false;

    private void Start()
    {
        isCalled = false;
        m_Player = GameObject.FindGameObjectWithTag("Player").transform;
        m_PlayerMove = m_Player.GetComponent<Move>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isCalled)
        {
            GameManager.GM.gameObject.GetComponent<DataManager>().JsonSave();
            m_PlayerMove.isCutScene = true;
            m_PlayerMove.isCalledScene = true;
            m_PlayerMove.isWalk = true;
            m_PlayerMove.m_CutX = isRight ? 1 : -1;
            StartCoroutine(InvokeLoad(1f));
            isCalled = true;
        }
    }

    private IEnumerator InvokeLoad(float time)
    {
        GameManager.GM.savePoint++;
        GameManager.GM.gameObject.GetComponent<DataManager>().JsonSave();
        m_PlayerMove.isCutScene = true;
        m_PlayerMove.isCalledScene = true;
        m_PlayerMove.isWalk = true;
        m_PlayerMove.m_CutX = isRight ? 1 : -1;
        for (float i = 0; i < time; i += Time.deltaTime)
        {
            yield return YieldInstructionCache.WaitForFixedUpdate;
        }
        Fade.instance.FadeIn(time);
        RadioBox.isCalled = false;
        for (float i = 0; i < time; i += Time.deltaTime)
        {
            yield return YieldInstructionCache.WaitForFixedUpdate;
        }
        SceneManager.LoadScene(SceneCount);
    }
}
