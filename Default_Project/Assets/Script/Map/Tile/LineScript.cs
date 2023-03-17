using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineScript : MonoBehaviour
{
    public Transform beginPos;
    public Transform endPos;

    private EdgeCollider2D ECOL;

    Vector3 beginPosOffset;
    Vector3 endPosOffset;

    LineRenderer diagLine;

    void Start()
    {
        diagLine = GetComponent<LineRenderer>();
        diagLine.startColor = diagLine.endColor = Color.red;
        diagLine.startWidth = diagLine.endWidth = 0.3f;

        diagLine.SetPosition(0, beginPos.localPosition);
        diagLine.SetPosition(1, endPos.localPosition);

        ECOL = GetComponent<EdgeCollider2D>();
    }

    void Update()
    {
        //Calculate new postion 
        Vector3 newBeginPos = transform.localToWorldMatrix * new Vector4(beginPos.localPosition.x, beginPos.localPosition.y, beginPos.localPosition.z, 1);
        Vector3 newEndPos = transform.localToWorldMatrix * new Vector4(endPos.localPosition.x, endPos.localPosition.y, endPos.localPosition.z, 1);

        //Apply new position
        diagLine.SetPosition(0, newBeginPos);
        diagLine.SetPosition(1, newEndPos);
    }
}
