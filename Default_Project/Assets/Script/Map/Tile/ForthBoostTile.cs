using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForthBoostTile : MonoBehaviour
{
    public GameObject[] m_DamageTile = new GameObject[4];
    private Animator[] m_DamageAnimator = new Animator[4];
    private PolygonCollider2D[] m_DamageCollider = new PolygonCollider2D[4];

    public bool isFour;
    public bool isHorizontal;
    public float LoopTime;
    private int LoopCount;

    private void Start()
    {
        for(int i = 0; i < 4; i++)
        {
            m_DamageAnimator[i] = m_DamageTile[i].GetComponent<Animator>();
            m_DamageCollider[i] = m_DamageTile[i].GetComponent<PolygonCollider2D>();
        }
        StartCoroutine(LoopDamage());
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

    private IEnumerator LoopDamage()
    {
        if(isFour)
        {
            while (true)
            {
                for (int i = 0; i < 4; i++)
                {
                    bool able = isHorizontal ? (LoopCount * 2 == i) || (LoopCount * 2 + 1 == i) : (LoopCount == i) || (3 - LoopCount == i);
                    m_DamageCollider[i].enabled = able;
                    m_DamageAnimator[i].SetBool("onDamage", able);
                }
                yield return YieldInstructionCache.WaitForSeconds(LoopTime);
                LoopCount++;
                if (LoopCount > 1) LoopCount = 0;
            }
        }
        else if(!isFour)
        {
            while (true)
            {
                for (int i = 0; i < 4; i++)
                {
                    bool able = i == LoopCount;
                    m_DamageCollider[i].enabled = able;
                    m_DamageAnimator[i].SetBool("onDamage", able);
                }
                yield return YieldInstructionCache.WaitForSeconds(LoopTime);
                LoopCount++;
                if (LoopCount > 3) LoopCount = 0;
            }
        }
    }
}
