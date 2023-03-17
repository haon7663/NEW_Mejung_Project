using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Release_Pipe : MonoBehaviour
{
    private Move mPlayerMove;
    private Vector2 SetVec;

    private void Start()
    {
        mPlayerMove = GameObject.FindGameObjectWithTag("Player").GetComponent<Move>();
    }
    private void Update()
    {
        float Ang = transform.rotation.eulerAngles.z;
        Collider2D Release;
        Release = Physics2D.OverlapBox(transform.position + new Vector3((Ang == 90 ? -3.3f : Ang == 270 ? 3.3f : 0), (Ang == 180 ? -3.3f : Ang == 0 ? 3.3f : 0)), new Vector2(Ang == 0 || Ang == 180 ? 3 : 6, Ang == 0 || Ang == 180 ? 6 : 3), 0, 1 << LayerMask.NameToLayer("Player"));
        if (Release) mPlayerMove.Floating(new Vector2(Ang == 0 || Ang == 180 ? 0 : Ang == 90 ? -1 : 1, Ang == 90 || Ang == 270 ? 0.8f : Ang == 0 ? 1 : -1));
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (transform.rotation.eulerAngles.z == 0) Gizmos.DrawWireCube(transform.position + new Vector3(0, 3.3f, 0), new Vector2(3f, 6f));
        else if (transform.rotation.eulerAngles.z == 90) Gizmos.DrawWireCube(transform.position + new Vector3(-3.3f, 0, 0), new Vector2(6f, 3f));
        else if (transform.rotation.eulerAngles.z == 180) Gizmos.DrawWireCube(transform.position + new Vector3(0, -3.3f, 0), new Vector2(3f, 6f));
        else Gizmos.DrawWireCube(transform.position + new Vector3(3.3f, 0, 0), new Vector2(6f, 3f));
    }
}