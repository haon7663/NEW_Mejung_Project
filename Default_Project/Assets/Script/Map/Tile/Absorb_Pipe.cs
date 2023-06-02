using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Absorb_Pipe : MonoBehaviour
{
    public Vector3 plusPos;

    private Move mPlayerMove;
    private Rigidbody2D mPlayerRB;

    private void Start()
    {
        mPlayerMove = GameObject.FindGameObjectWithTag("Player").GetComponent<Move>();
        mPlayerRB = mPlayerMove.GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        Collider2D Absorb = Physics2D.OverlapBox(transform.position + plusPos, new Vector2(1f, 1f), 0, 1 << LayerMask.NameToLayer("Player"));

        if (Absorb && !mPlayerMove.isPipe)
        {
            if((transform.rotation.eulerAngles.z == 0 && mPlayerRB.velocity.y <= 0) || (transform.rotation.eulerAngles.z == 90 && mPlayerRB.velocity.x >= 0)
                || (transform.rotation.eulerAngles.z == 180 && mPlayerRB.velocity.y >= 0) || (transform.rotation.eulerAngles.z == 270 && mPlayerRB.velocity.x <= 0)) StartCoroutine(mPlayerMove.PipeMove(plusPos, transform.position + plusPos));
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + plusPos, new Vector2(1f, 1f));
    }
}
