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
    public float mCameraSize;
    public float CinemacineSize;
    public float real_CineSize;
    private float plus_CineSize = 1;

    private Rigidbody2D RB;
    private SpriteRenderer SR;
    private CapsuleCollider2D CPCOL;
    private Animator AN;
    private BoxCollider2D BXCOL;

    //__________________________//

    private Collison COL;
    private Death DIE;
    private SetConfiner m_SetConfiner;
    private AnimationScript Player_Animation;
    private AudioSource m_AuidoSource;

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
    public float xRaw;
    public float yRaw;
    public float AddMaxSpeed;
    public float m_CutX = 0;

    [Space]
    [Header("Time")]
    public float m_CoyoteTime;
    public float m_CoyoteCount;
    public float BufferingTime;
    public float BufferingCount;

    [Space]
    [Header("Dash")]
    public int DashCount;

    [Space]
    [Header("CheckPoint")]
    public Transform mCheckPoint;

    [Space]
    [Header("HitBox")]
    public Vector2 mHitSize;

    [Space]
    [Header("Bool")]
    public bool isCutScene;
    public bool isCalledScene;
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
    public bool isStopY = false;

    [Space]
    [Header("GetBool")]
    public bool GetDash;
    public bool GetSteamDash;

    private bool isMove = false;
    private bool canWallSlide;
    private bool isObserve = false;
    private bool isInvDeath = false;

    private bool isLanding = false;

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
    private float steamTime;
    public Vector3 LastVelocity;

    private Transform MainCamera;
    private Transform mLastMainTransform;

    public Transform m_TargetCamera;
    public Vector3 m_TargetPlus;

    public Vector3 setPos;

    public SpriteRenderer m_SteamMachine;
    public Sprite m_ChangeSteamMachine;
    public GameObject FallDustEffect;
    public GameObject JumpDustEffect;
    public GameObject SteamDustEffect;
    public GameObject SteamDustVerticalEffect;
    public GameObject BoosterDustEffect;

    [Space]
    [Header("Death")]
    public GameObject MapConfiner;
    public GameObject DeathConfiner;
    public GameObject LightParents;

    [Space]
    [Header("AudioClip")]
    public AudioClip m_WalkClip;
    public AudioClip m_RunClip;
    public AudioClip m_DashClip;
    public AudioClip m_DeathClip;
    public AudioClip m_SteamPlunkClip;
    public GameObject m_LandClipPref;
    public GameObject m_SteamClipPref;
    public GameObject m_BoosterClipPref;

    [Space]
    [Header("Light")]
    public UnityEngine.Rendering.Universal.Light2D mBackGroundLight;
    public UnityEngine.Rendering.Universal.Light2D mPlatformLight;
    public UnityEngine.Rendering.Universal.Light2D mPlatformLight2;


    private void Start()
    {
        Time.timeScale = GameTime;
        RB = GetComponent<Rigidbody2D>();
        SR = GetComponent<SpriteRenderer>();
        CPCOL = GetComponent<CapsuleCollider2D>();
        COL = GetComponent<Collison>();
        DIE = GetComponent<Death>();
        AN = GetComponent<Animator>();
        BXCOL = GetComponent<BoxCollider2D>();
        m_SetConfiner = GetComponent<SetConfiner>();
        m_AuidoSource = GetComponent<AudioSource>();
        Invoke(nameof(InvokeAble), 0.02f);

        Player_Animation = GetComponent<AnimationScript>();

        mHitBox.size = new Vector2(1 * mHitSize.x, 1.875f * mHitSize.y);

        MainCamera = Camera.main.transform;
        mCinemachineTransposer = cinevirtual.GetCinemachineComponent<CinemachineTransposer>();
    }
    private void InvokeAble()
    {
        m_SetConfiner.enabled = true;
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
        real_CineSize = Mathf.Lerp(real_CineSize, CinemacineSize * plus_CineSize, Time.deltaTime * 3);
        cinevirtual.m_Lens.OrthographicSize = real_CineSize;

        if (isDeath)
        {
            transform.SetParent(null);
            return;
        }
        if (isPipe || isInteraction)
        {
            isDash = false;
            AN.SetBool("run", false);
            xRaw = 0; yRaw = 0;
            return;
        }

        //_____________________________//

        bool xLeft = Input.GetKey(KeySetting.keys[KeyAction.LEFT]);
        bool xRight = Input.GetKey(KeySetting.keys[KeyAction.RIGHT]);

        if(xRaw == 0)
        {
            xRaw = xRight ? 1 : xLeft ? -1 : 0;
        }
        else if(xRaw == -1)
        {
            xRaw = xLeft ? -1 : 0;
        }
        else if(xRaw == 1)
        {
            xRaw = xRight ? 1 : 0;
        }
        yRaw = Input.GetKey(KeySetting.keys[KeyAction.DOWN]) && Input.GetKey(KeySetting.keys[KeyAction.UP]) ? 0 : Input.GetKey(KeySetting.keys[KeyAction.DOWN]) ? -1 : Input.GetKey(KeySetting.keys[KeyAction.UP]) ? 1 : 0;

        //_____________________________//

        if ((Input.GetKeyDown(KeySetting.keys[KeyAction.JUMP])) && !isCutScene) Jump();
        if (Input.GetKeyDown(KeySetting.keys[KeyAction.STEAM]) && steamTime > 0 && haveSteamDash && GetSteamDash) StartCoroutine(SteamDash(xRaw, yRaw));
        mAimObject.SetActive(COL.onSlope);

        if (COL.onSlope && !isANDash)
        {
            plus_CineSize = 1.2f;
            float xraw = setRaw().x;
            float yraw = setRaw().y;
            float angle = Mathf.Atan2(yraw, xraw) * Mathf.Rad2Deg;
            if (xraw == 0 && yraw == 0)
            {
                angle = Mathf.Atan2(LastCollision.contacts[0].normal.y, LastCollision.contacts[0].normal.x) * Mathf.Rad2Deg;
                m_TargetCamera.position = Vector3.Lerp(m_TargetCamera.position, transform.position + (new Vector3(LastCollision.contacts[0].normal.x, LastCollision.contacts[0].normal.y).normalized + new Vector3(LastCollision.contacts[0].normal.x * 1.25f, 0)) * 10, 1);
            }
            else m_TargetCamera.position = Vector3.Lerp(m_TargetCamera.position, transform.position + (new Vector3(xraw, yraw).normalized + new Vector3(LastCollision.contacts[0].normal.x * 1.25f, 0)) * 10, 1);

            Quaternion angleAxis = Quaternion.AngleAxis(angle - 90f, Vector3.forward);
            Quaternion rotation = Quaternion.Slerp(mAimObject.transform.rotation, angleAxis, 25 * Time.deltaTime);
            mAimObject.transform.rotation = rotation;

            mCinemachineTransposer.m_XDamping = 2f;
            mCinemachineTransposer.m_YDamping = 2f;
        }
        else if(!isCutScene)
        {
            plus_CineSize = 1;

            m_TargetCamera.position = Vector3.Lerp(m_TargetCamera.position, transform.position + m_TargetPlus, 0.75f);
            mCinemachineTransposer.m_XDamping = 1;
            if (!isStopY) mCinemachineTransposer.m_YDamping = 0.6f;
            else mCinemachineTransposer.m_YDamping = 20;
        }

        if (KeyBreakTime > 0 && Input.GetKeyDown(KeySetting.keys[KeyAction.DASH]))
        {
            isKeyBreak = true;
            KeyBreakXY = new Vector2(xRaw, yRaw);
        }
        if (isKeyBreak && !isCutScene && GetDash)
        {
            if (!COL.onGround && (xRaw != 0 || yRaw != 0) && DashTime <= 0 && (((COL.onRightWall && xRaw == -1) || (COL.onLeftWall && xRaw == 1)) || !COL.onWall))
            {
                isKeyBreak = false;
                Dash(KeyBreakXY.x, KeyBreakXY.y);
            }
        }

        if (Input.GetKeyDown(KeySetting.keys[KeyAction.DASH]) && haveDash && !isCutScene && GetDash)
        {
            if (COL.onSlope) ChargeDash();
            else if (!COL.onGround && (xRaw != 0 || yRaw != 0) && DashTime <= 0 && (((COL.onRightWall && xRaw == -1) || (COL.onLeftWall && xRaw == 1)) || !COL.onWall)) Dash(xRaw, yRaw);
        }

        if(COL.onGround && !isLanding && !isDash)
        {
            isLanding = true;
            Instantiate(FallDustEffect, transform.position + new Vector3(0, -0.125f), Quaternion.identity);
        }
        else if(!COL.onGround)
        {
            isLanding = false;
        }


        //_____________________________//

        CollisonTime -= Time.deltaTime;
        KeyBreakTime -= Time.deltaTime;
        DashTime -= Time.deltaTime;
        Timer += Time.deltaTime;
        mANDahsTime -= Time.deltaTime;
        steamTime -= Time.deltaTime;
        PushTime -= Time.deltaTime;
        m_CoyoteCount -= Time.deltaTime;

        //_____________________________//

        if (RB.velocity.x > 0.02f) SR.flipX = false;
        else if (RB.velocity.x < -0.02f) SR.flipX = true;
       /* if (COL.onSlopeRightWall) SR.flipX = true;
        else if (COL.onSlopeLeftWall) SR.flipX = false;*/

        if (isDash) mANDahsTime = 0.2f;
        if (mANDahsTime > 0) isANDash = true;
        else if (mANDahsTime <= 0) isANDash = false;
        if (isANDash) steamTime = 0.2f;

        //_____________________________//

        if (transform.position.y < -40) Death(false);

        LastVelocity = RB.velocity;

        if (COL.onWall && !COL.onGround) WallSlide();
        else if (!COL.onWall || COL.onGround) ElseWallSlide();
        if (COL.onSlope && !isANDash) WallSlope();
    }
    private void FixedUpdate()
    {
        if (GameManager.GM.onPause) return;

        if (isDeath) return;
        if (isInteraction) RB.velocity = new Vector2(0, RB.velocity.y);
        if (isPipe || isInteraction || COL.onSlope) return;

        Player_Animation.SetHorizontalMovement(xRaw, yRaw, RB.velocity.y, xRaw); Walk();
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
            bool isRSound = AN.GetCurrentAnimatorStateInfo(0).IsName("Player_Run");
            bool isWSound = AN.GetCurrentAnimatorStateInfo(0).IsName("Player_Walk");
            if(!isRSound && !isWSound) m_AuidoSource.Stop();
            else if (isRSound)
            {
                m_AuidoSource.clip = m_RunClip;
                if (!m_AuidoSource.isPlaying)
                    m_AuidoSource.Play();
            }
            else if (isWSound)
            {
                m_AuidoSource.clip = m_WalkClip;
                if (!m_AuidoSource.isPlaying)
                    m_AuidoSource.Play();
            }
            else
                m_AuidoSource.Stop();
            return;
        }
        if (!isCanMove || isDash || CollisonTime > 0 || isObserve)
        {
            if (isObserve) RB.velocity = Vector2.zero;
            AN.SetBool("run", false);
            return;
        }

        if (isWalk) AN.SetBool("isWalk", xRaw != 0);
        else AN.SetBool("run", xRaw != 0);
         
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
                RB.velocity = new Vector2(xRaw * mMoveSpeed * 0.15f * Time.fixedDeltaTime, RB.velocity.y);
            }
            else if ((MathF.Abs(RB.velocity.x) < maxSpeed || PlayerFlip()) || (PushTime >= 0 && MathF.Abs(RB.velocity.x) < maxSpeed))
            {
                RB.AddForce(new Vector2(xRaw * mMoveSpeed * Time.fixedDeltaTime, 0));
            }
            else if (PushTime >= 0)
            {
                RB.AddForce(new Vector2(xRaw * mMoveSpeed * Time.fixedDeltaTime * 0.1f, 0));
            }
            else if (!isDash && !isSteamDash)
            {
                CPCOL.size = new Vector2(1, 1.875f);
                mHitBox.size = new Vector2(1 * mHitSize.x, 1.875f * mHitSize.y);

                if (isWalk) RB.velocity = new Vector2(xRaw * mMoveSpeed * 0.15f * 0.2f * Time.fixedDeltaTime, RB.velocity.y);
                else RB.velocity = new Vector2(xRaw * mMoveSpeed * 0.15f * Time.fixedDeltaTime, RB.velocity.y);
            }
        }

        bool isRunSound = AN.GetCurrentAnimatorStateInfo(0).IsName("Player_Run");
        bool isWalkSound = AN.GetCurrentAnimatorStateInfo(0).IsName("Player_Walk");
        if (isDash || isDeath)
        {
            m_AuidoSource.Stop();
        }
        else if (isRunSound)
        {
            m_AuidoSource.clip = m_RunClip;
            if (!m_AuidoSource.isPlaying)
                m_AuidoSource.Play();
        }
        else if(isWalkSound)
        {
            m_AuidoSource.clip = m_WalkClip;
            if (!m_AuidoSource.isPlaying)
                m_AuidoSource.Play();
        }
        else
            m_AuidoSource.Stop();
    }

    public void Jump()
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
        else if (COL.onGround || m_CoyoteCount > 0)
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
        m_CoyoteCount = 0;
    }

    private IEnumerator SteamDash(float x, float y)
    {
        Instantiate(m_SteamClipPref);
        m_AuidoSource.Stop();
        m_AuidoSource.clip = m_DashClip;
        m_AuidoSource.Play();

        m_CoyoteCount = 0;
        DashTime = 0.1f;
        KeyBreakTime = 0f;
        isKeyBreak = false;
        AN.SetTrigger("steamdash");

        if (x == 0)
        {
            GameObject dust = Instantiate(SteamDustVerticalEffect, transform.position, Quaternion.identity);
            dust.transform.localScale = new Vector3(x == 0 ? (SR.flipX ? -1 : 1) : (x < 0 ? -1 : 1), y == 0 ? 1 : y);
        }
        else
        {
            GameObject dust = Instantiate(SteamDustEffect, transform.position, Quaternion.identity);
            dust.transform.localScale = new Vector3(x == 0 ? (SR.flipX ? -1 : 1) : (x < 0 ? -1 : 1), 1);
        }

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
        m_CoyoteCount = 0;
        float xraw = setRaw().x;
        float yraw = setRaw().y;

        DashTime = 0.1f;
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

        haveDash = true;
        isDash = true;
        isSteamDash = false;

        StartCoroutine(AfterDashWait(true));
    }
    private Vector2 setRaw()
    {
        if (LastCollision.contacts[0].normal.x < 0 && xRaw > 0)
        {
            xRaw = 0;
            yRaw = 1;
        }
        else if (LastCollision.contacts[0].normal.x > 0 && xRaw < 0)
        { 
            xRaw = 0;
            yRaw = 1;
        }
        if (LastCollision.contacts[0].normal.y < 0 && yRaw > 0) yRaw = 0;
        else if (LastCollision.contacts[0].normal.y > 0 && yRaw < 0) yRaw = 0;

        return new Vector2(xRaw, yRaw);
    }
    public void SetSlopeCamera() => m_TargetCamera.position = transform.position + (new Vector3(LastCollision.contacts[0].normal.x, LastCollision.contacts[0].normal.y).normalized + new Vector3(LastCollision.contacts[0].normal.x * 1.25f, 0)) * 10;
    private void Dash(float x, float y)
    {
        m_CoyoteCount = 0;
        DashCount++;
        DashTime = 0.15f;
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
        m_CoyoteCount = 0;
        int thisCount = DashCount;

        springTime = 0;
        RB.gravityScale = 0;
        Invoke("InvDash", .1f);
        AN.SetTrigger("dash");
        isDash = true;
        isANDash = true;
        GetComponent<BetterJump>().enabled = false;
        CPCOL.size = new Vector2(1.875f, 1.875f);
        mHitBox.size = new Vector2(1.875f * mHitSize.x, 1.875f * mHitSize.y);

        RB.velocity = new Vector2(x != 0 ? x : 0f, y != 0 ? y : 0.1f).normalized * 65;
        StartCoroutine(GroundDash());
        DOVirtual.Float(6, 0, 2f, RigidbodyDrag);

        m_AuidoSource.Stop();
        m_AuidoSource.clip = m_DashClip;
        m_AuidoSource.Play();

        GetComponent<BetterJump>().enabled = false;
        isWallJump = true;
        if (x != 0)
        {
            while ((RB.velocity.x < 0 ? RB.velocity.x * -1 : RB.velocity.x) > maxSpeed || (thisCount < DashCount))
            {
                if (isPipe || isSteamDash || springTime > 0) break;
                yield return YieldInstructionCache.WaitForFixedUpdate;
            }
        }
        else if (x == 0)
        {
            while ((RB.velocity.y < 0 ? RB.velocity.y * -0.4f : RB.velocity.y) > maxSpeed || (thisCount < DashCount))
            {
                if (isPipe || isSteamDash || springTime > 0) break;
                yield return YieldInstructionCache.WaitForFixedUpdate;
            }
        }
        yield return YieldInstructionCache.WaitForFixedUpdate;
        if (!isPipe && thisCount >= DashCount)
        {
            GetComponent<BetterJump>().enabled = true;
            isWallJump = false;
            isDash = false;
            CPCOL.size = new Vector2(1, 1.875f);
            mHitBox.size = new Vector2(1 * mHitSize.x, 1.875f * mHitSize.y);
        }
        DashCount--;

        yield return null;
    }
    private void InvDash()
    {
        if (!isPipe) RB.gravityScale = 3;
    }

    IEnumerator AfterDashWait(bool isShake)
    {
        Instantiate(m_BoosterClipPref);
        m_CoyoteCount = 0;
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
        m_CoyoteCount = 0;
        springTime = 1;
        isDash = false;
        isANDash = false;
        mANDahsTime = 0;
        steamTime = 0;
        yield return YieldInstructionCache.WaitForFixedUpdate;
        GetComponent<BetterJump>().enabled = true;
        var vel = 5;
        Vector2 MaxVelocity = new Vector2(RB.velocity.x > vel ? vel : RB.velocity.x, RB.velocity.y > vel ? vel : RB.velocity.y);
        RB.velocity = new Vector2(angle.x == 0 ? MaxVelocity.x : angle.x, angle.y == 0 ? MaxVelocity.y : angle.y);
        haveDash = true;
    }
    public void Death(bool isInv)
    {
        isDeath = true;
        mCinemachineTransposer.m_XDamping = 5;
        mCinemachineTransposer.m_YDamping = 5;
        isInvDeath = isInv;
        cinevirtual.Follow = transform;
        m_AuidoSource.Stop();
        m_AuidoSource.clip = m_DeathClip;
        m_AuidoSource.Play();
        if (isInv)
        {
            DOVirtual.Float(7, 15, 1.25f, LightRadius).SetEase(Ease.OutCirc);

            CinemacineSize = 5.5f;
            CinemachineShake.Instance.ShakeCamera(5, 0.6f);
            MapConfiner.SetActive(false);
            DeathConfiner.SetActive(true);
        }
        else
        {
            DOVirtual.Float(7, 0, 0.25f, LightRadius).SetEase(Ease.InCirc);

            CinemacineSize = 10f;
            CinemachineShake.Instance.ShakeCamera(5, 0.6f);
        }
        RB.bodyType = RigidbodyType2D.Static;
        AN.SetTrigger("death");
        AN.SetBool("isDeath", true);
        transform.SetParent(null);
    }
    public IEnumerator EffectDeath()
    {
        GameObject particle = Instantiate(mDeathParticle, transform.position + new Vector3(0.2f, -0.4f, -0.5f), Quaternion.Euler(-90, 0, 0));
        yield return YieldInstructionCache.WaitForSeconds(.25f);
        SR.DOFade(0, 0.6f).SetEase(Ease.InCirc);
        yield return YieldInstructionCache.WaitForSeconds(isInvDeath ? 0.9f : 0f);
        Fade.instance.FadeIn(0.5f);
        yield return YieldInstructionCache.WaitForSeconds(0.5f);
        DIE.SceneLoad();
    }

    private void LightRadius(float x)
    {
        mBackGroundLight.pointLightOuterRadius = x;
        mPlatformLight.pointLightOuterRadius = x * 1.14f;
        mPlatformLight2.pointLightOuterRadius = x * 3.75f;
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
        BXCOL.enabled = COL.onGround && xRaw == 0 && !Input.GetKey(KeySetting.keys[KeyAction.JUMP]) && !isANDash && !COL.onSlope && RB.velocity.y > -1;
        
        RB.gravityScale = 3;
        isSlide = false;
        if (COL.onGround)
        {
            if (!AN.GetCurrentAnimatorStateInfo(0).IsName("Player_Jump") && !isDash) m_CoyoteCount = m_CoyoteTime;
            haveDash = true;
            isSteamDash = false;
            isWallJump = false;
        }
        else if(AN.GetCurrentAnimatorStateInfo(0).IsName("Player_Jump")) m_CoyoteCount = 0;
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
        m_AuidoSource.Stop();
        m_AuidoSource.clip = m_SteamPlunkClip;
        m_AuidoSource.Play();
        m_SteamMachine.sprite = m_ChangeSteamMachine;
        OnShake();
    }
    public void OnShake()
    {
        CinemachineShake.Instance.ShakeCamera(6, 0.3f);
    }

    bool PlayerFlip()
    {
        bool flipSprite = (SR.flipX ? xRaw > 0f : xRaw < 0f);
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

        var collision = Physics2D.OverlapCircle(transform.position + setPos, 0.2f, COL.pipeLayer);
        while (true)
        {
            CinemacineSize = 7.5f;
            transform.position += setPos * mPipelineSpeed * Time.deltaTime;
            collision = Physics2D.OverlapCircle(transform.position + setPos, 0.2f, COL.pipeLayer);
            if(!collision)
            {
                var rightcollision = Physics2D.OverlapCircle(transform.position + new Vector3(setPos.y, setPos.x)*2, 0.2f, COL.pipeLayer);
                var leftcollision = Physics2D.OverlapCircle(transform.position + new Vector3(-setPos.y, -setPos.x)*2, 0.2f, COL.pipeLayer);
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
            var speed = LastVelocity.magnitude;
            var dir = Vector2.Reflect(LastVelocity.normalized, collision.contacts[0].normal);
            RB.velocity = dir * Mathf.Max(speed, 0f);

            DashTime = 0.15f;
            KeyBreakTime = 0.1f;
            haveDash = true;
            isSteamDash = false;
        }
        if (collision.transform.CompareTag("Boost") && isDash)
        {
            DOVirtual.Float(0.1f, 1f, 0.12f, timedrag).SetEase(Ease.InCirc);

            float x = LastVelocity.x > 0 ? 200 : -200;
            float y = LastVelocity.y > 0 ? 200 : -200;

            float Revert_X = LastVelocity.x == 0 ? 0 : LastVelocity.x * (y / LastVelocity.y);
            float Revert_Y = LastVelocity.y == 0 ? 0 : LastVelocity.y * (x / LastVelocity.x);

            if (Mathf.Abs(LastVelocity.x) > Mathf.Abs(LastVelocity.y))
                LastVelocity = new Vector3(x, Revert_Y);
            else if (Mathf.Abs(LastVelocity.x) <= Mathf.Abs(LastVelocity.y))
                LastVelocity = new Vector3(Revert_X, y);

            var speed = 250;
            var dir = Vector2.Reflect(LastVelocity.normalized, collision.contacts[0].normal);

            RB.velocity = dir * Mathf.Max(speed, 0f);

            DashTime = 0.1f;
            CollisonTime = 0.1f;
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
            collision.GetComponent<Dash_Item>().TakeDash();
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("RadioBox") && !isCalledScene && !GameManager.GM.onRadio)
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
