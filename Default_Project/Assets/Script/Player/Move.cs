using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cinemachine;

public class Move : MonoBehaviour
{
    public float GameTime = 1;

    //__________________________//

    public CinemachineVirtualCamera cinevirtual;
    public CinemachineConfiner2D mCinemachineConfiner;
    private CinemachineTransposer mCinemachineTransposer;
    public float mCameraSize = 10;
    public float CinemacineSize = 10;
    public float real_CineSize = 10;

    private Rigidbody2D RB;
    private SpriteRenderer SR;
    private CapsuleCollider2D CPCOL;
    private Animator AN;

    //__________________________//

    private Collison COL;
    private Death DIE;
    private AnimationScript Player_Animation;

    //__________________________//

    public CapsuleCollider2D mHitBox;

    [Space]
    [Header("Sprite")]
    public Sprite[] mSprite = new Sprite[2];

    [Space]
    [Header("Prefab")]
    [SerializeField] private GameObject mAfterImage;
    [SerializeField] private GameObject mAimObject;
    [SerializeField] private GameObject mDeathParticle;

    [Space]
    [Header("Velocity")]
    [SerializeField] private float mMoveSpeed;
    [SerializeField] private float mJumpPower;
    [SerializeField] private float mSlideSpeed;
    [SerializeField] private float mWallJumpLerp;
    [SerializeField] private float mPipelineSpeed;
    [SerializeField] private float maxSpeed;

    [Space]
    [Header("XY")]
    public float x;
    public float y;
    public float xRaw;
    public float yRaw;
    public float Resistance_MovetileSpeed;
    public float AddMaxSpeed;
    public float m_CutX = 0;

    [Space]
    [Header("CheckPoint")]
    public Transform mCheckPoint;

    [Space]
    [Header("HitBox")]
    public Vector2 mHitSize;

    [Space]
    [Header("Bool")]
    public bool isCutScene;
    public bool isSlide;
    public bool isWallJump;
    public bool isCanMove = true;
    public bool isWalk = false;
    public bool isRun = false;
    public bool isDash = false;
    public bool isANDash = false;
    public bool haveDash = true;
    public bool isMoveGround = false;
    public bool isPipe = false;
    public bool isInteraction = false;
    public bool isSteamDash = false;
    public bool isSpring = false;
    public bool haveSteamDash = false;
    public bool isSetCameraSize;
    public bool isDeath = false;

    private bool isMove = false;
    private bool canWallSlide;

    private float KeyBreakTime;
    private bool isKeyBreak = false;
    private Vector2 KeyBreakXY;

    //__________________________//

    private float Timer;
    private float CollisonTime;
    private float DashTime;
    private float mANDahsTime;
    private float PushTime;
    private float springTime;
    private Vector3 LastVelocity;

    private Transform MainCamera;
    private Transform mLastMainTransform;

    public Vector3 setPos;

    public SpriteRenderer m_SteamMachine;
    public Sprite m_ChangeSteamMachine;
    public GameObject FallDustEffect;
    public GameObject JumpDustEffect;
    public GameObject SteamDustEffect;

    [Space]
    [Header("Death")]
    public GameObject MapConfiner;
    public GameObject DeathConfiner;
    public UnityEngine.Rendering.Universal.Light2D mBackGroundLight;
    public UnityEngine.Rendering.Universal.Light2D mPlatformLight;

    private static Move instance;

    private void Awake()
    {
        /*if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);*/
    }

    private void Start()
    {
        Time.timeScale = GameTime;
        RB = GetComponent<Rigidbody2D>();
        SR = GetComponent<SpriteRenderer>();
        CPCOL = GetComponent<CapsuleCollider2D>();
        COL = GetComponent<Collison>();
        DIE = GetComponent<Death>();
        AN = GetComponent<Animator>();

        Player_Animation = GetComponent<AnimationScript>();

        mHitBox.size = new Vector2(1 * mHitSize.x, 1.875f * mHitSize.y);

        MainCamera = Camera.main.transform;
        mCinemachineTransposer = cinevirtual.GetCinemachineComponent<CinemachineTransposer>();
    }

    internal static class YieldInstructionCache
    {
        public static readonly WaitForEndOfFrame WaitForEndOfFrame = new WaitForEndOfFrame();
        public static readonly WaitForFixedUpdate WaitForFixedUpdate = new WaitForFixedUpdate();
        private static readonly Dictionary<float, WaitForSeconds> waitForSeconds = new Dictionary<float, WaitForSeconds>();

        public static WaitForSeconds WaitForSeconds(float seconds)
        {
            WaitForSeconds wfs;
            if (!waitForSeconds.TryGetValue(seconds, out wfs))
                waitForSeconds.Add(seconds, wfs = new WaitForSeconds(seconds));
            return wfs;
        }
    }

    private void Update()
    {
        if (GameManager.GM.onPause) return;

        mLastMainTransform = MainCamera;
        real_CineSize = Mathf.Lerp(real_CineSize, CinemacineSize, Time.deltaTime * 3);
        cinevirtual.m_Lens.OrthographicSize = real_CineSize;

        if (isDeath)
        {
            transform.SetParent(null);
            CinemacineSize = 6;
            return;
        }
        if (isPipe || isInteraction)
        {
            isDash = false;
            AN.SetBool("run", false);
            x = 0; xRaw = 0; y = 0; yRaw = 0;
            return;
        }

        //_____________________________//

        x = Input.GetAxis("Horizontal");
        y = Input.GetAxis("Vertical");
        xRaw = Input.GetAxisRaw("Horizontal");
        yRaw = Input.GetAxisRaw("Vertical");

        //_____________________________//

        if (Input.GetKeyDown(KeySetting.keys[KeyAction.JUMP]) && !isCutScene) Jump();
        if (Input.GetKeyDown(KeySetting.keys[KeyAction.STEAM]) && isANDash && haveSteamDash) StartCoroutine(SteamDash(xRaw, yRaw));
        mAimObject.SetActive(COL.onSlope);
        if (COL.onSlope && !isANDash)
        {
            float xraw = setRaw().x;
            float yraw = setRaw().y;
            float angle = Mathf.Atan2(yraw, xraw) * Mathf.Rad2Deg;
            if (xraw == 0 && yraw == 0) angle = Mathf.Atan2(LastCollision.contacts[0].normal.y, LastCollision.contacts[0].normal.x) * Mathf.Rad2Deg;
            Quaternion angleAxis = Quaternion.AngleAxis(angle - 90f, Vector3.forward);
            Quaternion rotation = Quaternion.Slerp(mAimObject.transform.rotation, angleAxis, 25 * Time.deltaTime);
            mAimObject.transform.rotation = rotation;
        }

        if(isKeyBreak && !isCutScene)
        {
            if (!COL.onGround && (xRaw != 0 || yRaw != 0) && DashTime <= 0 && (((COL.onRightWall && xRaw == -1) || (COL.onLeftWall && xRaw == 1)) || !COL.onWall))
            {
                isKeyBreak = false;
                Dash(KeyBreakXY.x, KeyBreakXY.y);
            }
        }
        if (Input.GetKeyDown(KeySetting.keys[KeyAction.DASH]) && haveDash && !isCutScene)
        {
            if (COL.onSlope) ChargeDash();
            else if (!COL.onGround && (xRaw != 0 || yRaw != 0) && DashTime <= 0 && (((COL.onRightWall && xRaw == -1) || (COL.onLeftWall && xRaw == 1)) || !COL.onWall)) Dash(xRaw, yRaw);
        }
        if (KeyBreakTime > 0 && Input.GetKeyDown(KeySetting.keys[KeyAction.DASH]))
        {
            isKeyBreak = true;
            KeyBreakXY = new Vector2(xRaw, yRaw);
        }


        //_____________________________//

        CollisonTime -= Time.deltaTime;
        KeyBreakTime -= Time.deltaTime;
        DashTime -= Time.deltaTime;
        Timer += Time.deltaTime;
        mANDahsTime -= Time.deltaTime;
        PushTime -= Time.deltaTime;

        //_____________________________//

        if (RB.velocity.x > 0.02f) SR.flipX = false;
        else if (RB.velocity.x < -0.02f) SR.flipX = true;

        if (isDash) mANDahsTime = 0.2f;
        if (mANDahsTime > 0) isANDash = true;
        else if (mANDahsTime <= 0) isANDash = false;

        //_____________________________//

        if (transform.position.y < -20) Death();

        LastVelocity = RB.velocity;

        if (COL.onWall) WallSlide();
        else if (!COL.onWall || COL.onGround) ElseWallSlide();
        if (COL.onSlope && !isANDash) WallSlope();
    }
    private void FixedUpdate()
    {
        if (GameManager.GM.onPause) return;

        if (isDeath) return;
        if (isInteraction) RB.velocity = new Vector2(0, RB.velocity.y);
        if (isPipe || isInteraction || COL.onSlope) return;

        Player_Animation.SetHorizontalMovement(x, y, RB.velocity.y, xRaw); Walk();
    }

    private void Walk()
    {
        if(isCutScene)
        {
            AN.SetBool("run", isRun);
            AN.SetBool("isWalk", isWalk);
            if (isWalk) RB.velocity = new Vector2(m_CutX * maxSpeed * 0.3f, RB.velocity.y);
            else if(isRun) RB.velocity = new Vector2(m_CutX * maxSpeed, RB.velocity.y);
            else RB.velocity = new Vector2(0, RB.velocity.y);
            return;
        }
        if (!isCanMove || isDash || CollisonTime > 0)
            return;

        AN.SetBool("run", Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D));
         
        if (isWallJump)
        {
            RB.velocity = Vector2.Lerp(RB.velocity, (new Vector2(xRaw * mMoveSpeed * 0.2f * Time.fixedDeltaTime, RB.velocity.y)), mWallJumpLerp * Time.deltaTime);
        }
        else
        {
            //Debug.Log("VelocityX : " + RB.velocity.x + " / maxSpeed : " + maxSpeed);
            if (springTime > 0)
            {
                isSpring = true;
                springTime -= Time.deltaTime;
                if (springTime <= 0) isSpring = false;
                RB.velocity = Vector2.Lerp(RB.velocity, new Vector2(xRaw * mMoveSpeed * 0.2f * Time.fixedDeltaTime, RB.velocity.y), Time.deltaTime * 3);
            }
            else if ((xRaw == 0 && PushTime < 0) || COL.onWall)
            {
                RB.velocity = new Vector2(x * maxSpeed, RB.velocity.y);
            }
            else if ((MathF.Abs(RB.velocity.x) < maxSpeed || PlayerFlip()) || (PushTime >= 0 && MathF.Abs(RB.velocity.x) < maxSpeed))
            {
                RB.AddForce(new Vector2(xRaw * mMoveSpeed * Time.fixedDeltaTime, 0));
                RB.velocity -= new Vector2(Resistance_MovetileSpeed, 0);
            }
            else if (PushTime >= 0)
            {
                RB.AddForce(new Vector2(xRaw * mMoveSpeed * Time.fixedDeltaTime * 0.1f, 0));
            }
            else if (!isDash && !isSteamDash)
            {
                CPCOL.size = new Vector2(1, 1.875f);
                mHitBox.size = new Vector2(1 * mHitSize.x, 1.875f * mHitSize.y);
                RB.velocity = new Vector2(x * maxSpeed, RB.velocity.y);
            }
        }
    }

    private void Jump()
    {
        canWallSlide = false;
        AN.SetTrigger("jump");
        if (COL.onSlope)
        {
            StopCoroutine(DisableMovement(0));
            StartCoroutine(DisableMovement(.1f));

            Vector2 wallDir = COL.onSlopeRightWall ? Vector2.left : Vector2.right;

            RB.velocity = new Vector2(RB.velocity.x, 0);
            RB.velocity += (Vector2.up / 1.5f + wallDir / 1f) * mJumpPower;

            isWallJump = true;
        }
        else if (COL.onGround)
        {
            Instantiate(JumpDustEffect, transform.position, Quaternion.identity);
            RB.velocity = new Vector2(RB.velocity.x, 0);
            RB.velocity += Vector2.up * mJumpPower;
            COL.onSlope = false;
        }
        else if (COL.onWall && !COL.onGround)
        {
            StopCoroutine(DisableMovement(0));
            StartCoroutine(DisableMovement(.1f));

            Vector2 wallDir = COL.onRightWall ? Vector2.left : Vector2.right;

            RB.velocity = new Vector2(RB.velocity.x, 0);
            RB.velocity += (Vector2.up / 1.5f + wallDir / 1f) * mJumpPower;

            isWallJump = true;
        }
    }

    private IEnumerator SteamDash(float x, float y)
    {
        DashTime = 0.1f;
        AN.SetTrigger("steamdash");

        GameObject dust = Instantiate(SteamDustEffect, transform.position, Quaternion.identity);
        dust.transform.localScale = new Vector3(x == 0 ? (SR.flipX ? -1 : 1) : (x < 0 ? -1 : 1), 1);

        GetComponent<BetterJump>().enabled = false;
        DOVirtual.Float(6, 0, 0.5f, RigidbodyDrag);
        isSteamDash = true;
        haveSteamDash = false;
        isANDash = false; isDash = false;
        RB.velocity = new Vector2(x, yRaw == 0 ? 0.2f : y).normalized * 35;

        for (float i = 0; i < 0.5f; i += Time.deltaTime)
        {
            yield return YieldInstructionCache.WaitForFixedUpdate;
        }
        isSteamDash = false;

        GetComponent<BetterJump>().enabled = true;
    }

    public Collision2D LastCollision;
    private void ChargeDash()
    {
        float xraw = setRaw().x;
        float yraw = setRaw().y;

        DashTime = 0.1f;
        KeyBreakTime = 0.1f;
        DOVirtual.Float(0.1f, 1, 0.12f, timedrag).SetEase(Ease.InCirc);
        CollisonTime = 0.1f;

        xraw = setRaw().x;
        yraw = setRaw().y;
        if (xraw == 0 && yraw == 0)
        {
            xraw = LastCollision.contacts[0].normal.x > 0 ? 1 : -1;
            yraw = LastCollision.contacts[0].normal.y > 0 ? 1 : -1;
        }

        Vector2 lastVelocity = new Vector3(xraw, yraw);
        float x = lastVelocity.x == 0 ? 0 : (lastVelocity.x > 0 ? 200 : -200);
        float y = lastVelocity.y == 0 ? 0 : (lastVelocity.y > 0 ? 200 : -200);

        float Revert_X = lastVelocity.x == 0 ? 0 : lastVelocity.x * (y / lastVelocity.y);
        float Revert_Y = lastVelocity.y == 0 ? 0 : lastVelocity.y * (x / lastVelocity.x);

        if (Mathf.Abs(lastVelocity.x) > Mathf.Abs(lastVelocity.y))
        {
            lastVelocity = new Vector3(x, Revert_Y);
        }

        else if (Mathf.Abs(lastVelocity.x) <= Mathf.Abs(lastVelocity.y))
        {
            lastVelocity = new Vector3(Revert_X, y);
        }

        var speed = 250;

        RB.velocity = new Vector3(xraw, yraw).normalized * Mathf.Max(speed, 0f);
        Debug.Log("speed" + speed + " Velocity" + RB.velocity);

        haveDash = true;
        isDash = true;
        isSteamDash = false;

        StartCoroutine(AfterDashWait(true));
    }
    private Vector2 setRaw()
    {
        float xraw = Input.GetAxisRaw("Horizontal");
        float yraw = Input.GetAxisRaw("Vertical");

        if (LastCollision.contacts[0].normal.x < 0 && xraw > 0)
        {
            xraw = 0;
            yraw = 1;
        }
        else if (LastCollision.contacts[0].normal.x > 0 && xraw < 0)
        { 
            xraw = 0;
            yraw = 1;
        }
        if (LastCollision.contacts[0].normal.y < 0 && yraw > 0) yraw = 0;
        else if (LastCollision.contacts[0].normal.y > 0 && yraw < 0) yraw = 0;

        return new Vector2(xraw, yraw);
    }
    private void Dash(float x, float y)
    {
        DashTime = 0.1f;
        haveDash = false;
        isSteamDash = false;
        StartCoroutine(DashWait(x, y));
        Invoke(nameof(getDash), 0.01f);
    }
    private void getDash() => haveSteamDash = true;

    void CircleColliderDrag(float x)
    {
        mHitBox.size = new Vector2(x * mHitSize.x, 1.875f * mHitSize.y);
        CPCOL.size = new Vector2(x, 1.875f);
    }
    IEnumerator DashWait(float x, float y)
    {
        springTime = 0;
        RB.gravityScale = 0;
        Invoke("InvDash", .1f);
        AN.SetTrigger("dash");
        isDash = true;
        isANDash = true;
        GetComponent<BetterJump>().enabled = false;
        CPCOL.size = new Vector2(1.875f, 1.875f);
        mHitBox.size = new Vector2(1.875f * mHitSize.x, 1.875f * mHitSize.y);

        RB.velocity = new Vector2(x, y != 0 ? y : 0.1f).normalized * 65;
        StartCoroutine(GroundDash());
        DOVirtual.Float(6, 0, 2f, RigidbodyDrag);

        GetComponent<BetterJump>().enabled = false;
        isWallJump = true;
        if (xRaw != 0)
        {
            while ((RB.velocity.x < 0 ? RB.velocity.x * -1 : RB.velocity.x) > maxSpeed)
            {
                if (isPipe || isSteamDash || springTime > 0) break;
                yield return YieldInstructionCache.WaitForFixedUpdate;
            }
        }
        else if (xRaw == 0)
        {
            while ((RB.velocity.y < 0 ? RB.velocity.y * -0.4f : RB.velocity.y) > maxSpeed)
            {
                if (isPipe || isSteamDash || springTime > 0) break;
                yield return YieldInstructionCache.WaitForFixedUpdate;
            }
        }

        if (!isPipe)
        {
            GetComponent<BetterJump>().enabled = true;
            isWallJump = false;
            isDash = false;
            CPCOL.size = new Vector2(1, 1.875f);
            mHitBox.size = new Vector2(1 * mHitSize.x, 1.875f * mHitSize.y);
        }

        yield return null;
    }
    private void InvDash()
    {
        if (!isPipe) RB.gravityScale = 3;
    }

    IEnumerator AfterDashWait(bool isShake)
    {
        springTime = 0;
        RB.gravityScale = 0;
        Invoke("InvDash", .1f);
        AN.SetTrigger("dash");
        isDash = true;
        isANDash = true;
        haveSteamDash = true;
        GetComponent<BetterJump>().enabled = false;

        yield return YieldInstructionCache.WaitForFixedUpdate;
        //DOVirtual.Float(1.067f, 2, 0.25f, CircleColliderDrag).SetEase(Ease.InCirc);
        CPCOL.size = new Vector2(1.875f, 1.875f);
        mHitBox.size = new Vector2(1.875f * mHitSize.x, 1.875f * mHitSize.y);
        StartCoroutine(GroundDash());
        DOVirtual.Float(9, 0, 1.5f, RigidbodyDrag);
        isWallJump = true;

        if(isShake)
        {
            CinemacineSize = mCameraSize * 1.4f;
            CinemachineShake.Instance.ShakeCamera(12, 0.4f);
        }
        else
            if (isSetCameraSize) CinemacineSize = mCameraSize * 0.87f;

        float AbsX = RB.velocity.x < 0 ? RB.velocity.x * -1 : RB.velocity.x;
        float AbsY = RB.velocity.y < 0 ? RB.velocity.y * -1 : RB.velocity.y;
        if (AbsX > AbsY)
        {
            //Debug.Log("Xup" + RB.velocity);
            while ((RB.velocity.x < 0 ? RB.velocity.x * -1 : RB.velocity.x) > maxSpeed)
            {
                if (isPipe || isSteamDash || springTime > 0) break;
                isDash = true;
                yield return YieldInstructionCache.WaitForFixedUpdate;
            }
        }
        else if (AbsX <= AbsY)
        {
            //Debug.Log("Yup" + RB.velocity);
            while ((RB.velocity.y < 0 ? RB.velocity.y * -1 : RB.velocity.y) > maxSpeed)
            {
                if (isPipe || isSteamDash || springTime > 0) break;
                isDash = true;
                yield return YieldInstructionCache.WaitForFixedUpdate;
            }
        }

        if(!isPipe)
        {
            GetComponent<BetterJump>().enabled = true;
            isWallJump = false;
            isDash = false;
            CPCOL.size = new Vector2(1, 1.875f);
            mHitBox.size = new Vector2(1 * mHitSize.x, 1.875f * mHitSize.y);
        }
        if(!isDeath) CinemacineSize = mCameraSize;

        yield return null;
    }
    public void AfterImage_Ins()
    {
        if (isPipe) return;
        GameObject afterImage = Instantiate(mAfterImage, transform.position, Quaternion.identity);
        afterImage.GetComponent<SpriteRenderer>().flipX = SR.flipX;
        afterImage.GetComponent<SpriteRenderer>().sprite = SR.sprite;
    }
    IEnumerator GroundDash()
    {
        yield return YieldInstructionCache.WaitForSeconds(.15f);
        if (COL.onGround)
            haveDash = true;
    }

    public void Floating(Vector2 Rot)
    {
        float Y_vel = RB.velocity.y > 40 ? 40 : RB.velocity.y;
        Y_vel = Y_vel < 0 ? 0 : Y_vel;
        RB.AddForce(Rot * Time.deltaTime * 15000, ForceMode2D.Force);
        PushTime = 0.1f;
    }
    public IEnumerator Spring(Vector2 angle)
    {
        if(angle.x != 0) AN.SetTrigger("steamdash");
        springTime = 1;
        yield return YieldInstructionCache.WaitForFixedUpdate;
        GetComponent<BetterJump>().enabled = true;
        RB.velocity = new Vector2(angle.x == 0 ? RB.velocity.x : angle.x, angle.y == 0 ? RB.velocity.y : angle.y);
        haveDash = true;
    }
    public void Death()
    {
        isDeath = true;
        mCinemachineTransposer.m_XDamping = 5;
        mCinemachineTransposer.m_YDamping = 5;
        DOVirtual.Float(5, 15, 1.25f, LightRadius).SetEase(Ease.OutCirc);
        MapConfiner.SetActive(false);
        DeathConfiner.SetActive(true);
        RB.bodyType = RigidbodyType2D.Static;
        AN.SetTrigger("death");
        AN.SetBool("isDeath", true);
        transform.SetParent(null);
        CinemacineSize = 6;
        CinemachineShake.Instance.ShakeCamera(5, 0.6f);
    }
    public IEnumerator EffectDeath()
    {
        GameObject particle = Instantiate(mDeathParticle, transform.position + new Vector3(0.2f, -0.4f, -0.5f), Quaternion.Euler(-90, 0, 0));
        yield return YieldInstructionCache.WaitForSeconds(.3f);
        SR.DOFade(0, 0.6f).SetEase(Ease.InCirc);
        yield return YieldInstructionCache.WaitForSeconds(2f);
        DIE.SceneLoad();
    }

    private void LightRadius(float x)
    {
        mBackGroundLight.pointLightOuterRadius = x;
        mPlatformLight.pointLightOuterRadius = x * 1.6f;
    }
    //_____________________________//

    private void WallSlide()
    {
        if (isDash) return;

        if (!isSlide)
        {
            AN.SetTrigger("clim");
            canWallSlide = true;
            isSlide = true;
            isWallJump = false;
            Timer = 0;
        }
        if (canWallSlide)
        {
            RB.gravityScale = 0;
            haveDash = true;
            isSteamDash = false;
            RB.velocity = new Vector2(RB.velocity.x, 0);

            SR.flipX = COL.onRightWall;
        }
    }
    private void ElseWallSlide()
    {
        RB.gravityScale = 3;
        isSlide = false;
        if (COL.onGround)
        {
            haveDash = true;
            isSteamDash = false;
            isWallJump = false;
        }
    }
    private void WallSlope()
    {
        RB.gravityScale = 0;
        haveDash = true;
        isSteamDash = false;
        RB.velocity = new Vector2(0, 0);

        if (COL.onSlopeRightWall) SR.flipX = true;
        else if (COL.onSlopeLeftWall) SR.flipX = false;
    }


    public void SteamPluck()
    {
        m_SteamMachine.sprite = m_ChangeSteamMachine;
        OnShake();
    }
    public void OnShake()
    {
        CinemachineShake.Instance.ShakeCamera(6, 0.3f);
    }

    bool PlayerFlip()
    {
        bool flipSprite = (SR.flipX ? x > 0f : x < 0f);
        if(flipSprite)
        {
            if(!isSteamDash) SR.flipX = !SR.flipX;
        }
        return flipSprite;
    }

    IEnumerator DisableMovement(float time)
    {
        isCanMove = false;
        yield return YieldInstructionCache.WaitForSeconds(time);
        isCanMove = true;
    }
    public IEnumerator PipeMove(Vector3 pos, Vector3 startpos)
    {
        transform.position = startpos;

        setPos = pos * -2.6875f;
        RB.velocity = Vector2.zero;
        GetComponent<BetterJump>().enabled = false;

        RB.gravityScale = 0;
        SR.enabled = false;
        CPCOL.enabled = false;
        

        isPipe = true;
        isDash = false;

        var collision = Physics2D.OverlapCircle(transform.position + setPos, 0.2f, COL.groundLayer);
        while (true)
        {
            CinemacineSize = 7.5f;
            transform.position += setPos * mPipelineSpeed * Time.deltaTime;
            collision = Physics2D.OverlapCircle(transform.position + setPos, 0.2f, COL.groundLayer);
            if(!collision)
            {
                var rightcollision = Physics2D.OverlapCircle(transform.position + new Vector3(setPos.y, setPos.x)*2, 0.2f, COL.groundLayer);
                var leftcollision = Physics2D.OverlapCircle(transform.position + new Vector3(-setPos.y, -setPos.x)*2, 0.2f, COL.groundLayer);
                if (rightcollision) setPos = new Vector3(setPos.y, setPos.x);
                else if (leftcollision) setPos = new Vector3(-setPos.y, -setPos.x);
                else if (!rightcollision && !leftcollision) break;
            }
            yield return YieldInstructionCache.WaitForFixedUpdate;
        }
        SR.enabled = true;
        for (float i = 0; i < 0.135f; i += Time.deltaTime)
        {
            transform.position += setPos * mPipelineSpeed * Time.deltaTime;
            yield return YieldInstructionCache.WaitForFixedUpdate;
        }
        GetComponent<BetterJump>().enabled = true;
        RB.gravityScale = 3;
        CPCOL.enabled = true;
        isWallJump = false;
        isPipe = false;
        haveDash = true;
        CinemacineSize = mCameraSize;

        yield return null;
    }

    void RigidbodyDrag(float x)
    {
        if(springTime <= 0)
            RB.drag = x;
        else
            RB.drag = 0;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Platform") && isDash)
        {
            DashTime = 0.1f;
            KeyBreakTime = 0.1f;
            var speed = LastVelocity.magnitude;
            var dir = Vector2.Reflect(LastVelocity.normalized, collision.contacts[0].normal);

            RB.velocity = dir * Mathf.Max(speed, 0f);
            haveDash = true;
            isSteamDash = false;
        }
        else if(collision.transform.CompareTag("Platform") && COL.onGround)
        {
            Instantiate(FallDustEffect, transform.position, Quaternion.identity);
        }
        if (collision.transform.CompareTag("Boost") && isDash)
        {
            DOVirtual.Float(0.1f, 1f, 0.12f, timedrag).SetEase(Ease.InCirc);
            DashTime = 0.1f;
            KeyBreakTime = 0.1f;
            CollisonTime = 0.1f;

            float x = LastVelocity.x > 0 ? 200 : -200;
            float y = LastVelocity.y > 0 ? 200 : -200;

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

            var speed = 250;
            var dir = Vector2.Reflect(LastVelocity.normalized, collision.contacts[0].normal);

            RB.velocity = dir * Mathf.Max(speed, 0f);
            Debug.Log("speed" + speed + " Velocity" + RB.velocity);

            haveDash = true;
            isDash = true;
            isSteamDash = false;

            StartCoroutine(AfterDashWait(true));
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("DashDown"))
        {
            haveDash = true;
            isSteamDash = false;
            Destroy(collision.gameObject);
        }
        if(collision.transform.CompareTag("RadioBox") && !isCutScene && !GameManager.GM.onRadio)
        {
            collision.GetComponent<SceneEvent>().Event();
        }
    }
    void timedrag(float x) => Time.timeScale = x;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position + setPos, 0.2f);
    }
}
