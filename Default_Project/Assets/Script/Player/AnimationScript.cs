using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationScript : MonoBehaviour
{
    private Animator AN;
    private Move MV;
    private Collison COL;
    [HideInInspector]
    public SpriteRenderer SR;

    void Start()
    {
        AN = GetComponent<Animator>();
        COL = GetComponentInParent<Collison>();
        MV = GetComponentInParent<Move>();
        SR = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        AN.SetBool("onGround", COL.onGround);
        AN.SetBool("onWall", COL.onWall);
        AN.SetBool("isDashing", MV.isANDash);
        AN.SetBool("isClim", MV.isSlide);
        AN.SetBool("isSteamDash", MV.isSteamDash);
        AN.SetBool("isSpring", MV.isSpring);
        AN.SetBool("isSlope", COL.onSlope);
        //AN.SetBool("onRightWall", COL.onRightWall);
        //AN.SetBool("wallGrab", move.wallGrab);
        //AN.SetBool("canMove", move.canMove);
    }

    public void SetHorizontalMovement(float x, float y, float yVel, float xRaw)
    {
        AN.SetFloat("HorizontalAxis", x);
        AN.SetFloat("VerticalAxis", y);
        AN.SetFloat("VerticalVelocity", yVel);
        AN.SetBool("isHorizontal", xRaw != 0);
    }
}
