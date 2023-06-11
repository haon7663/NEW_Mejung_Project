using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveScene : MonoBehaviour
{
    private Transform m_Player;
    private Move m_PlayerMove;

    public int SceneCount;
    public Vector2 SetPostion;

    public bool isRight;

    private bool isCalled = false;

    private void Start()
    {
        m_Player = GameObject.FindGameObjectWithTag("Player").transform;
        m_PlayerMove = m_Player.GetComponent<Move>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isCalled)
        {
            StartCoroutine(InvokeLoad(1f));
        }
    }

    private IEnumerator InvokeLoad(float time)
    {
        isCalled = true;
        GameManager.GM.savePoint++;
        GameManager.GM.gameObject.GetComponent<DataManager>().JsonSave();
        m_PlayerMove.isCutScene = true;
        m_PlayerMove.isWalk = true;
        m_PlayerMove.m_CutX = isRight ? 1 : -1;
        for (float i = 0; i < time; i += Time.deltaTime)
        {
            yield return YieldInstructionCache.WaitForFixedUpdate;
        }
        Fade.instance.FadeIn(time);
        for (float i = 0; i < time; i += Time.deltaTime)
        {
            yield return YieldInstructionCache.WaitForFixedUpdate;
        }
        m_Player.position = SetPostion;
        SceneManager.LoadScene(SceneCount);
    }
}
