using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveScene : MonoBehaviour
{
    private Transform mPlayer;
    private Move mPlayerMove;

    public int SceneCount;
    public Vector2 SetPostion;

    public bool isRight;
    public bool isWalk;

    private void Start()
    {
        mPlayer = GameObject.FindGameObjectWithTag("Player").transform;
        mPlayerMove = mPlayer.GetComponent<Move>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Fade.instance.FadeIn(0.95f);
            Invoke(nameof(InvokeLoad), 1);

            if(isWalk)
            {
                mPlayerMove.isInputMove = true;
                mPlayerMove.x = (isRight ? 1 : -1);
            }
        }
    }

    private void InvokeLoad()
    {
        mPlayerMove.isInputMove = false;
        mPlayer.position = SetPostion;
        SceneManager.LoadScene(SceneCount);
    }
}
