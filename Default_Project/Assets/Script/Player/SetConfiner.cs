using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class SetConfiner : MonoBehaviour
{
    [Header("Layers")]
    public LayerMask confiner_Layer;
    public LayerMask sizeConfiner_Layer;
    public LayerMask posConfiner_Layer;
    public LayerMask keySetting_Layer;

    public CinemachineConfiner2D mCinemachineConfiner;

    private Move mPlayerMove;
    private float mLastSize;
    public float m_DefaultSize;

    private void Start()
    {
        mPlayerMove = GetComponent<Move>();
        enabled = false;
    }
    private void Update()
    {
        var collison_Confiner = Physics2D.OverlapBox(transform.position, new Vector2(0.1f, 2f), 0, confiner_Layer);
        if (collison_Confiner) mCinemachineConfiner.m_BoundingShape2D = collison_Confiner;

        var size_Confiner = Physics2D.OverlapBox(transform.position, new Vector2(1f, 1f), 0, sizeConfiner_Layer);
        if (mPlayerMove.isDeath) return;
        if(!mPlayerMove.isCutScene)
        {
            if (size_Confiner)
            {
                mPlayerMove.CinemacineSize = size_Confiner.transform.position.z;
                mPlayerMove.mCameraSize = size_Confiner.transform.position.z;
            }
            else if (!size_Confiner && !mPlayerMove.isCutScene)
            {
                mPlayerMove.CinemacineSize = m_DefaultSize;
                mPlayerMove.mCameraSize = m_DefaultSize;
            }
        }

        var pos_Confiner = Physics2D.OverlapBox(transform.position, new Vector2(1f, 1f), 0, posConfiner_Layer);
        if (pos_Confiner)
        {
            mPlayerMove.m_TargetPlus = new Vector3(0, pos_Confiner.transform.position.z);
        }
        else
        {
            mPlayerMove.m_TargetPlus = new Vector3(0, 0);
        }

        var key_Setter = Physics2D.OverlapBox(transform.position, new Vector2(1f, 1f), 0, keySetting_Layer);
        if (key_Setter) InventoryManager.IM.maxKey = (int)key_Setter.transform.position.z;

        if (mPlayerMove.real_CineSize != mLastSize)
        {
            mCinemachineConfiner.InvalidateCache(); 
            mLastSize = mPlayerMove.real_CineSize;
        }
    }
}