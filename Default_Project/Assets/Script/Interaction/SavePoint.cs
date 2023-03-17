using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : MonoBehaviour
{
    private Move mPlayerMove;
    public int PointCount;

    private Interaction mInteraction;
    private void Start()
    {
        mPlayerMove = GameObject.FindGameObjectWithTag("Player").GetComponent<Move>();
        mInteraction = GameObject.FindGameObjectWithTag("Player").GetComponent<Interaction>();

        if (GameManager.GM.savePoint == PointCount)
        {
            mPlayerMove.mCheckPoint = transform;
            mPlayerMove.gameObject.transform.position = transform.position;
        }
    }

    public void Save()
    {
        mPlayerMove.mCheckPoint = transform;
        GameManager.GM.savePoint = PointCount;
        GameManager.GM.gameObject.GetComponent<DataManager>().JsonSave();
    }
    public void Interactions()
    {
        Save();
    }
}
