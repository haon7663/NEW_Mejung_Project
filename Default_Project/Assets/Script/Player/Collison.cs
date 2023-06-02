using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collison : MonoBehaviour
{
    [Header("Layers")]
    public LayerMask groundLayer;
    public LayerMask interruptLayer;

    [Space]

    public bool onGround;
    public bool onWall;
    public bool onRightWall;
    public bool onLeftWall;
    public bool onDashRightWall;
    public bool onDashLeftWall;
    public bool onSlopeRightWall;
    public bool onSlopeLeftWall;
    public bool onSlope;
    public int wallSide;

    [Space]

    [Header("Collision")]

    public float collisionRadius = 0.25f;
    public float dashCollisionRadius = 0.25f;
    public Vector2 bottomOffset, rightOffset, leftOffset, rightDashOffset, leftDashOffset;
    private Color debugCollisionColor = Color.red;

    private Move mPlayerMove;
    private CapsuleCollider2D mCapsuleCollider2D;
    private Rigidbody2D mRigidbody2D;

    public float slopeAngle;

    private void Start()
    {
        mCapsuleCollider2D = GetComponent<CapsuleCollider2D>();
        mRigidbody2D = GetComponent<Rigidbody2D>();
        mPlayerMove = GetComponent<Move>();
    }

    private void Update()
    {
        rightOffset = mPlayerMove.isDash ? new Vector2(1f, 0) : new Vector2(0.5f, 0.4f);
        leftOffset = mPlayerMove.isDash ? new Vector2(-1f, 0) : new Vector2(-0.5f, 0.4f);
        onGround = Physics2D.OverlapCircle((Vector2)transform.position + bottomOffset, collisionRadius*5f, groundLayer + interruptLayer);
        onWall = Physics2D.OverlapCircle((Vector2)transform.position + rightOffset, collisionRadius, groundLayer)
            || Physics2D.OverlapCircle((Vector2)transform.position + leftOffset, collisionRadius, groundLayer);

        onRightWall = Physics2D.OverlapCircle((Vector2)transform.position + rightOffset, collisionRadius, groundLayer);
        onLeftWall = Physics2D.OverlapCircle((Vector2)transform.position + leftOffset, collisionRadius, groundLayer);

        onDashRightWall = Physics2D.OverlapCircle((Vector2)transform.position + rightDashOffset, dashCollisionRadius, groundLayer);
        onDashLeftWall = Physics2D.OverlapCircle((Vector2)transform.position + leftDashOffset, dashCollisionRadius, groundLayer);

        onSlopeRightWall = Physics2D.OverlapCircle((Vector2)transform.position + new Vector2(rightOffset.x, 0), 0.65f, groundLayer);
        onSlopeLeftWall = Physics2D.OverlapCircle((Vector2)transform.position + new Vector2(leftOffset.x, 0), 0.65f, groundLayer);

        wallSide = onRightWall ? -1 : 1;

        if (onSlope && mPlayerMove.LastCollision.contacts.Length <= 0) onSlope = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Boost"))
        {
            slopeAngle = Vector2.Angle(collision.contacts[0].normal, Vector2.up);
            mPlayerMove.LastCollision = collision;
            if (slopeAngle > 40 && slopeAngle < 50 && mPlayerMove.LastCollision.contacts.Length > 0)
            {
                onSlope = true;
                mPlayerMove.SetSlopeCamera();
            }
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Boost"))
        {
            slopeAngle = Vector2.Angle(collision.contacts[0].normal, Vector2.up);
            mPlayerMove.LastCollision = collision;
            if (slopeAngle > 40 && slopeAngle < 50 && mPlayerMove.LastCollision.contacts.Length > 0)
            {
                onSlope = true;
            }
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Boost"))
        {
            onSlope = false;
        }
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        var positions = new Vector2[] { bottomOffset, rightOffset, leftOffset };

        Gizmos.DrawWireSphere((Vector2)transform.position + bottomOffset, collisionRadius * 5f);

        Gizmos.DrawWireSphere((Vector2)transform.position + rightOffset, collisionRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + leftOffset, collisionRadius);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere((Vector2)transform.position + new Vector2(rightOffset.x, 0), 0.65f);
        Gizmos.DrawWireSphere((Vector2)transform.position + new Vector2(leftOffset.x, 0), 0.65f);
    }
}
