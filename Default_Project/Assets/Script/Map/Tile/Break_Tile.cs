using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Break_Tile : MonoBehaviour
{
    private GameObject mPlayer;
    private Move mPlayerMove;
    private BoxCollider2D BCOL;

    private void Start()
    {
        mPlayer = GameObject.FindGameObjectWithTag("Player");
        mPlayerMove = mPlayer.GetComponent<Move>();
        BCOL = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if (mPlayerMove.isDash)
            BCOL.isTrigger = true;
        else
            BCOL.isTrigger = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            if (collision.transform.GetComponent<Move>().isDash)
            {
                Destroy(gameObject);
            }
        }
    }
}
