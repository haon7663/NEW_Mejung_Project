using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectRay : MonoBehaviour
{
    private Rigidbody2D m_Rigidbody2D;

    public Vector2 reflect;
    public int reflections;
    public float MaxRayDistance;
    public LayerMask LayerDetection;

    public RaycastHit2D hitInfo;
    public RaycastHit2D realhitInfo;

    Vector2 LastVelocity;

    private void Start()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        Physics2D.queriesStartInColliders = false;
    }

    private void Update()
    {
        LastVelocity = m_Rigidbody2D.velocity;
        float x = LastVelocity.x > 0 ? 1 : -1;
        float y = LastVelocity.y > 0 ? 1 : -1;

        float Revert_X = LastVelocity.x == 0 ? 0 : LastVelocity.x * (y / LastVelocity.y);
        float Revert_Y = LastVelocity.y == 0 ? 0 : LastVelocity.y * (x / LastVelocity.x);

        if (Mathf.Abs(LastVelocity.x) > Mathf.Abs(LastVelocity.y))
        {
            LastVelocity = new Vector3(x, Revert_Y);
        }
        else if (Mathf.Abs(LastVelocity.x) <= Mathf.Abs(LastVelocity.y))
        {
            LastVelocity = new Vector3(Revert_X, y);
        }
        LastVelocity = LastVelocity.normalized;

        hitInfo = Physics2D.Raycast(transform.position, LastVelocity, MaxRayDistance, LayerDetection);
        realhitInfo = Physics2D.Raycast(transform.position, LastVelocity, 1.5f, LayerDetection);
        Debug.DrawRay(transform.position, LastVelocity * MaxRayDistance);


        if (hitInfo.collider != null)
        {
            if (hitInfo.collider.CompareTag("Boost"))
            {
                reflect = (Vector2.Reflect(LastVelocity, hitInfo.normal) * Mathf.Max(250, 0f));
                hitInfo = Physics2D.Raycast((Vector2)hitInfo.point, reflect, MaxRayDistance, LayerDetection);
                Debug.DrawRay((Vector2)hitInfo.point, reflect * MaxRayDistance);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position + new Vector3(0, 0.05f), LastVelocity * 1.5f);
    }
}
