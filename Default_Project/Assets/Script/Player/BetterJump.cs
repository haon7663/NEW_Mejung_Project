using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetterJump : MonoBehaviour
{
    private Rigidbody2D RB;
    private Move mPlayerMove;

    //__________________________//

    [SerializeField] private float mFallMultiplier;
    [SerializeField] private float mLowJumpMultiplier;

    //__________________________//

    private void Start()
    {
        RB = GetComponent<Rigidbody2D>();
        mPlayerMove = GetComponent<Move>();
    }

    private void Update()
    {
        if(!mPlayerMove.isANDash)
        {
            if (RB.velocity.y < 0)
            {
                RB.velocity += Vector2.up * Physics2D.gravity.y * (mFallMultiplier - 1) * Time.deltaTime;
            }
            else if (RB.velocity.y > 0)
            {
                RB.velocity += Vector2.up * Physics2D.gravity.y * (mLowJumpMultiplier - 1) * Time.deltaTime;
            }
        }
    }
}
