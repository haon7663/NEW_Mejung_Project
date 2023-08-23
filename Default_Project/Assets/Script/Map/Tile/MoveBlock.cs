using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MoveBlock : MonoBehaviour
{
    private Animator mAnimator;
    private AudioSource m_AudioSource;

    public Vector3 mStartPosition;
    public Vector3 mLastPosition;
    public Vector3 mPlusPosition;

    public float mSpeed;
    public float timer;
    public float maxtime;

    public bool isInfinity;
    public bool isAuto;
    public bool lastPush;

    private bool isPush;
    private int xRaw;
    private bool isStartPush;
    private bool isOnetime;

    [Range(0, 1)]
    public float PosPersent;

    private GameObject mPlayer;
    private Move mPlayerMove;
    private Collider2D Push;

    private void Start()
    {
        mPlayer = GameObject.FindGameObjectWithTag("Player");
        mPlayerMove = mPlayer.GetComponent<Move>();
        mStartPosition = transform.position;
        mLastPosition = transform.position + mPlusPosition;
        transform.position = mStartPosition;

        mAnimator = GetComponentInChildren<Animator>();
        m_AudioSource = GetComponent<AudioSource>();
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
        Push = Physics2D.OverlapBox(transform.position, new Vector2(4f, 1.45f), 0, 1 << LayerMask.NameToLayer("Player"));

        mAnimator.SetBool("Move", false);

        if (isAuto)
        {
            if ((isInfinity || (!isInfinity && Push)) && PosPersent <= 0)
                isStartPush = true;
            else if (PosPersent <= 0)
                isStartPush = false;
        }

        if (isStartPush)
        {
            if (PosPersent <= 0)
            {
                if(!isOnetime)
                {
                    timer = 0;
                    xRaw = 0;
                    isOnetime = true;
                }
                if (timer <= 0.5f)
                {
                    xRaw = 0;
                    timer += Time.deltaTime;
                }
                else xRaw = 1;
            }
            else if (PosPersent >= 1)
            {
                if (isOnetime)
                {
                    timer = 0;
                    xRaw = 0;
                    isOnetime = false;
                }
                if (timer <= 0.5f)
                {
                    xRaw = 0;
                    timer += Time.deltaTime;
                }
                else xRaw = -1;
            }
            mAnimator.SetBool("Move", xRaw != 0);
            PosPersent += Time.deltaTime * mSpeed * xRaw;
            if (Push)
            {
                if (!isPush)
                {
                    isPush = true;
                    mPlayer.transform.SetParent(this.transform);
                }
            }
            else
            {
                if (isPush)
                {
                    mPlayer.transform.SetParent(null);
                    isPush = false;
                }
            }
        }
        else if(!isAuto)
        {
            if (lastPush != Push)
            {
                timer = 0;
            }
            mAnimator.SetBool("Move", false);
            if (Push)
            {
                isPush = true;
                timer += Time.deltaTime;
                if (timer > maxtime)
                {
                    if (PosPersent < 1)
                    {
                        mAnimator.SetBool("Move", true);
                        PosPersent += Time.deltaTime * mSpeed;
                    }
                    else if (PosPersent >= 1) PosPersent = 1;
                }
                mPlayer.transform.SetParent(this.transform);
            }
            else
            {
                timer += Time.deltaTime;
                if (timer > maxtime)
                {
                    if (PosPersent > 0)
                    {
                        mAnimator.SetBool("Move", true);
                        PosPersent -= Time.deltaTime * mSpeed;
                    }
                    else if (PosPersent <= 0) PosPersent = 0;
                }
                if (isPush)
                {
                    mPlayer.transform.SetParent(null);
                    isPush = false;
                }
            }
            lastPush = Push;
        }

        if (mAnimator.GetBool("Move") && !m_AudioSource.isPlaying) m_AudioSource.Play();
        else if(!mAnimator.GetBool("Move")) m_AudioSource.Stop();
        //if (Push) mPlayerMove.Resistance_MovetileSpeed = mPlusPosition.x * Time.deltaTime * mSpeed * xRaw;
        transform.position = Vector3.Lerp(transform.position, mStartPosition + mPlusPosition * PosPersent, Time.deltaTime * 10);
    }
    /*IEnumerator InvokeMove(float time, bool TF)
    {
        for(float i = 0; i <= time; i += Time.deltaTime)
        {
            if (Push != TF) break;
            yield return YieldInstructionCache.WaitForFixedUpdate;
        }
        if (Push == TF)
        {
            isMoveing = TF;
        }
        yield return null;
    }*/
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector2(4f, 1.45f));
    }
}
