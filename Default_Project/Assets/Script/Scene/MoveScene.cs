using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveScene : MonoBehaviour
{
    private Transform mPlayer;

    public int SceneCount;
    public Vector2 SetPostion;

    private void Start()
    {
        mPlayer = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            mPlayer.position = SetPostion;
            SceneManager.LoadScene(SceneCount);
        }
    }
}
